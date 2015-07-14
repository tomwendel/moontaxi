using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonTaxi.Models
{
    public class Guest
    {
        public const int GUEST_WIDTH = 57;

        public const int GUEST_HEIGHT = 57;

        public Vector2 Position { get; set; }

        public Block CurrentBlock { get; set; }

        public Block Departure { get; set; }

        public Block Destination { get; set; }
    }
}
