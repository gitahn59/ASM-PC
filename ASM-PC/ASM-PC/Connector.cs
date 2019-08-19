using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Drawing;

namespace ASM_PC
{
    class Connector
    {

        public int Port { get; }
        public string Ip { get; }
        private IPEndPoint localAddress;

        public UdpClient UdpServer { get; }

        public NetworkStream Stream { get; }
        public bool isCreated { get; }

        public MemoryStream ms;
        public Image i;

        public Connector()
        {
            string s = "marst branch test";

            this.Port = 9876;
            this.Ip = GetIPAddress();

            this.localAddress = new IPEndPoint(IPAddress.Any,this.Port);
            UdpServer = new UdpClient(this.Port);

            //this.UdpServer.Close();
            ms = new MemoryStream();
        }

        public void GetImage()
        {
            int num = -1;

            while (true)
            {
                // receiving data
                byte[] dgram = this.UdpServer.Receive(ref this.localAddress);
                
                int count = dgram[dgram.Length - 1];
                if (num == -1)
                    num = count;
                else
                {
                    if (num != count)
                    {
                        try
                        {
                            i = Image.FromStream(ms);
                            break;
                        }
                        catch (Exception e)
                        {
                            i = null;
                            break;
                        }
                    }else
                    {
                        ms.Write(dgram, 0, dgram.Length);
                    }
                }
               
                
            }

            //sending data
            //this.UdpServer.Send(dgram, dgram.Length, localAddress);
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
