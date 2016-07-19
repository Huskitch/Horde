// <copyright file="Tile.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Level
{
    public class Tile
    {
        public Vector2 Position;
        public Vector2 Scale;
        private Texture2D Texture;
        public Body Body;

        public Tile(Vector2 pos, Vector2 scale, Texture2D texture, bool collide, World world)
        {
            Position = pos;
            Scale = scale;
            Texture = texture;

            if (collide)
            {
                Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(scale.X), ConvertUnits.ToSimUnits(scale.Y), 1f);
                Body.IsStatic = true;
                Body.Position = ConvertUnits.ToSimUnits(Position);
            }
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