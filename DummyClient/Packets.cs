
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Core;

public class Packets
{

}


public class S_Login : IPacket
{
    public short Protocol { get; set; } = (short)PacketType.S_Login;
    public string msg;
    public List<int> arr = new List<int>();
    public bool isMine;
    public float pos;
    public int id;
    public char type;
    public PositionInfo positionInfo{ get; set; } = new PositionInfo();
    
    public int offset = 0;

    public byte[] Serialize()
    {
        byte[] packetType = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Protocol));
        
        byte[] msgPkt = Encoding.UTF8.GetBytes(msg);
        byte[] msgSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)msgPkt.Length));
	byte [] arrCountSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)arr.Count));
	byte[] isMinePkt = BitConverter.GetBytes(isMine);
        byte[] isMineSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)isMinePkt.Length));
	byte[] posPkt = BitConverter.GetBytes(pos);
        byte[] posSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)posPkt.Length));
	byte[] idPkt = BitConverter.GetBytes(id);
        byte[] idSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)idPkt.Length));
	byte[] typePkt = BitConverter.GetBytes(type);
        byte[] typeSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)typePkt.Length));
	byte[] PositionInfoPkt = positionInfo.Serialize();
        byte[] PositionInfoSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)PositionInfoPkt.Length));
	
	short dataSize = (short)(packetType.Length + msgPkt.Length + msgSize.Length + arrCountSize.Length + sizeof(int) * arr.Count + isMinePkt.Length + isMineSize.Length + posPkt.Length + posSize.Length + idPkt.Length + idSize.Length + typePkt.Length + typeSize.Length + PositionInfoPkt.Length + PositionInfoSize.Length);
        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(dataSize));

        byte[] buffer = new byte[2 + dataSize];

        offset = 0;

        Array.Copy(header, 0, buffer, offset, header.Length);
        offset += header.Length;

        Array.Copy(packetType, 0, buffer, offset, packetType.Length);
        offset += packetType.Length;
        
        Array.Copy(msgSize, 0, buffer, offset, msgSize.Length);
        offset += msgSize.Length;
        Array.Copy(msgPkt, 0, buffer, offset, msgPkt.Length);
        offset += msgPkt.Length;

	Array.Copy(arrCountSize, 0, buffer, offset, arrCountSize.Length);
        offset += arrCountSize.Length;

        foreach (var item in arr)
        {
            byte[] arrPkt = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)item));
            byte[] arrSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)arrPkt.Length));

            Array.Copy(arrSize, 0, buffer, offset, arrSize.Length);
            offset += arrSize.Length;

            Array.Copy(arrPkt, 0, buffer, offset, arrPkt.Length);
            offset += arrPkt.Length;
        }

	Array.Copy(isMineSize, 0, buffer, offset, isMineSize.Length);
        offset += isMineSize.Length;
        Array.Copy(isMinePkt, 0, buffer, offset, isMinePkt.Length);
        offset += isMinePkt.Length;

	Array.Copy(posSize, 0, buffer, offset, posSize.Length);
        offset += posSize.Length;
        Array.Copy(posPkt, 0, buffer, offset, posPkt.Length);
        offset += posPkt.Length;

	Array.Copy(idSize, 0, buffer, offset, idSize.Length);
        offset += idSize.Length;
        Array.Copy(idPkt, 0, buffer, offset, idPkt.Length);
        offset += idPkt.Length;

	Array.Copy(typeSize, 0, buffer, offset, typeSize.Length);
        offset += typeSize.Length;
        Array.Copy(typePkt, 0, buffer, offset, typePkt.Length);
        offset += typePkt.Length;

	Array.Copy(PositionInfoSize, 0, buffer, offset, PositionInfoSize.Length);
        offset += PositionInfoSize.Length;
        Array.Copy(PositionInfoPkt, 0, buffer, offset, PositionInfoPkt.Length);
        offset += PositionInfoPkt.Length;

	
        return buffer;
    }

    public void DeSerialize(byte[] buffer)
    {
        offset = 2;

        short msgSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        msg = Encoding.UTF8.GetString(buffer, offset, msgSize);
        offset += msgSize;
        
	short arrCountSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);

        for (int i = 0; i < arrCountSize; i++)
        {
            short itemSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
            offset += sizeof(short);

            short item = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
            offset += itemSize;

            arr.Add(item);
        }

        short isMineSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        isMine = BitConverter.ToBoolean(buffer, offset);
        offset += isMineSize;
        
	short posSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        pos = BitConverter.ToSingle(buffer, offset);
        offset += posSize;
        
	short idSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        id = BitConverter.ToInt32(buffer, offset);
        offset += idSize;
        
	short typeSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        type = BitConverter.ToChar(buffer, offset);
        offset += typeSize;
        
	short positionInfoSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        PositionInfo _positionInfo = new PositionInfo();
        _positionInfo.DeSerialize(new ArraySegment<byte>(buffer, offset, positionInfoSize).ToArray());
        positionInfo = _positionInfo;
        offset += positionInfoSize;
        
	
    }
}


