using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Level
{
    public class Tile
    {
        public Vector2 Position { get; private set; }
        public Vector2 Scale { get; private set; }
        private Texture2D Texture;

        public Tile(Vector2 pos, Vector2 scale, Texture2D texture)
        {
            Position = pos;
            Scale = scale;
            Texture = texture;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, (int)Scale.X, (int)Scale.Y), null, Color.White, 0f,
                Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}
