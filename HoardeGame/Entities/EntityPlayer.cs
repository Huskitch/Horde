// <copyright file="EntityPlayer.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Graphics.Rendering;
using HoardeGame.Input;
using HoardeGame.Level;
using HoardeGame.Resources;
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
        private IInputProvider inputProvider;
        private float fireTimer;
        private int fireRate = 100;

        private enum Directions
        {
            NORTH,
            EAST,
            SOUTH,
            WEST,
            NORTHEAST,
            NORTHWEST,
            SOUTHEAST,
            SOUTHWEST
        }

        private readonly IResourceProvider resourceProvider;
        private Directions direction;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPlayer"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> in which the player will spawn</param>
        /// <param name="inputProvider"><see cref="IInputProvider"/> to use for input</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        public EntityPlayer(DungeonLevel level, IInputProvider inputProvider, IResourceProvider resourceProvider) : base(level)
        {
            this.inputProvider = inputProvider;
            this.resourceProvider = resourceProvider;

            FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(10), 1f, Body);
            Body.Position = Level.GetSpawnPosition();
            Body.CollisionCategories = Category.Cat1;
            Body.CollidesWith = Category.Cat3 | Category.Cat4;

            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;

            Bullets = new List<EntityBullet>();

            Health = 10;

            animator = new AnimatedSprite(resourceProvider.GetTexture("PlayerSheet"));
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
                direction = Directions.SOUTH;
                velocity.Y++;
            }

            if (inputProvider.KeyboardState.IsKeyDown(Keys.W))
            {
                direction = Directions.NORTH;
                velocity.Y--;
            }

            if (inputProvider.KeyboardState.IsKeyDown(Keys.D))
            {
                if (direction == Directions.SOUTH)
                {
                    direction = Directions.SOUTHEAST;
                }
                else if (direction == Directions.NORTH)
                {
                    direction = Directions.NORTHEAST;
                }
                else
                {
                    direction = Directions.EAST;
                }

                velocity.X++;
            }

            if (inputProvider.KeyboardState.IsKeyDown(Keys.A))
            {
                if (direction == Directions.SOUTH)
                {
                    direction = Directions.SOUTHWEST;
                }
                else if (direction == Directions.NORTH)
                {
                    direction = Directions.NORTHWEST;
                }
                else
                {
                    direction = Directions.WEST;
                }

                velocity.X--;
            }

            fireTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (fireTimer > fireRate)
            {
                if (inputProvider.KeyboardState.IsKeyDown(Keys.Space))
                {
                    Vector2 direction = Vector2.Zero;

                    switch (this.direction)
                    {
                        case Directions.NORTH:
                            direction = new Vector2(0, -1);
                            break;
                        case Directions.SOUTH:
                            direction = new Vector2(0, 1);
                            break;
                        case Directions.WEST:
                            direction = new Vector2(-1, 0);
                            break;
                        case Directions.EAST:
                            direction = new Vector2(1, 0);
                            break;
                        case Directions.NORTHEAST:
                            direction = new Vector2(1, -1);
                            break;
                        case Directions.NORTHWEST:
                            direction = new Vector2(-1, -1);
                            break;
                        case Directions.SOUTHEAST:
                            direction = new Vector2(1, 1);
                            break;
                        case Directions.SOUTHWEST:
                            direction = new Vector2(-1, 1);
                            break;
                    }

                    EntityBullet bullet = new EntityBullet(Level, resourceProvider, ConvertUnits.ToDisplayUnits(Position), direction);

                    Bullets.Add(bullet);
                    fireTimer = 0;
                }
             }

            Body.ApplyForce(velocity * 50);

            animator.Update(gameTime);

            for (int i = 0; i < Bullets.Count; i++)
            {
                if (Bullets[i].Removed)
                {
                    Bullets[i].Body.Dispose();
                    Bullets.Remove(Bullets[i]);
                    continue;
                }

                Bullets[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);

            if (Velocity.Length() < 0.1f)
            {
                animator.DrawAnimation("Idle", screenPos, spriteBatch);
            }
            else
            {
                switch (direction)
                {
                    case Directions.NORTH:
                        animator.DrawAnimation("North", screenPos, spriteBatch);
                        break;
                    case Directions.SOUTH:
                        animator.DrawAnimation("South", screenPos, spriteBatch);
                        break;
                    case Directions.WEST:
                        animator.DrawAnimation("West", screenPos, spriteBatch);
                        break;
                    case Directions.EAST:
                        animator.DrawAnimation("East", screenPos, spriteBatch);
                        break;
                    case Directions.NORTHEAST:
                        animator.DrawAnimation("NorthEast", screenPos, spriteBatch);
                        break;
                    case Directions.NORTHWEST:
                        animator.DrawAnimation("NorthWest", screenPos, spriteBatch);
                        break;
                    case Directions.SOUTHEAST:
                        animator.DrawAnimation("SouthEast", screenPos, spriteBatch);
                        break;
                    case Directions.SOUTHWEST:
                        animator.DrawAnimation("SouthWest", screenPos, spriteBatch);
                        break;
                }
            }

            foreach (EntityBullet bullet in Bullets)
            {
                bullet.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }
    }
}