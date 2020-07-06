using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat
{
    internal sealed class Program
    {
        const string IP = "127.0.0.1";
        //TCP const int  PORT = 8080;

        //UDP
        const int PORT = 8081;

        private static void Main()
        {
            ServerUDP();
            //ServerTCP();
        }

        private static void ServerUDP()
        {
            var udpEndPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);
            var udpSocet    = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            udpSocet.Bind(udpEndPoint);

            while (true)
            {
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

                udpSocet.SendTo(Encoding.UTF8.GetBytes("Сообщние получено!"), senderEndPoint);

                Console.WriteLine(data);


            }

            //TODO: Off && Exit socet.
        }

        private static void ServerTCP()
        {
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);
            var tcpSocet = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            tcpSocet.Bind(tcpEndPoint);

            tcpSocet.Listen(5);

            while (true)
            {
                Socket listener = tcpSocet.Accept();
                byte[] buffer = new byte[256];

                var size = 0;
                var data = new StringBuilder();

                do
                {
                    size = listener.Receive(buffer);

                    data.Append(Encoding.UTF8.GetString(buffer, 0, size));

                }
                while (listener.Available > 0);

                Console.WriteLine(data);

                listener.Send(Encoding.UTF8.GetBytes("Success!"));

                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }
    }
}
