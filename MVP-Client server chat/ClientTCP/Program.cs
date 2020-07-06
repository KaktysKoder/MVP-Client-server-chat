using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientTCP
{
    internal sealed class Program
    {
        const string IP = "127.0.0.1";
        //TCP const int  PORT = 8080;

        //UDP
        const int PORT = 8082;

        static void Main()
        {
            ClientUDP();
            //ClientTCP();
        }

        private static void ClientUDP()
        {
            var udpEndPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);
            var udpSocet    = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            
            udpSocet.Bind(udpEndPoint);

            while (true)
            {
                Console.WriteLine("Введите сообщение: ");
                var message = Console.ReadLine();

                //udpSocet.Send(Encoding.UTF8.GetBytes(message));

                var serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081);
                udpSocet.SendTo(Encoding.UTF8.GetBytes("Сообщние получено!"), serverEndPoint);

                var buffer = new byte[256];
                var size   = 0;
                var data   = new StringBuilder();

                EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);

                do
                {
                    size = udpSocet.ReceiveFrom(buffer, ref senderEndPoint);

                    data.Append(Encoding.UTF8.GetString(buffer));
                }
                while (udpSocet.Available > 0);

                Console.WriteLine(data);

                Console.ReadLine();
            }
        }

        private static void ClientTCP()
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

            Console.ReadLine();
        }
    }
}
