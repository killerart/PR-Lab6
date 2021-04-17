using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lab5.Server
{
    class Program
    {
        static readonly string host = "127.0.0.1";
        const int port = 10000;
        static readonly IPEndPoint EP = new(IPAddress.Parse(host), port);
        static readonly UdpClient listener = new(EP);
        static void Main()
        {
            listener.BeginReceive(HandleConnection, listener);
            Console.WriteLine($"Server listening for UPD packets on port {port}\n");
            Console.ReadKey(true);
        }

        static void HandleConnection(IAsyncResult result)
        {
            listener.BeginReceive(HandleConnection, listener);
            IPEndPoint remoteEP = null;
            var bytes = listener.EndReceive(result, ref remoteEP);
            var message = Encoding.Default.GetString(bytes);
            Console.WriteLine($"Message '{message}' received from {remoteEP}");

            message = string.Join(string.Empty, message.Reverse());
            bytes = Encoding.Default.GetBytes(message);
            listener.Send(bytes, bytes.Length, remoteEP);
            Console.WriteLine($"Message '{message}' sent to {remoteEP}\n");
        }
    }
}
