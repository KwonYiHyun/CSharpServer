using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface IPacket
    {
        byte[] Serialize();

        void DeSerialize(byte[] buffer);
    }
}