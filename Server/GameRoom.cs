using System;
using System.Collections.Generic;
using System.Text;
using Core;


public class GameRoom
{
    List<ClientSession> _sessions = new List<ClientSession>();

    public async void Broadcast(ClientSession session)
    {
        S_Login pkt = new S_Login();

        foreach (var s in _sessions)
        {
            await s.SendAsync(pkt.Serialize());
        }
    }

    public void Enter(ClientSession session)
    {
        _sessions.Add(session);
        session.Room = this;
    }

    public void Leave(ClientSession session)
    {
        _sessions.Remove(session);
    }

    public void Push(ClientSession session, Action action)
    {
        Program.roomManager.Push(action);
    }
}