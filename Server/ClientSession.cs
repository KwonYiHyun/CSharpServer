using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class ClientSession : Session
    {
        public override void OnConnected()
        {
            Console.WriteLine("OnConnected");
        }

        public override void OnDisconnected()
        {
            Console.WriteLine("OnDisconnected");
        }

        public override Task<int> ReceiveAsync(byte[] _headerBuffer, SocketFlags flags = SocketFlags.None)
        {
            return socket.ReceiveAsync(_headerBuffer, flags);
        }

        public override Task<int> ReceiveAsync(ArraySegment<byte> buffer, SocketFlags flags = SocketFlags.None)
        {
            return socket.ReceiveAsync(buffer, flags);
        }

        public override Task<int> SendAsync(ArraySegment<byte> buffer, SocketFlags flags = SocketFlags.None)
        {
            return socket.SendAsync(buffer, flags);
        }
    }
}
