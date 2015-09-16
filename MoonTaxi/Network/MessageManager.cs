using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonTaxi.Network
{
    static class MessageManager
    {
        private static Dictionary<byte, Type> types;
        static MessageManager()
        {
            types = new Dictionary<byte, Type>();
            types.Add(1, typeof(HandshakeRequest));
            types.Add(2, typeof(HandshakeResponse));
            types.Add(3, typeof(LevelInitMessage));
        }
        public static Message Deserialize(byte[] data,int count)
        {
            if (count == 1)
                return Deserialize(data[0], null);

            byte[] payload = new byte[count - 1];
            Array.Copy(data, 1, payload, 0, count - 1);
            return Deserialize(data[0], payload);
        }
        public static Message Deserialize(byte id, byte[] payload)
        {
            if (types.ContainsKey(id))
            {
                return (Message)types[id].GetConstructor(new Type[] { typeof(byte[]) }).Invoke(new object[] {payload });
            }
            return null;
        }
        /*private static MessageManager manager;
        public static MessageManager Instance {
            get
            {
            }
        }*/

    }
}
