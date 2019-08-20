using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Drawing;
using System.Threading;

namespace ASM_PC
{
    delegate void ImageArrivedEventHandler();

    class Connector
    {
        public event ImageArrivedEventHandler imageArrived;

        public int Port { get; }
        public string Ip { get; }
        private IPEndPoint localAddress;

        public Socket socket { get; }

        public NetworkStream Stream { get; }
        public bool isCreated { get; }

        public MemoryStream ms;
        public Image i;

        public TcpClient client;
        public NetworkStream stream;

        Thread mirroring;

        public Connector()
        {
            this.Port = 9876;
            this.Ip = GetIPAddress();

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(this.Ip), this.Port);

            TcpListener server = new TcpListener(ipep);
            server.Start();

            client = server.AcceptTcpClient();
            stream = client.GetStream();

            ms = new MemoryStream();
            mirroring = new Thread(new ThreadStart(GetImage));
        }

        public void MirroringStart()
        {
            mirroring.Start();
        }

        public void MirroringEnd()
        {
            if (mirroring.ThreadState == ThreadState.Running)
            {
                mirroring.Interrupt();
            }
        }

        public void GetImage()
        {
            try
            {
                while (true)
                {
                    byte[] bytes = new byte[4];
                    bytes = new byte[4];
                    stream.Write(bytes, 0, 4);

                    stream.Read(bytes, 0, 4);

                    int length = ByteToInt(bytes);
                    bytes = new byte[20000];

                    MemoryStream ms = new MemoryStream();
                    int read = 0;
                    while (length > 0)
                    {
                        //socket read
                        read = stream.Read(bytes, 0, 20000);
                        length -= read;
                        ms.Write(bytes, 0, read);
                    }

                    this.i = Image.FromStream(ms);
                    imageArrived();
                }
            }catch(ThreadInterruptedException tie)
            {
                mirroring.Abort();
            }
        }

        public int ByteToInt(byte[] bytes)
        {
            int num = 0;
            num |= bytes[0];
            num = num << 8;
            num |= bytes[1];
            num = num << 8;
            num |= bytes[2];
            num = num << 8;
            num |= bytes[3];

            return num;
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
