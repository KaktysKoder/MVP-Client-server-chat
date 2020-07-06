using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientTCP
{
    internal sealed class Program
    {
        const string IP = "127.0.0.1";
        const int  PORT = 8080;

        static void Main()
        {
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);
            var tcpSocet = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Введите сообщение: ");
            var message = Console.ReadLine();

            var data = Encoding.UTF8.GetBytes(message);
            tcpSocet.Connect(tcpEndPoint);
            tcpSocet.Send(data);

            byte[] buffer = new byte[256];

            var size = 0;
            var answer = new StringBuilder();

            do
            {
                size = tcpSocet.Receive(buffer);

                answer.Append(Encoding.UTF8.GetString(buffer, 0, size));

            }
            while (tcpSocet.Available > 0);

            Console.WriteLine(answer.ToString());

            tcpSocet.Shutdown(SocketShutdown.Both);
            tcpSocet.Close();
        }
    }
}
