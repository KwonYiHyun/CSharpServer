using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static Listener listener = new Listener();

    public static GameRoomManager roomManager = new GameRoomManager();

    static void FlushRoom()
    {
        roomManager.Push(() => roomManager.Flush());
        JobTimer.Instance.Push(FlushRoom, 250);
    }

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

        Console.WriteLine($"OS : {Environment.OSVersion} \nEndPoint : {endPoint}");

        JobTimer.Instance.Push(FlushRoom);

        Task jobTask = new Task(() =>
          {
              while (true)
              {
                  JobTimer.Instance.Flush();
              }
          }, TaskCreationOptions.LongRunning);
        jobTask.Start();

        listener.Init(endPoint, 50);
        await listener.StartAsync();
    }
}