// <copyright file="EntityPlayer.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Graphics.Rendering;
using HoardeGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Player entity
    /// </summary>
    public class EntityPlayer : EntityBase
    {
        /// <summary>
        /// Gets instance of <see cref="EntityPlayer"/>
        /// </summary>
        public static EntityPlayer Player { get; private set; }

        /// <summary>
        /// Gets a list of bullets shot by player
        /// </summary>
        public List<EntityBullet> Bullets { get; private set; }

        private AnimatedSprite animator;
        private World worldInstance;
        private IInputProvider inputProvider;

        private enum Directions
        {
            NORTH,
            EAST,
            SOUTH,
            WEST
        }

        private Directions dir;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPlayer"/> class.
        /// </summary>
        /// <param name="world"><see cref="World"/> to place this entity in</param>
        /// <param name="inputProvider"><see cref="IInputProvider"/> to use for input</param>
        public EntityPlayer(World world, IInputProvider inputProvider) : base(world)
        {
            this.inputProvider = inputProvider;

            Body = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(10), 1f, ConvertUnits.ToSimUnits(new Vector2(400, 400)));
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;

            Bullets = new List<EntityBullet>();

            worldInstance = world;

            Health = 10;

            animator = new AnimatedSprite(ResourceManager.GetTexture("PlayerSheet"));
            animator.AddAnimation("South", 32, 0, 5, 100);
            animator.AddAnimation("West", 32, 2, 5, 100);
            animator.AddAnimation("East", 32, 6, 5, 100);
            animator.AddAnimation("North", 32, 4, 5, 100);
            animator.AddAnimation("SouthWest", 32, 1, 5, 100);
            animator.AddAnimation("SouthEast", 32, 7, 5, 100);
            animator.AddAnimation("NorthWest", 32, 3, 5, 100);
            animator.AddAnimation("NorthEast", 32, 5, 5, 100);
            animator.AddAnimation("Idle", 32, 8, 5, 100);
            animator.SetDefaultAnimation("Idle");

            Player = this;
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            Vector2 velocity = Vector2.Zero;

            if (inputProvider.KeyboardState.IsKeyDown(Keys.S))
            {
                dir = Directions.SOUTH;
                velocity.Y++;
            }

            if (inputProvider.KeyboardState.IsKeyDown(Keys.W))
            {
                dir = Directions.NORTH;
                velocity.Y--;
            }

            if (inputProvider.KeyboardState.IsKeyDown(Keys.D))
            {
                dir = Directions.EAST;
                velocity.X++;
            }

            if (inputProvider.KeyboardState.IsKeyDown(Keys.A))
            {
                dir = Directions.WEST;
                velocity.X--;
            }

            if (inputProvider.KeyboardState.IsKeyDown(Keys.Space))
            {
                Vector2 direction = Vector2.Zero;

                if (dir == Directions.NORTH)
                {
                    direction = new Vector2(0, -1);
                }

                Bullets.Add(new EntityBullet(worldInstance, ConvertUnits.ToDisplayUnits(Position), direction));
            }

            Body.ApplyForce(velocity * 50);

            animator.Update(gameTime);

            foreach (EntityBullet bullet in Bullets)
            {
                bullet.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);

            if (inputProvider.KeyboardState.IsKeyDown(Keys.S) && inputProvider.KeyboardState.IsKeyDown(Keys.A))
            {
                animator.DrawAnimation("SouthWest", screenPos, spriteBatch);
            }
            else if (inputProvider.KeyboardState.IsKeyDown(Keys.W) && inputProvider.KeyboardState.IsKeyDown(Keys.A))
            {
                animator.DrawAnimation("NorthWest", screenPos, spriteBatch);
            }
            else if (inputProvider.KeyboardState.IsKeyDown(Keys.D) && inputProvider.KeyboardState.IsKeyDown(Keys.S))
            {
                animator.DrawAnimation("SouthEast", screenPos, spriteBatch);
            }
            else if (inputProvider.KeyboardState.IsKeyDown(Keys.W) && inputProvider.KeyboardState.IsKeyDown(Keys.D))
            {
                animator.DrawAnimation("NorthEast", screenPos, spriteBatch);
            }
            else if (inputProvider.KeyboardState.IsKeyDown(Keys.W))
            {
                animator.DrawAnimation("North", screenPos, spriteBatch);
            }
            else if (inputProvider.KeyboardState.IsKeyDown(Keys.S))
            {
                animator.DrawAnimation("South", screenPos, spriteBatch);
            }
            else if (inputProvider.KeyboardState.IsKeyDown(Keys.A))
            {
                animator.DrawAnimation("West", screenPos, spriteBatch);
            }
            else if (inputProvider.KeyboardState.IsKeyDown(Keys.D))
            {
                animator.DrawAnimation("East", screenPos, spriteBatch);
            }
            else
            {
                animator.DrawLastUsedAnimation(screenPos, spriteBatch);
            }

            foreach (EntityBullet bullet in Bullets)
            {
                bullet.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }
    }
}