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
            S_Login pkt = new S_Login();
            pkt.msg = "aaaa";
            pkt.arr = arr;
            pkt.isMine = false;
            pkt.pos = 3.14f;
            pkt.id = 22222;
            pkt.type = 't';

            await SessionManager.Instance.sessions[1].SendAsync(pkt.Serialize());

            Thread.Sleep(250);
        }

        connector.Dispose();
    }
}