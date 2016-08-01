// <copyright file="Tile.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Gameplay.Level
{
    /// <summary>
    /// A representation of a tile in a <see cref="DungeonLevel"/>
    /// </summary>
    public class Tile : Graphics.IDrawable
    {
        /// <inheritdoc/>
        public Vector2 Position { get; set; }

        /// <inheritdoc/>
        public Vector2 ScreenPosition => ConvertUnits.ToDisplayUnits(Position);

        /// <inheritdoc/>
        public float Depth { get; private set; }

        /// <summary>
        /// Gets the size of this tile
        /// </summary>
        public Vector2 Scale { get; private set; }

        private readonly Texture2D texture;
        private readonly Rectangle source;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="pos"><see cref="Vector2"/> to place the tile on</param>
        /// <param name="scale"><see cref="Vector2"/> to use for scaling</param>
        /// <param name="texture"><see cref="Texture2D"/> of the tile</param>
        /// <param name="collide">Whether to generate collision boxes</param>
        /// <param name="level"><see cref="DungeonLevel"/> to place this tile in</param>
        /// <param name="type">Type of the wall</param>
        public Tile(Vector2 pos, Vector2 scale, Texture2D texture, bool collide, DungeonLevel level, int type)
        {
            Scale = scale;
            this.texture = texture;
            Position = pos / 32f;
            Depth = 0;
            source = new Rectangle(0, 0, 32, 32);

            if (collide)
            {
                Fixture fixture = FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(32), 1f, ConvertUnits.ToSimUnits(pos), level.Body);
                fixture.CollisionCategories = Category.Cat4;
                fixture.CollidesWith = Category.All;
                Depth = Position.Y;
                source = new Rectangle(type * 32, 0, 32, 56);
            }
        }

        /// <summary>
        /// Update the tile
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draw the tile
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/></param>
        /// <param name="parameter"><see cref="EffectParameter"/> for flashing effect</param>
        public void Draw(SpriteBatch spriteBatch, EffectParameter parameter)
        {
            spriteBatch.Draw(texture, new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, (int)Scale.X, (int)Scale.Y), source, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}