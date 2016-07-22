// <copyright file="EntityBat.cs" company="Kuub Studios">
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
    /// Bat entity
    /// </summary>
    public class EntityBat : EntityBaseEnemy
    {
        private readonly AnimatedSprite animator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBat"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to load resources with</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> for acessing the player entity</param>
        public EntityBat(DungeonLevel level, IResourceProvider resourceProvider, IPlayerProvider playerProvider) : base(level, resourceProvider, playerProvider)
        {
            FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(10f), 1f, Body);
            Body.Position = ConvertUnits.ToSimUnits(new Vector2(500, 500));
            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = Category.All;
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;

            Health = 3;
            MinGemDrop = 3;
            MaxGemDrop = 5;

            animator = new AnimatedSprite(resourceProvider.GetTexture("BatSheet"));
            animator.AddAnimation("Flap", 32, 0, 2, 100);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            animator.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, Effect effect)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);
            animator.DrawAnimation("Flap", screenPos, spriteBatch, CurrentBlinkFrame);
        }
    }
}