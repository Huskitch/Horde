// <copyright file="EntityGem.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

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
    /// Gem entity
    /// </summary>
    public class EntityGem : EntityBase
    {
        private AnimatedSprite animator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityGem"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        public EntityGem(DungeonLevel level, IResourceProvider resourceProvider) : base(level)
        {
            Body = BodyFactory.CreateCircle(Level.World, 4, 1);
            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = Category.All;

            animator = new AnimatedSprite(resourceProvider.GetTexture("GemAnimation"));
            animator.AddAnimation("Bounce", 12, 0, 7, 150);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            animator.Update(gameTime);

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch)
        {
            animator.DrawAnimation("Bounce", Position, spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}