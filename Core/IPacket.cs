using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public interface IPacket
    {
        short Protocol { get; set; }

        byte[] Serialize();

        void DeSerialize(byte[] buffer);
    }
}