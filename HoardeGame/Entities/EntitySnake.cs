// <copyright file="EntitySnake.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Graphics.Rendering;
using HoardeGame.Level;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Snake entity
    /// </summary>
    public class EntitySnake : EntityBaseEnemy
    {
        private readonly AnimatedSprite animator;
        private IPlayerProvider playerProvider;
        private float walkTimer;
        private Vector2 direction;
        private Random rng = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySnake"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to load resources with</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> for acessing the player entity</param>
        public EntitySnake(DungeonLevel level, IResourceProvider resourceProvider, IPlayerProvider playerProvider) : base(level, resourceProvider, playerProvider)
        {
            FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(10f), 1f, Body);
            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = Category.All;
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;

            this.playerProvider = playerProvider;

            Health = 3;
            MinGemDrop = 3;
            MaxGemDrop = 5;

            animator = new AnimatedSprite(resourceProvider.GetTexture("SnakeSheet"));
            animator.AddAnimation("North", 48, 1, 3, 100);
            animator.AddAnimation("East", 48, 2, 3, 100);
            animator.AddAnimation("South", 48, 3, 3, 100);
            animator.AddAnimation("West", 48, 0, 3, 100);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            UpdateAI(gameTime);
            animator.Update(gameTime);
        }

        /// <summary>
        /// Updates the AI state
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public void UpdateAI(GameTime gameTime)
        {
            walkTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Vector2.Distance(playerProvider.Player.Position, Position) > 5)
            {
                if (walkTimer > rng.Next(100, 3000))
                {
                    direction = new Vector2(rng.Next(-1, 2), rng.Next(-1, 2));
                    walkTimer = 0;
                }
            }
            else
            {
                direction = playerProvider.Player.Position - Position;
                direction.Normalize();
            }

            Body.ApplyForce(direction * 20);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, Effect effect)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);

            if (direction == new Vector2(0, -1))
            {
                animator.DrawAnimation("North", screenPos, spriteBatch, CurrentBlinkFrame);
            }
            else if (direction == new Vector2(1, 0))
            {
                animator.DrawAnimation("East", screenPos, spriteBatch, CurrentBlinkFrame);
            }
            else if (direction == new Vector2(0, 1))
            {
                animator.DrawAnimation("South", screenPos, spriteBatch, CurrentBlinkFrame);
            }
            else if (direction == new Vector2(-1, 0))
            {
                animator.DrawAnimation("West", screenPos, spriteBatch, CurrentBlinkFrame);
            }
            else
            {
                animator.DrawAnimation("South", screenPos, spriteBatch, CurrentBlinkFrame);
            }
        }
    }
}