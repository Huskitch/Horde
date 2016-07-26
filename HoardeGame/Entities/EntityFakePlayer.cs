// <copyright file="EntityFakePlayer.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using HoardeGame.Gameplay;
using HoardeGame.Graphics.Rendering;
using HoardeGame.Level;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Fake AI player for main menu
    /// </summary>
    public class EntityFakePlayer : EntityBase, IPlayer
    {
        /// <inheritdoc/>
        public int Armour { get; set; }

        /// <inheritdoc/>
        public int Ammo { get; set; }

        /// <inheritdoc/>
        public int[] Gems { get; }

        /// <inheritdoc/>
        public bool Dead { get; set; }

        /// <inheritdoc/>
        public int ChestKeys { get; set; }

        /// <inheritdoc/>
        public EntityWeapon Weapon { get; set; }

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
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        public EntityFakePlayer(DungeonLevel level, IResourceProvider resourceProvider) : base(level)
        {
            animator = new AnimatedSprite(resourceProvider.GetTexture("PlayerSheet"));
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

            Weapon = new EntityWeapon(level, resourceProvider, this);
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