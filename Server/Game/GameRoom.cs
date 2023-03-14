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

public class GameRoom : JobSerializer
{
    public int RoomId { get; set; }

    Dictionary<int, Player> _players = new Dictionary<int, Player>();
    Dictionary<int, Projectile> _projectiles = new Dictionary<int, Projectile>();

    List<ClientSession> _sessions = new List<ClientSession>();
    
    public void Init()
    {

    }

    public void Update()
    {
        foreach (Projectile projectile in _projectiles.Values)
        {
            projectile.Update();
        }

        Flush();
    }

    public void Broadcast(IPacket packet)
    {
        foreach (Player p in _players.Values)
        {
            p.Session.SendAsync(packet.Serialize());
        }
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
}