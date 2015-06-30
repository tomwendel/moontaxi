using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MoonTaxi.Models;
using System;

namespace MoonTaxi
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MoonTaxi : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Taxi taxi = new Taxi();
        Level level = new Level();

        Texture2D taxiTexture;
        Texture2D background;
        Texture2D stone;

        public MoonTaxi()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = (int)level.Size.X;
            graphics.PreferredBackBufferHeight = (int)level.Size.Y;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            taxiTexture = Content.Load<Texture2D>("Textures/taxi");
            background = Content.Load<Texture2D>("Textures/background");
            stone = Content.Load<Texture2D>("Textures/stone");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            taxi.Update(gameTime);

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

            Vector2 deltaPosition = taxi.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            float taxiLeft = taxi.Position.X - sizeHalf.X; 
            float taxiTop = taxi.Position.Y - sizeHalf.Y;
            float taxiRight = taxi.Position.X + sizeHalf.X;
            float taxiBottom = taxi.Position.Y + sizeHalf.Y;

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

                    if (factorX > factorY)
                    {
                        taxi.Position -= new Vector2(deltaPosition.X * (factorX + 0.05f), 0);
                        taxi.Velocity *= new Vector2(0, 1);
                    }
                    else
                    {
                        taxi.Position -= new Vector2(0, deltaPosition.Y * (factorY + 0.05f));
                        taxi.Velocity *= new Vector2(1, 0);
                    }
                }

                //if (taxiRectangle.Intersects(block.Size))
                //{
                //    //float factorX = 0f;

                //    if (taxi.Velocity.X > 0)
                //    {
                //        int deltaLeft = block.Size.Left - taxiRectangle.Right;
                //        taxi.Position -= new Vector2(deltaLeft, 0);
                //        taxi.Velocity *= new Vector2(0, 1);
                //        //factorX = deltaLeft / deltaPosition.X;
                //    }
                //    else
                //    {
                //        int deltaRight = block.Size.Right - taxiRectangle.Left;
                //        taxi.Position -= new Vector2(deltaRight, 0);
                //        taxi.Velocity *= new Vector2(0, 1);

                //        // factorX = deltaRight / deltaPosition.X;
                //    }

                //    float factorY = 0f;
                //    if (taxi.Velocity.Y > 0)
                //    {
                //        int deltaTop = taxiRectangle.Bottom - block.Size.Top;
                //        taxi.Position -= new Vector2(0, deltaTop);
                //        taxi.Velocity *= new Vector2(1, 0);

                //        // factorY = deltaTop / deltaPosition.Y;
                //    }
                //    else
                //    {
                //        int deltaBottom = block.Size.Bottom - taxiRectangle.Top;
                //        taxi.Position -= new Vector2(0, deltaBottom);
                //        taxi.Velocity *= new Vector2(1, 0);

                //        // factorY = deltaBottom / deltaPosition.Y;
                //    }

                
                //}
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
            
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(taxiTexture, new Rectangle(
                (int)(taxi.Position.X - taxi.Size.X / 2), 
                (int)(taxi.Position.Y - taxi.Size.Y / 2), 
                (int)taxi.Size.X, 
                (int)taxi.Size.Y), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
