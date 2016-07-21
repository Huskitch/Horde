// <copyright file="EntityWeapon.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.Generic;
using FarseerPhysics;
using HoardeGame.Level;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Base entity for all weapons
    /// </summary>
    public class EntityWeapon : EntityBase
    {
        private readonly List<EntityBullet> bullets = new List<EntityBullet>();
        private float fireTimer;

        /// <summary>
        /// Gets or sets the fire rate of the weapon
        /// </summary>
        public int FireRate { get; set; } = 100;

        private readonly DungeonLevel level;
        private readonly IResourceProvider resourceProvider;
        private readonly EntityBase owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityWeapon"/> class.
        /// </summary>
        /// <param name="level"> <see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to use for loading resources</param>
        /// <param name="owner"><see cref="EntityBase"/> owner of the weapon</param>
        public EntityWeapon(DungeonLevel level, IResourceProvider resourceProvider, EntityBase owner) : base(level)
        {
            this.level = level;
            this.resourceProvider = resourceProvider;
            this.owner = owner;
        }

        /// <summary>
        /// Shoots
        /// </summary>
        /// <param name="direction">Direction of fired shot</param>
        public void Shoot(Vector2 direction)
        {
            if (fireTimer > FireRate)
            {
                bullets.Add(new EntityBullet(level, resourceProvider, ConvertUnits.ToDisplayUnits(owner.Position), direction));
                fireTimer = 0;
            }
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            fireTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].Removed)
                {
                    bullets[i].Body.Dispose();
                    bullets.Remove(bullets[i]);
                    continue;
                }

                bullets[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, Effect effect)
        {
            foreach (EntityBullet bullet in bullets)
            {
                bullet.Draw(spriteBatch, effect);
            }

            base.Draw(spriteBatch, effect);
        }
    }
}