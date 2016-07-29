// <copyright file="EntityBaseShootingEnemy.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.Entities.Misc;
using HoardeGame.Gameplay.Level;
using HoardeGame.Gameplay.Player;
using HoardeGame.Gameplay.Weapons;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities.Base
{
    /// <summary>
    /// Base class for all shooting enemies
    /// </summary>
    public class EntityBaseShootingEnemy : EntityBaseEnemy
    {
        /// <summary>
        /// Gets or sets the weapon of this enemy
        /// </summary>
        public EntityWeapon Weapon { get; set; }

        /// <summary>
        /// Gets or sets the current ammo type
        /// </summary>
        public BulletInfo CurrentAmmo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBaseShootingEnemy"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to load resources with</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> for accessing the player entity</param>
        protected EntityBaseShootingEnemy(DungeonLevel level, IResourceProvider resourceProvider, IPlayerProvider playerProvider) : base(level, resourceProvider, playerProvider)
        {
            CurrentAmmo = new BulletInfo
            {
                Delay = 100,
                Speed = 10
            };
        }

        /// <inheritdoc/>
        public override void UpdateAI(GameTime gameTime)
        {
            if (PlayerProvider.Player.Dead)
            {
                return;
            }

            ShootingDirection = PlayerProvider.Player.Position - Position;
            ShootingDirection = Vector2.Normalize(ShootingDirection);

            Weapon.Update(gameTime);
            Weapon.Shoot(ShootingDirection, false);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, EffectParameter parameter)
        {
            Weapon.Draw(spriteBatch, parameter);
        }
    }
}