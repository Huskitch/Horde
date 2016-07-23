// <copyright file="EntityBase.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
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
        private readonly Color[] blinkFrames =
        {
            Color.Black,
            Color.Black,
            Color.Gray,
            Color.Gray,
            Color.White,
            Color.White,
            Color.White,
            Color.White,
            Color.Gray,
            Color.Gray,
            Color.Black,
            Color.Black,
            Color.Black,
            Color.Black,
            Color.DarkRed,
            Color.DarkRed,
            Color.Red,
            Color.Red,
            Color.Red,
            Color.Red,
            Color.DarkRed,
            Color.DarkRed
        };

        private int blinkFrame;
        private int blinkRepeat;

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
        /// Gets the position of the <see cref="Body"/> in meters
        /// </summary>
        public Vector2 Position => Body.Position;

        /// <summary>
        /// Gets the position of the <see cref="Body"/> in pixels
        /// </summary>
        public Vector2 ScreenPosition => ConvertUnits.ToDisplayUnits(Position);

        /// <summary>
        /// Gets the velocity of the <see cref="Body"/>
        /// </summary>
        public Vector2 Velocity => Body.LinearVelocity;

        /// <summary>
        /// Gets the value of the current blink frame
        /// </summary>
        public Color CurrentBlinkFrame
        {
            // this probably doesn't belong here but it works for now
            get
            {
                Color frame = blinkFrames[blinkFrame];

                if (blinkFrame > 0)
                {
                    blinkFrame--;
                }
                else
                {
                    if (blinkRepeat > 0)
                    {
                        blinkFrame = blinkFrames.Length - 1;
                        blinkRepeat--;
                    }
                }

                return frame;
            }
        }

        /// <summary>
        /// Gets or sets the amount of times the blink animation will play
        /// </summary>
        protected int BlinkMultiplier { get; set; } = 1;

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
        /// Determines whether the entity is being hit (can be used to determine if it was talking shit)
        /// </summary>
        /// <returns>Whether the entity is being hit</returns>
        public bool IsHit()
        {
            if (blinkRepeat > 0 || blinkFrame > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Initializes the entity when it's added into the level
        /// </summary>
        public virtual void Start()
        {
        }

        /// <summary>
        /// Starts the hit animation
        /// </summary>
        public void Hit()
        {
            blinkFrame = blinkFrames.Length - 1;
            blinkRepeat = BlinkMultiplier;
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
        /// <param name="parameter"></param>
        public virtual void Draw(SpriteBatch spriteBatch, EffectParameter parameter)
        {
        }
    }
}