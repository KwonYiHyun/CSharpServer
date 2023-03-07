using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core;


public class GameLobby : GameRoom
{
    public override void Enter(ClientSession session)
    {
        base.Enter(session);

        Console.WriteLine("Lobby Enter");
    }

    public override void Leave(ClientSession session)
    {
        base.Leave(session);

        // TODO 로비를 나갔다는것은 게임 종료
    }
}

public class GameRoom
{
    public int RoomId { get; set; }

    List<ClientSession> _sessions = new List<ClientSession>();
    List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

    public void Broadcast(ClientSession session, IPacket packet)
    {
        ArraySegment<byte> buffer = packet.Serialize();

        _pendingList.Add(buffer);
    }

    public virtual void Enter(ClientSession session)
    {
        _sessions.Add(session);
        session.Room = this;
    }

    public virtual void Leave(ClientSession session)
    {
        _sessions.Remove(session);
    }

    public void Push(ClientSession session, Action action)
    {
        Program.roomManager.Push(action);
    }
    
    public async Task Flush()
    {
        foreach (ClientSession s in _sessions)
        {
            await s.SendAsync(_pendingList);
        }
    }
}