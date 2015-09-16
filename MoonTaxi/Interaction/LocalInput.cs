using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoonTaxi.Interaction
{
    internal class LocalInput : IInput
    {
        private LocalInputType type;
        public LocalInput(LocalInputType type)
        {
            this.type = type;
        }

        public Vector2 GetDirection()
        {
            switch (type)
            {
                case LocalInputType.GamePad1:
                    return GamePad.GetState(PlayerIndex.One).ThumbSticks.Left * new Vector2(1, -1);
                case LocalInputType.GamePad2:
                    return GamePad.GetState(PlayerIndex.Two).ThumbSticks.Left * new Vector2(1, -1);
                case LocalInputType.GamePad3:
                    return GamePad.GetState(PlayerIndex.Three).ThumbSticks.Left * new Vector2(1, -1);
                case LocalInputType.GamePad4:
                    return GamePad.GetState(PlayerIndex.Four).ThumbSticks.Left * new Vector2(1, -1);
                case LocalInputType.Keyboard1:
                    var keyboard1 = Keyboard.GetState();
                    return new Vector2(
                        (keyboard1.IsKeyDown(Keys.Left) ? -1 : 0) + (keyboard1.IsKeyDown(Keys.Right) ? 1 : 0),
                        (keyboard1.IsKeyDown(Keys.Up) ? -1 : 0) + (keyboard1.IsKeyDown(Keys.Down) ? 1 : 0));
                case LocalInputType.Keyboard2:
                    var keyboard2 = Keyboard.GetState();
                    return new Vector2(
                        (keyboard2.IsKeyDown(Keys.A) ? -1 : 0) + (keyboard2.IsKeyDown(Keys.D) ? 1 : 0),
                        (keyboard2.IsKeyDown(Keys.W) ? -1 : 0) + (keyboard2.IsKeyDown(Keys.S) ? 1 : 0));
                default: return Vector2.Zero;
            }
        }
    }

    public enum LocalInputType
    {
        GamePad1,
        GamePad2,
        GamePad3,
        GamePad4,
        Keyboard1,
        Keyboard2,
    }
}
