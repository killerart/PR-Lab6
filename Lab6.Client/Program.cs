using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lab6.Client
{
    class Program
    {
        static async Task Main()
        {
            string host = "127.0.0.1";
            var hostIP = IPAddress.Parse(host);
            const int serverPort = 10000;
            var serverEP = new IPEndPoint(hostIP, serverPort);

            var tasks = Enumerable.Range(10001, 100).Select(port => Task.Run(() =>
            {
                try
                {

                    var EP = new IPEndPoint(hostIP, port);

                    using var client = new UdpClient(EP);

                    var message = "Hello there";
                    var bytes = Encoding.Default.GetBytes(message);
                    client.Send(bytes, bytes.Length, serverEP);
                    Console.WriteLine($"Message '{message}' sent from {EP} to {serverEP}");

                    IPEndPoint remoteEP = null;
                    bytes = client.Receive(ref remoteEP);
                    message = Encoding.Default.GetString(bytes);
                    Console.WriteLine($"Message '{message}' received from {remoteEP} to {EP}\n");
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }));
            await Task.WhenAll(tasks);
        }
    }
}
