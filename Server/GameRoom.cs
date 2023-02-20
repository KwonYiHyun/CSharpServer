using System;
using System.Collections.Generic;
using System.Text;
using Core;


public class GameRoom : IJobQueue
{
    List<ClientSession> _sessions = new List<ClientSession>();
    JobQueue _jobQueue = new JobQueue();

    public void Push(Action job)
    {
        _jobQueue.Push(job);
    }

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
}