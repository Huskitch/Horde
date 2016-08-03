// <copyright file="EntityFakePlayer.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Entities.Base;
using HoardeGame.Entities.Misc;
using HoardeGame.Gameplay.Gems;
using HoardeGame.Gameplay.Level;
using HoardeGame.Gameplay.Player;
using HoardeGame.Gameplay.Weapons;
using HoardeGame.Graphics;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities.Player
{
    /// <summary>
    /// Fake AI player for main menu
    /// </summary>
    public class EntityFakePlayer : EntityBase, IPlayer
    {
        /// <inheritdoc/>
        public int Armour { get; set; }

        /// <inheritdoc/>
        public GemInfo Gems { get; } = new GemInfo();

        /// <inheritdoc/>
        public bool Dead { get; set; }

        /// <inheritdoc/>
        public EntityWeapon Weapon { get; set; }

        /// <inheritdoc/>
        public List<EntityWeapon> InventoryWeapons { get; set; }

        private AnimatedSprite animator;
        private Directions direction;

        private enum Directions
        {
            EAST,
            NORTHEAST,
            NORTH,
            NORTHWEST,
            WEST,
            SOUTHWEST,
            SOUTH,
            SOUTHEAST
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFakePlayer"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        public EntityFakePlayer(DungeonLevel level, GameServiceContainer serviceContainer) : base(level, serviceContainer)
        {
            FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(10), 1f, Body);
            Body.Position = Level.GetSpawnPosition();
            Body.CollisionCategories = Category.Cat1;
            Body.CollidesWith = Category.Cat3 | Category.Cat4;

            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;

            var resourceProvider = serviceContainer.GetService<IResourceProvider>();

            animator = new AnimatedSprite(resourceProvider.GetTexture("PlayerSheet"), serviceContainer);
            animator.AddAnimation("South", 32, 0, 5, 100);
            animator.AddAnimation("West", 32, 2, 5, 100);
            animator.AddAnimation("East", 32, 6, 5, 100);
            animator.AddAnimation("North", 32, 4, 5, 100);
            animator.AddAnimation("SouthWest", 32, 1, 5, 100);
            animator.AddAnimation("SouthEast", 32, 7, 5, 100);
            animator.AddAnimation("NorthWest", 32, 3, 5, 100);
            animator.AddAnimation("NorthEast", 32, 5, 5, 100);
            animator.AddAnimation("Idle", 32, 8, 5, 100);
            animator.SetDefaultAnimation("Idle");

            Weapon = new EntityWeapon(level, serviceContainer, this, serviceContainer.GetService<IWeaponProvider>().GetWeapon("fakeWeapon"));
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (Dead)
            {
                return;
            }

            Vector2 velocity = new Vector2(0, 0);

            if (velocity != Vector2.Zero)
            {
                direction = GetDirection(velocity);
            }

            Vector2 shootingDirection = new Vector2(0, 0);
            direction = GetDirection(shootingDirection);

            shootingDirection.Normalize();

            // Weapon.Shoot(shootingDirection);
            Body.ApplyForce(velocity * 50);

            animator.Update(gameTime);
            Weapon.Update(gameTime);

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(EffectParameter parameter)
        {
            if (Dead)
            {
                return;
            }

            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);

            if (Velocity.Length() < 0.1f)
            {
                animator.DrawAnimation("Idle", screenPos, Color.White, parameter, CurrentBlinkFrame);
            }
            else
            {
                switch (direction)
                {
                    case Directions.NORTH:
                        animator.DrawAnimation("North", screenPos, Color.White, parameter, CurrentBlinkFrame);
                        break;
                    case Directions.SOUTH:
                        animator.DrawAnimation("South", screenPos, Color.White, parameter, CurrentBlinkFrame);
                        break;
                    case Directions.WEST:
                        animator.DrawAnimation("West", screenPos, Color.White, parameter, CurrentBlinkFrame);
                        break;
                    case Directions.EAST:
                        animator.DrawAnimation("East", screenPos, Color.White, parameter, CurrentBlinkFrame);
                        break;
                    case Directions.NORTHEAST:
                        animator.DrawAnimation("NorthEast", screenPos, Color.White, parameter, CurrentBlinkFrame);
                        break;
                    case Directions.NORTHWEST:
                        animator.DrawAnimation("NorthWest", screenPos, Color.White, parameter, CurrentBlinkFrame);
                        break;
                    case Directions.SOUTHEAST:
                        animator.DrawAnimation("SouthEast", screenPos, Color.White, parameter, CurrentBlinkFrame);
                        break;
                    case Directions.SOUTHWEST:
                        animator.DrawAnimation("SouthWest", screenPos, Color.White, parameter, CurrentBlinkFrame);
                        break;
                }
            }

            Weapon.Draw(parameter);

            base.Draw(parameter);
        }

        private Directions GetDirection(Vector2 velocity)
        {
            return (Directions)(((int)Math.Round(Math.Atan2(-velocity.Y, velocity.X) / (2 * Math.PI / 8)) + 8) % 8);
        }

        private Vector2 GetVector(Directions directions)
        {
            switch (directions)
            {
                case Directions.NORTH:
                    return new Vector2(0, -1);
                case Directions.SOUTH:
                    return new Vector2(0, 1);
                case Directions.WEST:
                    return new Vector2(-1, 0);
                case Directions.EAST:
                    return new Vector2(1, 0);
                case Directions.NORTHEAST:
                    return new Vector2(1, -1);
                case Directions.NORTHWEST:
                    return new Vector2(-1, -1);
                case Directions.SOUTHEAST:
                    return new Vector2(1, 1);
                case Directions.SOUTHWEST:
                    return new Vector2(-1, 1);
            }

            return Vector2.Zero;
        }
    }
}