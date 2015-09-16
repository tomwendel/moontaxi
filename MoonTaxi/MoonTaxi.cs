using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MoonTaxi.Components;
using MoonTaxi.Generator;
using MoonTaxi.Interaction;
using MoonTaxi.Models;
using MoonTaxi.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MoonTaxi
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MoonTaxi : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random rand = new Random();


        List<Taxi> taxis = new List<Taxi>();
        Level level;
        int points = 0;

        List<Color> playerColor = new List<Color>();
        List<Texture2D> taxiTextures = new List<Texture2D>();
        Texture2D background;
        Texture2D stone;
        Texture2D eva;
        Texture2D pix;
        Texture2D taxiSign;
        Texture2D flag;
        SpriteFont hudFont;

        SoundComponent sound;
        private bool isServer = true;
        private Server server;
        private Client client;
        public MoonTaxi(bool isServer,string username)
            : base()
        {
            this.isServer = isServer;
            if (isServer)
            {
                server = new Server();
                server.Start();
            }
            else
            {
                client = new Client(username);
                client.Connect(IPAddress.Loopback, 1234);
                client.DataReceived += Client_DataReceived;
            }
            graphics = new GraphicsDeviceManager(this);

            level = new RandomLevel(new Vector2(1280, 720), 2, Environment.TickCount);

            graphics.PreferredBackBufferWidth = (int)level.Size.X;
            graphics.PreferredBackBufferHeight = (int)level.Size.Y;

            Content.RootDirectory = "Content";

            Components.Add(sound = new SoundComponent(this));
        }

        private void Client_DataReceived(Client sender, byte[] buffer, int count)
        {
            Console.WriteLine("Respond: " + System.Text.Encoding.Default.GetString(buffer, 0, count));
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Window.Position = new Point(10, 50);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>("Textures/background");
            stone = Content.Load<Texture2D>("Textures/stone");
            eva = Content.Load<Texture2D>("Textures/eva");
            pix = Content.Load<Texture2D>("Textures/pix");
            taxiSign = Content.Load<Texture2D>("Textures/taxisign");
            flag = Content.Load<Texture2D>("Textures/flag");
            hudFont = Content.Load<SpriteFont>("HudFont");

            taxiTextures.Add(Content.Load<Texture2D>("Textures/taxi_blue"));
            taxiTextures.Add(Content.Load<Texture2D>("Textures/taxi_red"));
            taxiTextures.Add(Content.Load<Texture2D>("Textures/taxi_yellow"));
            taxiTextures.Add(Content.Load<Texture2D>("Textures/taxi_green"));

            playerColor.Add(Color.Blue);
            playerColor.Add(Color.Red);
            playerColor.Add(Color.Yellow);
            playerColor.Add(Color.Green);

            Random rand = new Random();
            Texture2D buffer;
            Color colorBuffer;
            for (int i = 0; i < 100; i++)
            {
                int from = rand.Next(0, 4);
                int to = rand.Next(0, 4);

                buffer = taxiTextures[from];
                taxiTextures[from] = taxiTextures[to];
                taxiTextures[to] = buffer;

                colorBuffer = playerColor[from];
                playerColor[from] = playerColor[to];
                playerColor[to] = colorBuffer;
            }

            Taxi taxi1 = new Taxi(new LocalInput(LocalInputType.GamePad1));
            taxi1.Position = level.GetTaxiSpawn();
            taxis.Add(taxi1);
            sound.AddTaxi(taxi1);

            Taxi taxi2 = new Taxi(new LocalInput(LocalInputType.Keyboard1));
            taxi2.Position = level.GetTaxiSpawn();
            taxis.Add(taxi2);
            sound.AddTaxi(taxi2);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var taxi in taxis)
            {
                taxi.Update(gameTime);

                float volume = Math.Max(0, Math.Min(1, taxi.DeltaVelocity.Length()));
                sound.SetEngineVolume(taxi, volume);

                #region Kollision mit der Wand

                Vector2 sizeHalf = taxi.Size / 2;
                if (taxi.Position.X - sizeHalf.X < 0)
                {
                    taxi.Position = new Vector2(sizeHalf.X, taxi.Position.Y);
                    taxi.Velocity *= new Vector2(0, 1);
                }
                if (taxi.Position.Y - sizeHalf.Y < 0)
                {
                    taxi.Position = new Vector2(taxi.Position.X, sizeHalf.Y);
                    taxi.Velocity *= new Vector2(1, 0);
                }
                if (taxi.Position.X + sizeHalf.X > level.Size.X)
                {
                    taxi.Position = new Vector2(level.Size.X - sizeHalf.X, taxi.Position.Y);
                    taxi.Velocity *= new Vector2(0, 1);
                }
                if (taxi.Position.Y + sizeHalf.Y > level.Size.Y)
                {
                    taxi.Position = new Vector2(taxi.Position.X, level.Size.Y - sizeHalf.Y);
                    taxi.Velocity *= new Vector2(1, 0);
                }

                #endregion

                #region Kollision mit Blöcken

                Vector2 deltaPosition = taxi.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                float taxiLeft = taxi.Position.X - sizeHalf.X;
                float taxiTop = taxi.Position.Y - sizeHalf.Y;
                float taxiRight = taxi.Position.X + sizeHalf.X;
                float taxiBottom = taxi.Position.Y + sizeHalf.Y;

                taxi.OnGround = null;
                float scratchVolume = 0f;
                float scratchPitch = 0f;

                foreach (var block in level.Blocks)
                {
                    if (taxiLeft < block.Size.Right && taxiRight > block.Size.Left &&
                        taxiTop < block.Size.Bottom && taxiBottom > block.Size.Top)
                    {
                        float factorX = 0f;
                        if (deltaPosition.X > 0)
                        {
                            float deltaLeft = taxiRight - block.Size.Left;
                            factorX = deltaLeft / deltaPosition.X;
                        }
                        else
                        {
                            float deltaRight = block.Size.Right - taxiLeft;
                            factorX = deltaRight / -deltaPosition.X;
                        }

                        float factorY = 0f;
                        if (deltaPosition.Y > 0)
                        {
                            float deltaTop = taxiBottom - block.Size.Top;
                            factorY = deltaTop / deltaPosition.Y;
                        }
                        else
                        {
                            float deltaBottom = block.Size.Bottom - taxiTop;
                            factorY = deltaBottom / -deltaPosition.Y;
                        }

                        if (Math.Abs(factorX) > 1f)
                            factorX = 0f;
                        if (Math.Abs(factorY) > 1f)
                            factorY = 0f;

                        // TODO: Fix Friction
                        if (factorX > factorY)
                        {
                            taxi.Position -= new Vector2(deltaPosition.X * (factorX + 0.05f), 0);

                            if (Math.Abs(taxi.Velocity.X) > 20f)
                                sound.PlayCrash(Math.Max(0, Math.Min(1f, Math.Abs(taxi.Velocity.X) / 100)));

                            taxi.Velocity *= new Vector2(0, 1);
                        }
                        else
                        {
                            taxi.Position -= new Vector2(0, deltaPosition.Y * (factorY + 0.05f));

                            if (Math.Abs(taxi.Velocity.Y) > 20f)
                                sound.PlayCrash(Math.Max(0, Math.Min(1f, Math.Abs(taxi.Velocity.Y) / 100)));

                            taxi.Velocity *= new Vector2(1, 0);

                            taxi.Velocity -= taxi.Velocity / block.Friction *
                                (float)gameTime.ElapsedGameTime.TotalSeconds;

                            if (taxi.Position.X - (taxi.Size.X / 2) >= block.Size.Left &&
                                taxi.Position.X + (taxi.Size.X / 2) <= block.Size.Right &&
                                taxi.Position.Y < block.Size.Top)
                            {
                                taxi.OnGround = block;

                                if (taxi.Velocity.LengthSquared() < 100)
                                {
                                    var guest = taxi.Guests.SingleOrDefault(g => g.Destination == block);
                                    if (guest != null)
                                    {
                                        taxi.Guests.Remove(guest);
                                        guest.Position = new Vector2(taxi.Position.X, guest.Destination.Size.Top);
                                        guest.CurrentBlock = guest.Destination;
                                        points++;
                                    }
                                }
                            }
                        }

                        scratchVolume = Math.Max(0, Math.Min(1, taxi.Velocity.Length() / 100));
                        scratchPitch = (scratchVolume - 0.5f) * 2;
                    }
                }

                sound.SetScratchPitch(taxi, -scratchPitch);
                sound.SetScratchVolume(taxi, scratchVolume);

                #endregion
            }

            #region Respawn Guests

            if (level.Guests.Count < level.ParallelGuests)
            {
                var platforms = level.Blocks.Where(b => b.SpawnPlatform & !level.Guests.Any(g => g.Departure == b)).ToList();
                var departurePlatform = platforms[rand.Next(platforms.Count)];
                platforms.Remove(departurePlatform);
                var destinationPlatform = platforms[rand.Next(platforms.Count)];


                Guest guest = new Guest()
                {
                    Position = new Vector2(
                        departurePlatform.Size.X + (departurePlatform.Size.Width / 2),
                        departurePlatform.Size.Y),
                    CurrentBlock = departurePlatform,
                    Departure = departurePlatform,
                    Destination = destinationPlatform,
                };

                level.Guests.Add(guest);
                sound.PlayShout();
            }

            #endregion

            foreach (var guest in level.Guests.ToArray())
            {
                #region Einsteigen

                foreach (var taxi in taxis)
                {
                    if (guest.Departure == taxi.OnGround &&
                        (guest.Position - taxi.Position).LengthSquared() < 10000 &&
                        taxi.Velocity.LengthSquared() < 10 &&
                        taxi.Guests.Count < taxi.GuestLimit &&
                        guest.CurrentBlock == guest.Departure)
                    {
                        float guestSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 20;
                        guest.Position += new Vector2(Math.Max(-guestSpeed, Math.Min(guestSpeed, taxi.Position.X - guest.Position.X)), 0);

                        if (Math.Abs(taxi.Position.X - guest.Position.X) < 4)
                        {
                            guest.CurrentBlock = null;
                            taxi.Guests.Add(guest);
                        }
                    }
                }

                #endregion

                #region Aussteigen

                if (guest.CurrentBlock == guest.Destination)
                {
                    float guestSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 20;
                    guest.Position += new Vector2(Math.Max(-guestSpeed, Math.Min(guestSpeed, guest.Destination.Size.X + (guest.Destination.Size.Width / 2) - guest.Position.X)), 0);

                    if (Math.Abs(guest.Destination.Size.X + (guest.Destination.Size.Width / 2) - guest.Position.X) < 4)
                    {
                        level.Guests.Remove(guest);
                    }
                }

                #endregion
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.LinearWrap);

            // Hintergrund
            spriteBatch.Draw(background,
                new Rectangle(0, 0, (int)level.Size.X, (int)level.Size.Y),
                new Rectangle(0, 0, (int)level.Size.X, (int)level.Size.Y), Color.White);

            // Blöcke
            foreach (var block in level.Blocks)
                spriteBatch.Draw(stone, block.Size, block.Size, Color.White);

            // Gäste
            foreach (var guest in level.Guests)
            {
                if (guest.CurrentBlock == null || guest.CurrentBlock == guest.Destination)
                {
                    Vector2 flagPos = new Vector2(guest.Destination.Size.X +
                        (guest.Destination.Size.Width / 2), guest.Destination.Size.Y);

                    spriteBatch.Draw(flag, new Rectangle((int)flagPos.X, (int)(flagPos.Y - flag.Height), 5, flag.Height), new Rectangle(0, 0, 5, flag.Height), Color.White);
                    spriteBatch.Draw(flag, new Rectangle((int)flagPos.X + 5, (int)(flagPos.Y - flag.Height), 25, flag.Height), new Rectangle((gameTime.TotalGameTime.Milliseconds / 250 % 2 == 0 ? 5 : 31), 0, 25, flag.Height), Color.Green);
                }

                if (guest.CurrentBlock == null)
                    continue;

                spriteBatch.Draw(eva,
                    new Rectangle((int)guest.Position.X - (Guest.GUEST_WIDTH / 4), (int)guest.Position.Y - (Guest.GUEST_HEIGHT / 2) + 3, Guest.GUEST_WIDTH / 2, Guest.GUEST_HEIGHT / 2),
                    new Rectangle(Guest.GUEST_WIDTH, 0, Guest.GUEST_WIDTH, Guest.GUEST_HEIGHT),
                    Color.White);
                spriteBatch.Draw(taxiSign, new Vector2(guest.Position.X + 5, guest.Position.Y - (Guest.GUEST_HEIGHT / 2) - taxiSign.Height + 10), Color.White);
            }

            spriteBatch.End();

            spriteBatch.Begin();

            int index = 0;
            foreach (var taxi in taxis)
            {
                spriteBatch.Draw(taxiTextures[index], new Rectangle(
                    (int)(taxi.Position.X - taxi.Size.X / 2),
                    (int)(taxi.Position.Y - taxi.Size.Y / 2),
                    (int)taxi.Size.X,
                    (int)taxi.Size.Y), Color.White);
                if (taxi.OnGround != null)
                    spriteBatch.Draw(pix, new Rectangle(10, 10, 10, 10), Color.Red);

                spriteBatch.Draw(pix, new Rectangle(10 + (index * 25), 10, 20, 20), playerColor[index]);
                spriteBatch.DrawString(hudFont, "0", new Vector2(11 + (index * 25), 11), Color.White);

                index++;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            if (isServer)
                server.Stop();
            else
                client.Close();
        }
    }
}
