using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using Core;

public class Connector
{
    private Socket socket;

    public ServerSession session;
    IPEndPoint endPoint;

    public void init(IPEndPoint _endPoint)
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        endPoint = _endPoint;
    }

    public async Task ConnectAsync()
    {
        await socket.ConnectAsync(endPoint);
        session = SessionManager.Instance.Generate(socket);
        session.OnConnected();
        ThreadPool.QueueUserWorkItem(ReceiveAsync, socket);
    }

    private async void ReceiveAsync(object? sender)
    {
        Socket socket = sender as Socket;
        byte[] headerBuffer = new byte[2];

        System.Timers.Timer timer = new System.Timers.Timer(5000);
        timer.Elapsed += async (s, e) =>
        {
            // TODO HeartbeatPacket 전송
        };

        try
        {
            while (true)
            {
                int n1 = await socket.ReceiveAsync(headerBuffer, SocketFlags.None);
                if (n1 < 1)
                {
                    Console.WriteLine("client disconnect");
                    timer.Dispose();
                    socket.Dispose();
                    return;
                }
                else if (n1 == 1)
                {
                    await socket.ReceiveAsync(new ArraySegment<byte>(headerBuffer, 1, 1), SocketFlags.None);
                }

                short dataSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(headerBuffer));
                byte[] dataBuffer = new byte[dataSize];

                int totalRecv = 0;
                while (totalRecv < dataSize)
                {
                    int n2 = await socket.ReceiveAsync(new ArraySegment<byte>(dataBuffer, totalRecv, dataSize - totalRecv), SocketFlags.None);
                    totalRecv += n2;
                }

                PacketType type = (PacketType)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(dataBuffer));
                IPacket pkt = null;

                Func<Session, byte[], IPacket> func;

                if (PacketManager.packetTypes.TryGetValue(type, out func))
                {
                    pkt = func.Invoke(session, dataBuffer);
                }
                else
                {
                    Console.WriteLine("Connector.ReceiveAsync (ERROR) [Type에 맞는 패킷이 없습니다.]");
                }

                Action<Session, IPacket> action;
                if (PacketManager.action.TryGetValue(type, out action))
                {
                    action?.Invoke(session, pkt);
                }
                else
                {
                    Console.WriteLine("Connector.ReceiveAsync (ERROR) [Type에 맞는 Action이 없습니다.]");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void Dispose()
    {
        socket.Dispose();
    }
}