using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;


class Program
{
    static void Main(string[] args)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPAddress ipA = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(ipA, 7777);

        socket.Connect(endPoint);

        List<int> arr = new List<int>();
        arr.Add(10);
        arr.Add(20);
        arr.Add(30);

        while (true)
        {
            S_Login pkt = new S_Login();
            pkt.msg = "aaaa";
            pkt.arr = arr;

            socket.Send(pkt.Serialize());

            Thread.Sleep(1000);
        }
    }
}