
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
    public string msg;
    public List<int> arr = new List<int>();


    public byte[] Serialize()
    {
        byte[] packetType = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)PacketType.S_Login));

        byte[] msgPkt = Encoding.UTF8.GetBytes(msg);
        byte[] msgSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)msgPkt.Length));
        byte[] arrCountSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)arr.Count));

        short dataSize = (short)(packetType.Length + msgPkt.Length + msgSize.Length + arrCountSize.Length + sizeof(int) * arr.Count);
        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(dataSize));

        byte[] buffer = new byte[2 + dataSize];

        int offset = 0;

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


        return buffer;
    }

    public void DeSerialize(byte[] buffer)
    {
        int offset = 2;

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
    }
}


public class C_Login : IPacket
{
    public string msg;
    public char ms;


    public byte[] Serialize()
    {
        byte[] packetType = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)PacketType.C_Login));

        byte[] msgPkt = Encoding.UTF8.GetBytes(msg);
        byte[] msgSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)msgPkt.Length));
        byte[] msPkt = BitConverter.GetBytes(ms);
        byte[] msSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)msPkt.Length));

        short dataSize = (short)(packetType.Length + msgPkt.Length + msgSize.Length + msPkt.Length + msSize.Length);
        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(dataSize));

        byte[] buffer = new byte[2 + dataSize];

        int offset = 0;

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


        return buffer;
    }

    public void DeSerialize(byte[] buffer)
    {
        int offset = 2;

        short msgSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);

        msg = Encoding.UTF8.GetString(buffer, offset, msgSize);
        offset += msgSize;

        short msSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);

        ms = BitConverter.ToChar(buffer, msSize);
        offset += msSize;


    }
}