public class C_Login : IPacket
{
    public short Protocol { get; set; } = (short)PacketType.C_Login;
    public string msg;
    public char ms;
    public PositionInfo positionInfo{ get; set; } = new PositionInfo();
    
    public int offset = 0;

    public byte[] Serialize()
    {
        byte[] packetType = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Protocol));
        
        byte[] msgPkt = Encoding.UTF8.GetBytes(msg);
        byte[] msgSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)msgPkt.Length));
	byte[] msPkt = BitConverter.GetBytes(ms);
        byte[] msSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)msPkt.Length));
	byte[] PositionInfoPkt = positionInfo.Serialize();
        byte[] PositionInfoSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)PositionInfoPkt.Length));
	
	short dataSize = (short)(packetType.Length + msgPkt.Length + msgSize.Length + msPkt.Length + msSize.Length + PositionInfoPkt.Length + PositionInfoSize.Length);
        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(dataSize));

        byte[] buffer = new byte[2 + dataSize];

        offset = 0;

        Array.Copy(header, 0, buffer, offset, header.Length);
        offset += header.Length;

        Array.Copy(packetType, 0, buffer, offset, packetType.Length);
        offset += packetType.Length;
        
        Array.Copy(msgSize, 0, buffer, offset, msgSize.Length);
        offset += msgSize.Length;
        Array.Copy(msgPkt, 0, buffer, offset, msgPkt.Length);
        offset += msgPkt.Length;

	Array.Copy(msSize, 0, buffer, offset, msSize.Length);
        offset += msSize.Length;
        Array.Copy(msPkt, 0, buffer, offset, msPkt.Length);
        offset += msPkt.Length;

	Array.Copy(PositionInfoSize, 0, buffer, offset, PositionInfoSize.Length);
        offset += PositionInfoSize.Length;
        Array.Copy(PositionInfoPkt, 0, buffer, offset, PositionInfoPkt.Length);
        offset += PositionInfoPkt.Length;

	
        return buffer;
    }

    public void DeSerialize(byte[] buffer)
    {
        offset = 2;

        short msgSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        msg = Encoding.UTF8.GetString(buffer, offset, msgSize);
        offset += msgSize;
        
	short msSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        ms = BitConverter.ToChar(buffer, offset);
        offset += msSize;
        
	short positionInfoSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        PositionInfo _positionInfo = new PositionInfo();
        _positionInfo.DeSerialize(new ArraySegment<byte>(buffer, offset, positionInfoSize).ToArray());
        positionInfo = _positionInfo;
        offset += positionInfoSize;
        
	
    }
}


public class S_PlayerMove : IPacket
{
    public short Protocol { get; set; } = (short)PacketType.S_PlayerMove;
    public int posX;
    public int posY;
    public int posZ;
    
    public int offset = 0;

