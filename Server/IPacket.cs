using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public interface IPacket
    {
        byte[] Serialize();

        void DeSerialize(byte[] buffer);
    }
}
