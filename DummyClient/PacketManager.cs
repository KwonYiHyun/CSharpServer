using System;
using System.Collections.Generic;
using System.Text;
using Core;

public static class PacketManager
{
    public static Dictionary<PacketType, Action<Session, IPacket>> action = new Dictionary<PacketType, Action<Session, IPacket>>();
    public static Dictionary<PacketType, Func<Session, byte[], IPacket>> packetTypes = new Dictionary<PacketType, Func<Session, byte[], IPacket>>();
    public static PacketHandler packetHandler = new PacketHandler();

    static PacketManager()
    {
        action.Add(PacketType.S_Login, packetHandler.S_LoginAction);
        action.Add(PacketType.C_Login, packetHandler.C_LoginAction);
        action.Add(PacketType.S_UserEnter, packetHandler.S_UserEnterAction);
        action.Add(PacketType.C_UserEnter, packetHandler.C_UserEnterAction);

        packetTypes.Add(PacketType.S_Login, MakePacket<S_Login>);
    }

    static T MakePacket<T>(Session session, byte[] buffer) where T : IPacket, new()
    {
        T pkt = new T();
        pkt.DeSerialize(buffer);
        return pkt;
    }
}