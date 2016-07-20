﻿// <copyright file="EntityBase.cs" company="Kuub Studios">
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
        /// Gets the level in which this entity exists
        /// </summary>
        public DungeonLevel Level { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is going to be removed
        /// </summary>
        public bool Removed { get; set; }

        /// <summary>
        /// Gets or sets the health of the entity
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Gets the physics body of the entity
        /// </summary>
        public Body Body { get; private set; }

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
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        public EntityBase(DungeonLevel level)
        {
            Level = level;
            Body = new Body(level.World);
        }

        /// <summary>
        /// Initializes the entity when it's added into the level
        /// </summary>
        public virtual void Start()
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