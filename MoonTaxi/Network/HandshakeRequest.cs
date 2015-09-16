using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonTaxi.Network
{
    internal class HandshakeRequest : Message
    {
        public HandshakeRequest(byte version)
            :base(1)
        {
            Version = version;
            byte[] buffer = Encoding.ASCII.GetBytes("Hello. I'm an awesome MoonTaxi-Server ;)");
            PayLoad = new byte[buffer.Length + 1];
            PayLoad[0] = version;
            Array.Copy(buffer, 0, PayLoad, 1, buffer.Length);
        }
        public HandshakeRequest(byte[] payload)
            :base(1,payload)
        {
            Version = payload[0];
        }
        public byte Version { get; private set; }
    }
}
