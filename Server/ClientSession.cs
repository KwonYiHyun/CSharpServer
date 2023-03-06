using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Core;

public class ClientSession : Session
{
    public GameRoom Room { get; set; }

    public void Dispose()
    {
        socket.Dispose();
    }

    public override void OnConnected()
    {
        Console.WriteLine("OnConnected");
        // Program.room.Push(() => Program.room.Enter(this));
    }

    public override void OnDisconnected()
    {
        Console.WriteLine("OnDisconnected");
    }

    public override Task<int> ReceiveAsync(byte[] headerBuffer, SocketFlags flags = SocketFlags.None)
    {
        return socket.ReceiveAsync(headerBuffer, flags);
    }

    public override Task<int> ReceiveAsync(ArraySegment<byte> buffer, SocketFlags flags = SocketFlags.None)
    {
        return socket.ReceiveAsync(buffer, flags);
    }

    public override Task<int> SendAsync(byte[] headerBuffer, SocketFlags flags = SocketFlags.None)
    {
        return socket.SendAsync(headerBuffer, flags);
    }

    public override Task<int> SendAsync(ArraySegment<byte> buffer, SocketFlags flags = SocketFlags.None)
    {
        return socket.SendAsync(buffer, flags);
    }

}