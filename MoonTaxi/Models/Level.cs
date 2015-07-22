using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MoonTaxi.Models
{
    [Serializable]
    public class Level
    {
        // TODO: Seiten sollten wrap-bar sein (x und y - levelsettings)
        public Vector2 Size { get; set; }

        public int ParallelGuests { get; set; }

        public List<Block> Blocks { get; set; }

        [XmlIgnore]
        public List<Guest> Guests { get; set; }

        public Vector2 TaxiSpawn { get; set; }

        public Level()
        {
            Blocks = new List<Block>();
            Guests = new List<Guest>();
            Size = new Vector2(1280, 720);
            ParallelGuests = 1;
        }
    }
}
