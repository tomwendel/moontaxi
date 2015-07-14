using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonTaxi.Models
{
    [Serializable]
    public class Block
    {
        public Rectangle Size { get; set; }

        public bool SpawnPlatform { get; set; }

        public float Friction { get; set; }

        public Block()
        {
            Friction = 1.4f;
        }
    }
}
