// <copyright file="EntityBase.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics.Dynamics;
using HoardeGame.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Base class for entities
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// Gets or sets the level in which this entity exists
        /// </summary>
        public DungeonLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the health of the entity
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Gets or sets the physics body of the entity
        /// </summary>
        public Body Body { get; set; }

        /// <summary>
        /// Gets the position of the <see cref="Body"/>
        /// </summary>
        public Vector2 Position => Body.Position;

        /// <summary>
        /// Gets the velocity of the <see cref="Body"/>
        /// </summary>
        public Vector2 Velocity => Body.LinearVelocity;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase"/> class.
        /// </summary>
        /// <param name="world"><see cref="World"/> to place this entity in</param>
        public EntityBase(World world)
        {
        }

        /// <summary>
        /// Update the entity
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draw the entity
        /// </summary>
        /// <param name="spriteBatch"><see cref="GameTime"/></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}