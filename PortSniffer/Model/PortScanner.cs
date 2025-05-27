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
    public class PortScanner
    {
        private readonly object _lock = new object();
        public PortScanner(){ }

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
            SemaphoreSlim semaphore = new SemaphoreSlim(scanConfiguration.MaxThreads);

            foreach (var ip in scanConfiguration.IPAddresses)
            {
                List<int> openPorts = new List<int>();
                List<Task> portTasks = new List<Task>();

                foreach (var port in scanConfiguration.Ports)
                {
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
                    });

                    portTasks.Add(task);
                }

                await Task.WhenAll(portTasks);

                Debug.WriteLine($"IP: {ip} - Open Ports: {string.Join(", ", openPorts)}");
            }
        }

        public async Task ScanTestAsync(List<IPAddress> ips, List<int> ports, int maxThreads)
        {
            SemaphoreSlim semaphore = new SemaphoreSlim(maxThreads);

            foreach (var ip in ips)
            {
                List<int> openPorts = new List<int>();
                List<Task> portTasks = new List<Task>();

                foreach (var port in ports)
                {
                    await semaphore.WaitAsync();

                    var task = Task.Run(async () =>
                    {
                        try
                        {
                            Debug.WriteLine($"Start fake scan {ip}:{port}");
                            await Task.Delay(300); // Simuluje čekání na scan
                            bool isOpen = (port % 2 == 0); // Falešný výsledek: otevřené jsou sudé porty
                            Debug.WriteLine($"End fake scan {ip}:{port} - {(isOpen ? "Open" : "Closed")}");

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
                    });

                    portTasks.Add(task);
                }

                Debug.WriteLine($"Before awaiting all ports for IP {ip}");
                await Task.WhenAll(portTasks);
                Debug.WriteLine($"After awaiting all ports for IP {ip}");

                Debug.WriteLine($"IP: {ip} - Open Ports: {string.Join(", ", openPorts)}");
            }
        }
    }
}
