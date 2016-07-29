// <copyright file="EntityFlyingDamageIndicator.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using HoardeGame.Level;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = HoardeGame.Graphics.Rendering.IDrawable;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Flying text indicating how much damage was dealt
    /// </summary>
    public class EntityFlyingDamageIndicator : EntityBase
    {
        /// <summary>
        /// Gets or sets the displayed damage
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Gets or sets the color of the text
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the lifetime of this entity in frames
        /// </summary>
        public int LifeTime { get; set; }

        /// <summary>
        /// Gets or sets velocity of movement
        /// Only use when FollowTarget is not set
        /// </summary>
        public new Vector2 Velocity { get; set; }

        /// <summary>
        /// Gets or sets the target to follow
        /// Only use when Velocity is not set
        /// </summary>
        public IDrawable FollowTarget { get; set; }

        private readonly IResourceProvider resourceProvider;

        private int lifetime;
        private Vector2 size = Vector2.Zero;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFlyingDamageIndicator"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place the entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to load resourced with</param>
        public EntityFlyingDamageIndicator(DungeonLevel level, IResourceProvider resourceProvider) : base(level)
        {
            this.resourceProvider = resourceProvider;
        }

        /// <inheritdoc/>
        public override void Start()
        {
            lifetime = LifeTime;
            base.Start();
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (size == Vector2.Zero)
            {
                size = resourceProvider.GetFont("SmallFont").MeasureString(Damage.ToString());
                size = ConvertUnits.ToSimUnits(size) / 2;
            }

            if (FollowTarget != null)
            {
                Body.Position = FollowTarget.Position - new Vector2(-size.X, size.Y);
            }
            else
            {
                Body.Position += Velocity;
            }

            LifeTime--;

            if (LifeTime <= 0)
            {
                Removed = true;
            }

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, EffectParameter parameter)
        {
            spriteBatch.DrawString(resourceProvider.GetFont("SmallFont"), Damage.ToString(), ScreenPosition, Color, 0f, Vector2.Zero, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
        }
    }
}