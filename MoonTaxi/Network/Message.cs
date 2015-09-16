using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonTaxi.Network
{
    internal abstract class Message
    {
        public Message(byte id)
        {
            ID = id;
        }

        public Message(byte id,byte[] payload)
        {
            ID = id;
            PayLoad = payload;
        }
        public byte ID { get; private set; }
        public byte[] PayLoad { get; protected set; }
    }
}