    public byte[] Serialize()
    {
        byte[] packetType = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Protocol));
        
        byte[] posXPkt = BitConverter.GetBytes(posX);
        byte[] posXSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)posXPkt.Length));
	byte[] posYPkt = BitConverter.GetBytes(posY);
        byte[] posYSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)posYPkt.Length));
	byte[] posZPkt = BitConverter.GetBytes(posZ);
        byte[] posZSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)posZPkt.Length));
	
	short dataSize = (short)(packetType.Length + posXPkt.Length + posXSize.Length + posYPkt.Length + posYSize.Length + posZPkt.Length + posZSize.Length);
        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(dataSize));

        byte[] buffer = new byte[2 + dataSize];

        offset = 0;

        Array.Copy(header, 0, buffer, offset, header.Length);
        offset += header.Length;

        Array.Copy(packetType, 0, buffer, offset, packetType.Length);
        offset += packetType.Length;
        
        Array.Copy(posXSize, 0, buffer, offset, posXSize.Length);
        offset += posXSize.Length;
        Array.Copy(posXPkt, 0, buffer, offset, posXPkt.Length);
        offset += posXPkt.Length;

	Array.Copy(posYSize, 0, buffer, offset, posYSize.Length);
        offset += posYSize.Length;
        Array.Copy(posYPkt, 0, buffer, offset, posYPkt.Length);
        offset += posYPkt.Length;

	Array.Copy(posZSize, 0, buffer, offset, posZSize.Length);
        offset += posZSize.Length;
        Array.Copy(posZPkt, 0, buffer, offset, posZPkt.Length);
        offset += posZPkt.Length;

	
        return buffer;
    }

    public void DeSerialize(byte[] buffer)
    {
        offset = 2;

        short posXSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        posX = BitConverter.ToInt32(buffer, offset);
        offset += posXSize;
        
	short posYSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        posY = BitConverter.ToInt32(buffer, offset);
        offset += posYSize;
        
	short posZSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        posZ = BitConverter.ToInt32(buffer, offset);
        offset += posZSize;
        
	
    }
}


public class C_PlayerMove : IPacket
{
    public short Protocol { get; set; } = (short)PacketType.C_PlayerMove;
    public int posX;
    public int posY;
    public int posZ;
    
    public int offset = 0;

    public byte[] Serialize()
    {
        byte[] packetType = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Protocol));
        
        byte[] posXPkt = BitConverter.GetBytes(posX);
        byte[] posXSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)posXPkt.Length));
	byte[] posYPkt = BitConverter.GetBytes(posY);
        byte[] posYSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)posYPkt.Length));
	byte[] posZPkt = BitConverter.GetBytes(posZ);
        byte[] posZSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)posZPkt.Length));
	
	short dataSize = (short)(packetType.Length + posXPkt.Length + posXSize.Length + posYPkt.Length + posYSize.Length + posZPkt.Length + posZSize.Length);
        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(dataSize));

        byte[] buffer = new byte[2 + dataSize];

        offset = 0;

        Array.Copy(header, 0, buffer, offset, header.Length);
        offset += header.Length;

        Array.Copy(packetType, 0, buffer, offset, packetType.Length);
        offset += packetType.Length;
        
        Array.Copy(posXSize, 0, buffer, offset, posXSize.Length);
        offset += posXSize.Length;
        Array.Copy(posXPkt, 0, buffer, offset, posXPkt.Length);
        offset += posXPkt.Length;

	Array.Copy(posYSize, 0, buffer, offset, posYSize.Length);
        offset += posYSize.Length;
        Array.Copy(posYPkt, 0, buffer, offset, posYPkt.Length);
        offset += posYPkt.Length;

	Array.Copy(posZSize, 0, buffer, offset, posZSize.Length);
        offset += posZSize.Length;
        Array.Copy(posZPkt, 0, buffer, offset, posZPkt.Length);
        offset += posZPkt.Length;

	
        return buffer;
    }

    public void DeSerialize(byte[] buffer)
    {
        offset = 2;

        short posXSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        posX = BitConverter.ToInt32(buffer, offset);
        offset += posXSize;
        
	short posYSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        posY = BitConverter.ToInt32(buffer, offset);
        offset += posYSize;
        
	short posZSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        posZ = BitConverter.ToInt32(buffer, offset);
        offset += posZSize;
        
	
    }
}


