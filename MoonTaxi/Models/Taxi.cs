using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonTaxi.Models
{
    //Taxi Model
    internal class Taxi
    {
        public Vector2 DeltaVelocity { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        public Block OnGround { get; set; }

        public int GuestLimit { get; set; }

        public List<Guest> Guests { get; set; }

        public Vector2 Size { get; private set; }

        public Taxi()
        {
            Guests = new List<Guest>();
            Reset();
            Size = new Vector2(50, 20);
            GuestLimit = 3;
        }

        public void Reset()
        {
            Position = new Vector2(100, 100);
            Velocity = new Vector2();
            Guests.Clear();
        }

        internal void Update(GameTime gameTime)
        {
            var gamepad = GamePad.GetState(PlayerIndex.One);
            var keyboard = Keyboard.GetState();


            DeltaVelocity = gamepad.ThumbSticks.Left * new Vector2(1, -1);
            DeltaVelocity += new Vector2((keyboard.IsKeyDown(Keys.Left) ? -1 : 0) + (keyboard.IsKeyDown(Keys.Right) ? 1 : 0),
                                        (keyboard.IsKeyDown(Keys.Up) ? -1 : 0) + (keyboard.IsKeyDown(Keys.Down) ? 1 : 0));
            DeltaVelocity.Normalize();
            DeltaVelocity *= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
            Velocity += DeltaVelocity;

            Velocity += new Vector2(0, 1.6f * (float)gameTime.ElapsedGameTime.TotalSeconds);
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
