using System;
using System.Collections.Generic;
using System.Text;

public interface IPacket
{
    byte[] Serialize();

    void DeSerialize(byte[] buffer);
}