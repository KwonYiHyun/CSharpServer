using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Core
{
    public abstract class Session
    {
        public int sessionId;
        protected Socket socket;

        public abstract void OnConnected();
        public abstract void OnDisconnected();

        public abstract Task<int> ReceiveAsync(byte[] headerBuffer, SocketFlags flags);
        public abstract Task<int> ReceiveAsync(ArraySegment<byte> buffer, SocketFlags flags);
        public abstract Task<int> SendAsync(byte[] headerBuffer, SocketFlags flags);
        public abstract Task<int> SendAsync(ArraySegment<byte> buffer, SocketFlags flags);

        public void init(Socket _socket)
        {
            socket = _socket;
        }
    }
}
