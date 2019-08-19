﻿using System;
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

        public Socket socket { get; }

        public NetworkStream Stream { get; }
        public bool isCreated { get; }

        public MemoryStream ms;
        public Image i;

        public TcpClient client;
        public NetworkStream stream;


        public Connector()
        {
            string s = "marst branch test";

            this.Port = 9876;
            this.Ip = GetIPAddress();

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(this.Ip), this.Port);

            TcpListener server = new TcpListener(ipep);
            server.Start();

            client = server.AcceptTcpClient();
            stream = client.GetStream();

            ms = new MemoryStream();
        }

        public void GetImage()
        {
            byte[] bytes = new byte[4];
            stream.Read(bytes, 0, 4);
            
            int length = ByteToInt(bytes);
            bytes = new byte[20000];

            MemoryStream ms = new MemoryStream();
            int read = 0;
            while (length>0)
            {
                //socket read
                read = stream.Read(bytes, 0, 20000);
                length -= read;
                ms.Write(bytes, 0, read);
            }
            bytes = new byte[4];
            stream.Write(bytes, 0, 4);

            this.i = Image.FromStream(ms);

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
