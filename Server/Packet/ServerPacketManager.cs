using System;
using System.Collections.Generic;
using System.Text;
using Core;

public static class PacketManager
{
    public static Dictionary<PacketType, Action<ClientSession, IPacket>> action = new Dictionary<PacketType, Action<ClientSession, IPacket>>();
    public static Dictionary<PacketType, Func<ClientSession, byte[], IPacket>> packetTypes = new Dictionary<PacketType, Func<ClientSession, byte[], IPacket>>();
    public static PacketHandler packetHandler = new PacketHandler();

    static PacketManager()
    {
        action.Add(PacketType.C_Login, packetHandler.C_LoginAction);
        packetTypes.Add(PacketType.C_Login, MakePacket<C_Login>);

		action.Add(PacketType.C_PlayerMove, packetHandler.C_PlayerMoveAction);
        packetTypes.Add(PacketType.C_PlayerMove, MakePacket<C_PlayerMove>);

		
    }

    static T MakePacket<T>(Session session, byte[] buffer) where T : IPacket, new()
    {
        T pkt = new T();
        pkt.DeSerialize(buffer);
        return pkt;
    }
}