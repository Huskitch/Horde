// <copyright file="EntityBaseShootingEnemy.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.Entities.Misc;
using HoardeGame.Gameplay.Level;
using HoardeGame.Gameplay.Weapons;
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
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        protected EntityBaseShootingEnemy(DungeonLevel level, GameServiceContainer serviceContainer) : base(level, serviceContainer)
        {
            CurrentAmmo = new BulletInfo
            {
                Delay = 100,
                Speed = 10,
                Lifetime = 1000
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
        public override void Draw(EffectParameter parameter)
        {
            Weapon.Draw(parameter);
        }
    }
}