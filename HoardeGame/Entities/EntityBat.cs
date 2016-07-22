// <copyright file="EntityBat.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
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
    /// Bat entity
    /// </summary>
    public class EntityBat : EntityBaseEnemy
    {
        private readonly AnimatedSprite animator;
        private IPlayerProvider playerProvider;
        private float walkTimer;
        private Vector2 direction;
        private Random rng = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBat"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to load resources with</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> for acessing the player entity</param>
        public EntityBat(DungeonLevel level, IResourceProvider resourceProvider, IPlayerProvider playerProvider) : base(level, resourceProvider, playerProvider)
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

            animator = new AnimatedSprite(resourceProvider.GetTexture("BatSheet"));
            animator.AddAnimation("Flap", 32, 0, 2, 100);
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
            animator.DrawAnimation("Flap", screenPos, spriteBatch, CurrentBlinkFrame);
        }
    }
}