public class PositionInfo : IPacket
{
    public short Protocol { get; set; } = (short)PacketType.PositionInfo;
    public int posX;
    public int posY;
    
    public int offset = 0;

    public byte[] Serialize()
    {
        byte[] packetType = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Protocol));
        
        byte[] posXPkt = BitConverter.GetBytes(posX);
        byte[] posXSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)posXPkt.Length));
	byte[] posYPkt = BitConverter.GetBytes(posY);
        byte[] posYSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)posYPkt.Length));
	
	short dataSize = (short)(packetType.Length + posXPkt.Length + posXSize.Length + posYPkt.Length + posYSize.Length);
        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(dataSize));

        byte[] buffer = new byte[2 + dataSize];

        offset = 0;

        Array.Copy(header, 0, buffer, offset, header.Length);
        offset += header.Length;

        Array.Copy(packetType, 0, buffer, offset, packetType.Length);
        offset += packetType.Length;
        
        Array.Copy(posXSize, 0, buffer, offset, posXSize.Length);
        offset += posXSize.Length;
        Array.Copy(posXPkt, 0, buffer, offset, posXPkt.Length);
        offset += posXPkt.Length;

	Array.Copy(posYSize, 0, buffer, offset, posYSize.Length);
        offset += posYSize.Length;
        Array.Copy(posYPkt, 0, buffer, offset, posYPkt.Length);
        offset += posYPkt.Length;

	
        return buffer;
    }

    public void DeSerialize(byte[] buffer)
    {
        offset = 2;

        short posXSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        posX = BitConverter.ToInt32(buffer, offset);
        offset += posXSize;
        
	short posYSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        posY = BitConverter.ToInt32(buffer, offset);
        offset += posYSize;
        
	
    }
}


public class ObjectInfo : IPacket
{
    public short Protocol { get; set; } = (short)PacketType.ObjectInfo;
    public int objectId;
    public PositionInfo positionInfo{ get; set; } = new PositionInfo();
    
    public int offset = 0;

    public byte[] Serialize()
    {
        byte[] packetType = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Protocol));
        
        byte[] objectIdPkt = BitConverter.GetBytes(objectId);
        byte[] objectIdSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)objectIdPkt.Length));
	byte[] PositionInfoPkt = positionInfo.Serialize();
        byte[] PositionInfoSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)PositionInfoPkt.Length));
	
	short dataSize = (short)(packetType.Length + objectIdPkt.Length + objectIdSize.Length + PositionInfoPkt.Length + PositionInfoSize.Length);
        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(dataSize));

        byte[] buffer = new byte[2 + dataSize];

        offset = 0;

        Array.Copy(header, 0, buffer, offset, header.Length);
        offset += header.Length;

        Array.Copy(packetType, 0, buffer, offset, packetType.Length);
        offset += packetType.Length;
        
        Array.Copy(objectIdSize, 0, buffer, offset, objectIdSize.Length);
        offset += objectIdSize.Length;
        Array.Copy(objectIdPkt, 0, buffer, offset, objectIdPkt.Length);
        offset += objectIdPkt.Length;

	Array.Copy(PositionInfoSize, 0, buffer, offset, PositionInfoSize.Length);
        offset += PositionInfoSize.Length;
        Array.Copy(PositionInfoPkt, 0, buffer, offset, PositionInfoPkt.Length);
        offset += PositionInfoPkt.Length;

	
        return buffer;
    }

    public void DeSerialize(byte[] buffer)
    {
        offset = 2;

        short objectIdSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        objectId = BitConverter.ToInt32(buffer, offset);
        offset += objectIdSize;
        
	short positionInfoSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        PositionInfo _positionInfo = new PositionInfo();
        _positionInfo.DeSerialize(new ArraySegment<byte>(buffer, offset, positionInfoSize).ToArray());
        positionInfo = _positionInfo;
        offset += positionInfoSize;
        
	
    }
}


