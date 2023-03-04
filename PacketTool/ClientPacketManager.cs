using System;
using System.Collections.Generic;
using System.Text;
using Core;

public class PacketManager
{
    public static Dictionary<PacketType, Action<ServerSession, IPacket>> action = new Dictionary<PacketType, Action<ServerSession, IPacket>>();
    public static Dictionary<PacketType, Func<ServerSession, byte[], IPacket>> packetTypes = new Dictionary<PacketType, Func<ServerSession, byte[], IPacket>>();
    public static PacketHandler packetHandler = new PacketHandler();

    static PacketManager _instance = new PacketManager();
    public static PacketManager Instance { get { return _instance; } }

    static PacketManager()
    {
        action.Add(PacketType.S_Login, packetHandler.S_LoginAction);
        packetTypes.Add(PacketType.S_Login, MakePacket<S_Login>);

		
    }

    static T MakePacket<T>(Session session, byte[] buffer) where T : IPacket, new()
    {
        T pkt = new T();
        pkt.DeSerialize(buffer);
        return pkt;
    }

    public void HandlePacket(ServerSession session, IPacket type)
    {
        Action<ServerSession, IPacket> _action = null;
        if (action.TryGetValue((PacketType)type.Protocol, out _action))
        {
            _action.Invoke(session, type);
        }
    }
}