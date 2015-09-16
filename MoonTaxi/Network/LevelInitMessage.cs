using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MoonTaxi.Network
{
    internal class LevelInitMessage : Message
    {
        public int LevelSeed { get; private set; }

        public string[] Users { get; private set; }

        public Color[] Colors { get; private set; }

        public LevelInitMessage(int levelSeed, string[] users, Color[] colors)
            : base(3)
        {
            if (users.Length != colors.Length)
                throw new ArgumentException("user.Length != colors.Length");
            LevelSeed = levelSeed;
            Users = users;
            Colors = colors;

            using (MemoryStream payload = new MemoryStream())
            {
                BinaryWriter writer = new BinaryWriter(payload,System.Text.Encoding.Default);
                writer.Write(levelSeed);
                writer.Write(users.Length);
                for (int i = 0; i < users.Length; i++)
                {
                    writer.Write(Users[i]);
                    writer.Write(Colors[i].PackedValue);
                }

            }
        }

        public LevelInitMessage(byte id, byte[] payload) : base(id, payload)
        {
            using (MemoryStream payloadStream = new MemoryStream(payload))
            {
                BinaryReader reader = new BinaryReader(payloadStream);

                LevelSeed = reader.ReadInt32();
                int userCount = reader.ReadInt32();
                Users = new string[userCount];
                Colors = new Color[userCount];
                for (int i=0;i< userCount;i++)
                {
                    Users[i] = reader.ReadString();
                    Colors[i].PackedValue = reader.ReadUInt32();
                }
            }
        }
    }
}
