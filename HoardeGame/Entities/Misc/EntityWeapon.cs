// <copyright file="EntityWeapon.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using HoardeGame.Entities.Base;
using HoardeGame.Gameplay.Level;
using HoardeGame.Gameplay.Weapons;
using HoardeGame.Input;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.Entities.Misc
{
    /// <summary>
    /// Base entity for all weapons
    /// </summary>
    public class EntityWeapon : EntityBase
    {
        private readonly List<EntityBullet> bullets = new List<EntityBullet>();
        private float fireTimer;
        private Vector2 pointerEnd;

        /// <summary>
        /// Gets or sets the current ammo type
        /// </summary>
        public BulletInfo CurrentAmmo { get; set; }

        /// <summary>
        /// Gets or sets the current weapon info
        /// </summary>
        public WeaponInfo WeaponInfo { get; set; }

        private readonly DungeonLevel level;
        private readonly IResourceProvider resourceProvider;
        private readonly IInputProvider inputProvider;
        private readonly EntityBase owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityWeapon"/> class.
        /// </summary>
        /// <param name="level"> <see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to use for loading resources</param>
        /// <param name="inputProvider"><see cref="IInputProvider"/> for user input</param>
        /// <param name="owner"><see cref="EntityBase"/> owner of the weapon</param>
        /// <param name="weapon"><see cref="WeaponInfo"/> to use</param>
        /// <param name="bullet"><see cref="BulletInfo"/> to use, 1st ammo type if null</param>
        public EntityWeapon(DungeonLevel level, IResourceProvider resourceProvider, IInputProvider inputProvider, EntityBase owner, WeaponInfo weapon, BulletInfo bullet = null) : base(level)
        {
            this.level = level;
            this.resourceProvider = resourceProvider;
            this.inputProvider = inputProvider;
            this.owner = owner;

            WeaponInfo = weapon;
            CurrentAmmo = bullet ?? WeaponInfo.Bullets[0];
        }

        /// <summary>
        /// Shoots
        /// </summary>
        /// <param name="direction">Direction of fired shot</param>
        /// <param name="friendly">Whether the bullet is fired by a friendly entity</param>
        /// <returns>Whether the gun shot</returns>
        public bool Shoot(Vector2 direction, bool friendly = true)
        {
            if (fireTimer > CurrentAmmo.Delay)
            {
                resourceProvider.GetSoundEffect("Fire").Play();

                BulletOwnershipInfo info = new BulletOwnershipInfo(friendly ? Faction.Player : Faction.Enemies, this);

                bullets.Add(new EntityBullet(level, resourceProvider, ConvertUnits.ToDisplayUnits(owner.Position), direction, CurrentAmmo.Speed, CurrentAmmo.Lifetime, info));
                fireTimer = 0;

                return true;
            }

            return false;
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
        public override void Draw(SpriteBatch spriteBatch, EffectParameter parameter)
        {
            foreach (EntityBullet bullet in bullets)
            {
                bullet.Draw(spriteBatch, parameter);
            }

            if (WeaponInfo.HasLaserPointer)
            {
                if (inputProvider.MouseState.RightButton == ButtonState.Pressed)
                {
                    Level.World.RayCast(
                        DetermineRayCast,
                        owner.Position,
                        owner.Position + ConvertUnits.ToSimUnits(new Vector2(16)) + 100 * owner.ShootingDirection);

                    spriteBatch.Draw(
                        resourceProvider.GetTexture("OneByOneEmpty"),
                        new Rectangle((int)owner.ScreenPosition.X + 16, (int)owner.ScreenPosition.Y + 16, GetPointerDistance(), 1),
                        null,
                        Color.White,
                        (float)Math.Atan2(owner.ShootingDirection.Y, owner.ShootingDirection.X),
                        Vector2.Zero,
                        SpriteEffects.None,
                        0f);
                }
            }

            base.Draw(spriteBatch, parameter);
        }

        private float DetermineRayCast(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
        {
            if (fixture.UserData != null && fixture.UserData.ToString() == "player")
            {
                return -1;
            }

            pointerEnd = point;
            return fraction;
        }

        private int GetPointerDistance()
        {
            float rayDistance = Vector2.Distance(owner.Position, pointerEnd);
            return (int)Math.Min(ConvertUnits.ToDisplayUnits(CurrentAmmo.Lifetime), ConvertUnits.ToDisplayUnits(rayDistance));
        }
    }
}