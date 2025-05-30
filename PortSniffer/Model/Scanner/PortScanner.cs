using PortSniffer.Model.Config;
using PortSniffer.Presenter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace PortSniffer.Model.Scanner
{
    /// <summary>
    /// Types (enum) of scan states.
    /// </summary>
    public enum ScaningState
    {
        IDLE,
        SUSPENDED,
        RUNNING,
        CANCELED,
        FINISHED
    }

    /// <summary>
    /// Port scanner, handles the scanning.
    /// </summary>
    public class PortScanner
    {
        private readonly ManualResetEvent pauseEvent = new ManualResetEvent(true);
        private readonly object _lock = new object();

        private CancellationTokenSource cancelationTS;
        public ScaningState ScanState { get; set; } = ScaningState.IDLE;
        public Settings Settings { get; internal set; }

        public event Action<ScanResult> OpenPortsFoundEvent;

        public PortScanner()
        {

        }

        /// <summary>
        /// Scans a single IP address and port asynchronously, if cancelation token is requested, scan will stop.
        /// </summary>
        /// <param name="ip">Scanned IP</param>
        /// <param name="port">Scanned port</param>
        /// <param name="timeout">Scan timeout</param>
        /// <param name="token">Cancelation token</param>
        /// <returns>True if port is open, otherwise false</returns>
        private async Task<bool> ScanAsync(IPAddress ip, int port, int timeout, CancellationToken token)
        {
            using TcpClient tcpClient = new TcpClient();
            Task connection = tcpClient.ConnectAsync(ip, port);
            if (await Task.WhenAny(connection, Task.Delay(timeout, token)) == connection)
            {
                if (tcpClient.Connected)
                {
                    return true;
                }
            }
            return false;
        }

        //TODO: after everything is finished add option for a thing to first filter hosts by pinging if they are alive (might be usefull for quick scan)
        //TODO: might add % of how much is done. => better UX when pausing/stoping 

        /// <summary>
        /// Starts scanning, scanning will be done in parallel using the specified number of threads in the scanConfiguration (maximum threads waiting for responce from TCP client or Timeout).
        /// If cancelation is requested, scan will stop immediately.
        /// If pause event is set, the scan will pause after all currently scanned ports are proccessed => if there is a big timeout, the scan may take a while to pause.
        /// </summary>
        /// <param name="scanConfiguration">Configuration according to which it will scan</param>
        /// <returns>Task (void)</returns>
        public async Task StartScanAsync(ScanConfiguration scanConfiguration)
        {
            if (ScanState != ScaningState.IDLE) return;

            ScanState = ScaningState.RUNNING;

            cancelationTS = new CancellationTokenSource();
            var token = cancelationTS.Token;
            SemaphoreSlim semaphore = new SemaphoreSlim(scanConfiguration.MaxThreads);

            foreach (var ip in scanConfiguration.IPAddresses)
            {
                List<int> openPorts = new List<int>();
                List<Task> portTasks = new List<Task>();

                foreach (var port in scanConfiguration.Ports)
                {
                    if (token.IsCancellationRequested)
                    {
                        ScanState = ScaningState.CANCELED;
                        return;
                    }

                    await Task.Run(pauseEvent.WaitOne);
                    await semaphore.WaitAsync();

                    var task = Task.Run(async () =>
                    {
                        try
                        {
                            bool isOpen = await ScanAsync(ip, port, scanConfiguration.Timeout, token);
                            if (isOpen)
                            {
                                lock (_lock)
                                {
                                    openPorts.Add(port);
                                }
                            }
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }, token);

                    portTasks.Add(task);
                }

                await Task.WhenAll(portTasks);

                Debug.WriteLine("ALL TASK FINISHED");
                if (openPorts.Count > 0)
                {
                    ScanResult scanResult = new ScanResult(ip.ToString(), openPorts);
                    OpenPortsFoundEvent?.Invoke(scanResult);
                }
            }

            ScanState = ScaningState.FINISHED;
        }

        /// <summary>
        /// Pauses the scan.
        /// </summary>
        public void PauseScan()
        {
            pauseEvent.Reset();
            ScanState = ScaningState.SUSPENDED;
        }

        /// <summary>
        /// Resumes the scan.
        /// </summary>
        public void ResumeScan()
        {
            pauseEvent.Set();
            ScanState = ScaningState.RUNNING;
        }

        /// <summary>
        /// Cancels the scan.
        /// </summary>
        public void CancelScan()
        {
            cancelationTS?.Cancel();
            ScanState = ScaningState.IDLE;
        }
    }
}
