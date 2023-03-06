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

        for (int i = 0; i < 200; i++)
        {
            C_Login pkt = new C_Login();
            pkt.msg = "aaaa" + i;

            await SessionManager.Instance.sessions[1].SendAsync(pkt.Serialize());

            Thread.Sleep(250);
        }

        connector.Dispose();
    }
}