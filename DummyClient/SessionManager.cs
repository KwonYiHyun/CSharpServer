using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Core;

class SessionManager
{
    static SessionManager sessionManager = new SessionManager();
    public static SessionManager Instance { get { return sessionManager; } }

    public Dictionary<int, ServerSession> sessions = new Dictionary<int, ServerSession>();
        
    int sessionId = 0;
    object _lock = new object();

    public ServerSession Generate(Socket serverSocket)
    {
        lock (_lock)
        {
            int _sessionId = ++sessionId;

            // 최적화 방안1 - 이쪽에서 Session 풀링하기

            ServerSession session = new ServerSession();
            session.sessionId = _sessionId;
            sessions.Add(_sessionId, session);

            session.init(serverSocket);

            return session;
        }
    }
}