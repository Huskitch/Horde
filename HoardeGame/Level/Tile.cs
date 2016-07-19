// <copyright file="Tile.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Diagnostics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Level
{
    /// <summary>
    /// A representation of a tile in a <see cref="DungeonLevel"/>
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// Gets or sets the position of this tile in meters
        /// </summary>
        public Vector2 Position
        {
            get { return body.Position; }
            set { body.Position = value; }
        }

        /// <summary>
        /// Gets or sets the position this tile in pixels
        /// </summary>
        public Vector2 ScreenPosition
        {
            get
            {
                return ConvertUnits.ToDisplayUnits(body.Position);
            }

            set
            {
                body.Position = ConvertUnits.ToSimUnits(value);
            }
        }

        /// <summary>
        /// Gets the size of this tile
        /// </summary>
        public Vector2 Scale { get; private set; }

        /// <summary>
        /// <see cref="Body"/> representing this tile
        /// </summary>
        private readonly Body body;

        private readonly Texture2D texture;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="pos"><see cref="Vector2"/> to place the tile on</param>
        /// <param name="scale"><see cref="Vector2"/> to use for scaling</param>
        /// <param name="texture"><see cref="Texture2D"/> of the tile</param>
        /// <param name="collide">Whether to generate collision boxes</param>
        /// <param name="world"><see cref="World"/> to place this tile in</param>
        public Tile(Vector2 pos, Vector2 scale, Texture2D texture, bool collide, World world)
        {
            Scale = scale;
            this.texture = texture;

            if (collide)
            {
                body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(32), 1f, new Vector2(0, 24));
                body.IsStatic = true;
                body.Position = ConvertUnits.ToSimUnits(pos);
            }
            else
            {
                body = BodyFactory.CreateBody(world);
                body.IsStatic = true;
                body.Position = ConvertUnits.ToSimUnits(pos);
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
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, (int)Scale.X, (int)Scale.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}