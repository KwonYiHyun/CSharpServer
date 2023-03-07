using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Core;

class SessionManager
{
    static SessionManager sessionManager = new SessionManager();
    public static SessionManager Instance { get { return sessionManager; } }

    int sessionId = 0;
    Dictionary<int, ClientSession> sessions = new Dictionary<int, ClientSession>();
    object _lock = new object();

    public ClientSession Generate(Socket clientSocket)
    {
        lock (_lock)
        {
            int _sessionId = ++sessionId;

            // 최적화 방안1 - 이쪽에서 Session 풀링하기

            ClientSession session = new ClientSession();
            session.sessionId = _sessionId;
            sessions.Add(_sessionId, session);

            session.init(clientSocket);

            return session;
        }
    }
}