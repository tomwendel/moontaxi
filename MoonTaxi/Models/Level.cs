using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MoonTaxi.Models
{
    [Serializable]
    public abstract class Level
    {
        // TODO: Seiten sollten wrap-bar sein (x und y - levelsettings)
        public Vector2 Size { get; private set; }

        public int ParallelGuests { get; set; }

        public List<Block> Blocks { get; private set; }

        [XmlIgnore]
        public List<Guest> Guests { get; private set; }

        public abstract Vector2 GetTaxiSpawn();

        public Level(Vector2 size, int parallelGuestCounts)
        {
            Blocks = new List<Block>();
            Guests = new List<Guest>();
            Size = size;
            ParallelGuests = parallelGuestCounts;
        }
    }
}
