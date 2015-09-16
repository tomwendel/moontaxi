using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonTaxi.Network
{
    class HandshakeResponse : Message
    {
        public HandshakeResponse(string username)
            : base(2)
        {
            Username = username;
            PayLoad = System.Text.Encoding.Default.GetBytes(username);
        }

        public HandshakeResponse(byte[] payload)
            : base(2, payload)
        {
            Username = System.Text.Encoding.Default.GetString(payload);
        }
        public string Username { get; private set; }
    }
}
