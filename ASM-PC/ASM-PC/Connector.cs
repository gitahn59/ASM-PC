using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Drawing;
using System.Threading;

namespace ASM_PC
{
    class Connector
    {
        /// <summary>
        /// Image arrived event
        /// </summary>
        public event Action imageArrived;
        public event Action disconnected;

        public int Port { get; }
        public string Ip { get; }

        private bool isConnected;
        public bool IsConnected { get { return isConnected; } }

        private Image androidScreenImage;
        public Image AndroidScreenImage { get { return androidScreenImage; } }

        private TcpListener server;
        private TcpClient client;
        private NetworkStream stream;
        private Thread mirroring;

        public Connector()
        {
            Port = 9876;
            Ip = GetIPAddress();
            if (Ip == null)
            {
                isConnected = false;
                return;
            }

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(this.Ip), this.Port);

            server = new TcpListener(ipep);
            server.Start();

            client = server.AcceptTcpClient();
            stream = client.GetStream();
            mirroring = new Thread(new ThreadStart(GetImage));
            isConnected = true;
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
                mirroring.Abort();
                stream.Close();
                client.Close();
                server.Stop();
                this.isConnected = false;
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
                    bytes = new byte[50000];

                    MemoryStream ms = new MemoryStream();
                    int read = 0;
                    while (length > 0)
                    {
                        //socket read
                        read = stream.Read(bytes, 0, 50000);
                        length -= read;
                        ms.Write(bytes, 0, read);
                    }

                    androidScreenImage = Image.FromStream(ms);
                    imageArrived();
                }
            }catch(ArgumentException ae)
            {

            }
            catch (IOException ioe)
            {
               
            }
            finally
            {
                stream.Close();
                client.Close();
                server.Stop();
                isConnected = false;
                disconnected(); // disconnect event appear 
            }
        }

        /// <summary>
        /// 4 byte array to int 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get Local Address
        /// </summary>
        /// <returns></returns>
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

            return null;
        }
    }
}
