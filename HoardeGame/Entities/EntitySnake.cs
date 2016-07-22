// <copyright file="EntitySnake.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

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

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySnake"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to load resources with</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> for acessing the player entity</param>
        public EntitySnake(DungeonLevel level, IResourceProvider resourceProvider, IPlayerProvider playerProvider) : base(level, resourceProvider, playerProvider)
        {
            FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(14f), 1f, Body);
            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = Category.All;
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;

            this.playerProvider = playerProvider;

            Health = 3;
            MinGemDrop = new[] { 3, 2, 1, 1 };
            MaxGemDrop = new[] { 6, 4, 2, 1 };

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

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, Effect effect)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);

            if (Direction == new Vector2(0, -1))
            {
                animator.DrawAnimation("North", screenPos, spriteBatch, CurrentBlinkFrame);
            }
            else if (Direction == new Vector2(1, 0))
            {
                animator.DrawAnimation("East", screenPos, spriteBatch, CurrentBlinkFrame);
            }
            else if (Direction == new Vector2(0, 1))
            {
                animator.DrawAnimation("South", screenPos, spriteBatch, CurrentBlinkFrame);
            }
            else if (Direction == new Vector2(-1, 0))
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