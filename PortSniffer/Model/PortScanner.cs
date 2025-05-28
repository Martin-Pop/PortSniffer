using PortSniffer.Presenter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PortSniffer.Model
{
    public enum ScaningState
    {
        IDLE,
        SUSPENDED,
        RUNNING,
        CANCELED
    }
    public class PortScanner
    {
        private readonly ManualResetEvent pauseEvent = new ManualResetEvent(true);
        private readonly object _lock = new object();
        
        private CancellationTokenSource cancelationTS;
        public ScaningState ScanState { get; set; } = ScaningState.IDLE;
        public event Action<ScanResult> OpenPortsFoundEvent;

        public PortScanner()
        {
            
        }

        private async Task<bool> ScanAsync(IPAddress ip, int port, int timeout)
        {
            using TcpClient tcpClient = new TcpClient();
            Task connection = tcpClient.ConnectAsync(ip, port);
            if (await Task.WhenAny(connection, Task.Delay(timeout)) == connection)
            {
                if (tcpClient.Connected)
                {
                    return true;
                }
            }
            return false;
        }

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
                        Debug.WriteLine("Scan canceled");
                        ScanState = ScaningState.CANCELED;
                        return;
                    }

                    //TODO: fix pause, add clear
                    await Task.Run(pauseEvent.WaitOne);
                    await semaphore.WaitAsync();

                    var task = Task.Run(async () =>
                    {
                        try
                        {
                            bool isOpen = await ScanAsync(ip, port, scanConfiguration.Timeout);
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
                    },token);

                    portTasks.Add(task);
                }

                
                await Task.WhenAll(portTasks);

                if (openPorts.Count > 0)
                {
                    ScanResult scanResult = new ScanResult(ip.ToString(), openPorts);
                    OpenPortsFoundEvent?.Invoke(scanResult);
                }
                //Debug.WriteLine($"IP: {ip} - Open Ports: {string.Join(", ", openPorts)}");
            }

            ScanState = ScaningState.IDLE;
            Debug.WriteLine("Set to idle down");
        }

        public void PauseScan()
        {    
            pauseEvent.Reset();
            ScanState = ScaningState.SUSPENDED;
            //Debug.WriteLine("Scan paused");
        }

        public void ResumeScan()
        {
            pauseEvent.Set();
            ScanState = ScaningState.RUNNING;
            //Debug.WriteLine("Scan resumed");
        }

        public void CancelScan()
        {
            cancelationTS?.Cancel();
            ScanState = ScaningState.IDLE;
        }
    }
}
