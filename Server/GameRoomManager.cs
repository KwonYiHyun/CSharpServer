using Core;
using System;
using System.Collections.Generic;
using System.Text;

public class GameRoomManager : IJobQueue
{
    GameRoom lobbyRoom;

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

    public GameRoom LobbyRoom
    {
        get
        {
            return lobbyRoom;
        }
    }

    public GameRoomManager()
    {
        lobbyRoom = new GameRoom();
    }

    public void Push(Action job)
    {
        _jobQueue.Push(job);
    }

    public GameRoom Generate(ClientSession owner, string roomName, int roomLimit)
    {
        lock (_lock)
        {
            int roomId = ++_roomId;

            GameRoom room = new GameRoom();

            _rooms.Add(roomId, room);

            return room;
        }
    }

    public void Flush()
    {
        // _jobQueue.fl
    }

    //public GameRoom Find(int id)
    //{
    //    lock (_lock)
    //    {
    //        GameRoom room = null;
    //        _rooms.TryGetValue(id, out room);
    //        return room;
    //    }
    //}

    public void Remove(ClientSession session)
    {
        lock (_lock)
        {
            if (session == null) return;

            // _rooms.Remove(session.Room.RoomId);
            session.Room = lobbyRoom;
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