using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Core;

public class ServerSession : Session
{
    public override void OnConnected()
    {
        Console.WriteLine("OnConnected");
    }

    public override void OnDisconnected()
    {
        Console.WriteLine("OnDisconnected");
    }

    public override Task<int> ReceiveAsync(byte[] headerBuffer, SocketFlags flags)
    {
        return socket.ReceiveAsync(headerBuffer, flags);
    }

    public override Task<int> ReceiveAsync(ArraySegment<byte> buffer, SocketFlags flags)
    {
        return socket.ReceiveAsync(buffer, flags);
    }

    public override Task<int> SendAsync(byte[] headerBuffer, SocketFlags flags = SocketFlags.None)
    {
        return socket.SendAsync(headerBuffer, flags);
    }

    public override Task<int> SendAsync(ArraySegment<byte> buffer, SocketFlags flags)
    {
        return socket.SendAsync(buffer, flags);
    }

}