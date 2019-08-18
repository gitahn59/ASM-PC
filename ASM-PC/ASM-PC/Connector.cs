using System;
using System.Net;
using System.Net.Sockets;


namespace ASM_PC
{
    class Connector
    {

        public int Port { get; }
        public string Ip { get; }

        public UdpClient UdpServer { get; }

        public NetworkStream Stream { get; }
        public bool isCreated { get; }

        public Connector()
        {
            string s = "marst branch test";

            this.Port = 9876;
            this.Ip = GetIPAddress();

            UdpServer = new UdpClient(this.Port);

            IPEndPoint localAddress = new IPEndPoint(IPAddress.Any, 9876);

            while (true)
            {
                // receiving data
                byte[] dgram = this.UdpServer.Receive(ref localAddress);

                if (dgram.Length != 0)
                    break;

                //sending data
                //this.UdpServer.Send(dgram, dgram.Length, localAddress);
            }

            this.UdpServer.Close();
        }

        public static string GetIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }


    }
}
