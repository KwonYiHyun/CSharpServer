using Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

public class GameRoomManager : IJobQueue
{
    public static GameRoomManager Instance { get; } = new GameRoomManager();

    public GameLobby gameLobby = new GameLobby();

    int _roomId = 0;
    Dictionary<int, GameRoom> _rooms = new Dictionary<int, GameRoom>();
    JobQueue _jobQueue = new JobQueue();
    object _lock = new object();

    public Dictionary<int, GameRoom> Rooms
    {
        get
        {
            return _rooms;
        }
    }

    public GameRoomManager()
    {

    }

    public void Push(Action job)
    {
        _jobQueue.Push(job);
    }

    public GameRoom Generate()
    {
        lock (_lock)
        {
            int roomId = ++_roomId;

            GameRoom room = new GameRoom();
            room.RoomId = roomId;
            _rooms.Add(roomId, room);

            return room;
        }
    }

    //public async Task Flush()
    //{
    //    foreach (KeyValuePair<int, GameRoom> room in _rooms)
    //    {
    //        await room.Value.Flush();
    //    }
    //    await gameLobby.Flush();
    //}

    public GameRoom Find(int id)
    {
        lock (_lock)
        {
            GameRoom room = null;
            _rooms.TryGetValue(id, out room);
            return room;
        }
    }

    public void Remove(ClientSession session)
    {
        lock (_lock)
        {
            if (session == null) return;

            _rooms.Remove(session.Room.RoomId);
            session.Room = gameLobby;
        }
    }

    public void Remove(GameRoom room)
    {
        lock (_lock)
        {
            if (room == null) return;

            // _rooms.Remove(room.RoomId);
        }
    }
}