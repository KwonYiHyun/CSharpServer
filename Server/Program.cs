using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    static Listener listener = new Listener();

    public static GameRoom room = new GameRoom();

    static async Task Main(string[] args)
    {
        /*
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);
        */

        IPAddress ipA = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(ipA, 7777);

        listener.init(endPoint, 50);
        await listener.StartAsync();
    }
}