using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Connector connector = new Connector();

        IPAddress ipA = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(ipA, 7777);

        connector.init(endPoint);

        await connector.ConnectAsync();

        List<int> arr = new List<int>();
        arr.Add(10);
        arr.Add(20);
        arr.Add(30);

        while (true)
        {
            S_Login pkt = new S_Login();
            pkt.msg = "aaaa";
            pkt.arr = arr;

            await SessionManager.Instance.sessions[1].SendAsync(pkt.Serialize());

            Thread.Sleep(1000);
        }
    }
}