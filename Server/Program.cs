using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static Listener listener = new Listener();
    static List<System.Timers.Timer> _timers = new List<System.Timers.Timer>();

    public static GameRoomManager roomManager = new GameRoomManager();

    static void TickRoom(GameRoom room, int tick = 100)
    {
        var timer = new System.Timers.Timer();
        timer.Interval = tick;
        timer.Elapsed += ((s, e) => { room.Update(); });
        timer.AutoReset = true;
        timer.Enabled = true;

        _timers.Add(timer);
    }

    static async Task Main(string[] args)
    {
        /*
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);
        */

        // 이런식으로 room 하나만 터는게 아니라 생성할때마다 해야할듯
        GameRoom room = new GameRoom();
        TickRoom(room, 100);
        
        IPAddress ipA = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(ipA, 7777);

        Console.WriteLine($"OS : {Environment.OSVersion} \nEndPoint : {endPoint}");

        //Task jobTask = new Task(() =>
        //  {
        //      while (true)
        //      {
        //          Thread.Sleep(100);
        //      }
        //  }, TaskCreationOptions.LongRunning);
        //jobTask.Start();

        listener.Init(endPoint, 50);
        await listener.StartAsync();
    }
}