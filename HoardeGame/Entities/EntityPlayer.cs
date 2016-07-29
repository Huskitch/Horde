// <copyright file="EntityPlayer.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Gameplay;
using HoardeGame.GameStates;
using HoardeGame.Graphics.Rendering;
using HoardeGame.Input;
using HoardeGame.Level;
using HoardeGame.Resources;
using HoardeGame.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Player entity
    /// </summary>
    public class EntityPlayer : EntityBase, IPlayer
    {
        /// <summary>
        /// Maximum health for the player
        /// </summary>
        public const int MaxHealth = 10;

        /// <summary>
        /// Maximum armour for the player
        /// </summary>
        public const int MaxArmour = 10;

        /// <inheritdoc/>
        public GemInfo Gems { get; } = new GemInfo();

        /// <inheritdoc/>
        public bool Dead { get; set; }

        /// <inheritdoc/>
        public int Ammo { get; set; }

        /// <inheritdoc/>
        public int Armour { get; set; }

        /// <inheritdoc/>
        public EntityWeapon Weapon { get; set; }

        private readonly IResourceProvider resourceProvider;
        private readonly SinglePlayer singlePlayer;

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

        private Directions direction;
        private IWeaponProvider weaponProvider;
        private AnimatedSprite animator;
        private IInputProvider inputProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPlayer"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> in which the player will spawn</param>
        /// <param name="inputProvider"><see cref="IInputProvider"/> to use for input</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        /// <param name="singlePlayer"><see cref="SinglePlayer"/></param>
        public EntityPlayer(DungeonLevel level, IInputProvider inputProvider, IResourceProvider resourceProvider, SinglePlayer singlePlayer) : base(level)
        {
            this.inputProvider = inputProvider;
            this.resourceProvider = resourceProvider;
            this.singlePlayer = singlePlayer;

            FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(10), 1f, Body);
            Body.Position = Level.GetSpawnPosition();
            Body.CollisionCategories = Category.Cat1;
            Body.CollidesWith = Category.Cat3 | Category.Cat4;

            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;

            Health = MaxHealth;
            Armour = MaxArmour;
            Ammo = 100;
            BlinkMultiplier = 1;

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

            Vector2 velocity = new Vector2(inputProvider.GamePadState.ThumbSticks.Left.X, -inputProvider.GamePadState.ThumbSticks.Left.Y);

            if (inputProvider.KeyboardState.IsKeyDown(Keys.S))
            {
                velocity.Y = 1;
            }

            if (inputProvider.KeyboardState.IsKeyDown(Keys.W))
            {
                velocity.Y = -1;
            }

            if (inputProvider.KeyboardState.IsKeyDown(Keys.D))
            {
                velocity.X = 1;
            }

            if (inputProvider.KeyboardState.IsKeyDown(Keys.A))
            {
                velocity.X = -1;
            }

            Vector2 shootingDirection = new Vector2(inputProvider.GamePadState.ThumbSticks.Right.X, -inputProvider.GamePadState.ThumbSticks.Right.Y);

            if (!inputProvider.GamePadState.IsConnected || shootingDirection == Vector2.Zero)
            {
                Vector2 target = singlePlayer.Camera.GetWolrdPosFromScreenPos(inputProvider.MouseState.Position.ToVector2(), singlePlayer.GraphicsDevice) - Position;
                target.Normalize();
                shootingDirection = target;
            }
            else
            {
                direction = GetDirection(shootingDirection);
            }

            shootingDirection.Normalize();

            if (velocity != Vector2.Zero)
            {
                direction = GetDirection(shootingDirection);
            }

            if ((inputProvider.MouseState.LeftButton == ButtonState.Pressed || inputProvider.GamePadState.IsButtonDown(Buttons.RightShoulder)) && Ammo > 0 && Weapon.Shoot(shootingDirection))
            {
                Ammo--;
            }

            Body.ApplyForce(velocity * 50);

            animator.Update(gameTime);
            Weapon.Update(gameTime);

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, EffectParameter parameter)
        {
            if (Dead)
            {
                return;
            }

            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);

            if (Velocity.Length() < 0.1f)
            {
                animator.DrawAnimation("Idle", screenPos, spriteBatch, Color.White, parameter, CurrentBlinkFrame);
            }
            else
            {
                switch (direction)
                {
                    case Directions.NORTH:
                        animator.DrawAnimation("North", screenPos, spriteBatch, Color.White, parameter, CurrentBlinkFrame);
                        break;
                    case Directions.SOUTH:
                        animator.DrawAnimation("South", screenPos, spriteBatch, Color.White, parameter, CurrentBlinkFrame);
                        break;
                    case Directions.WEST:
                        animator.DrawAnimation("West", screenPos, spriteBatch, Color.White, parameter, CurrentBlinkFrame);
                        break;
                    case Directions.EAST:
                        animator.DrawAnimation("East", screenPos, spriteBatch, Color.White, parameter, CurrentBlinkFrame);
                        break;
                    case Directions.NORTHEAST:
                        animator.DrawAnimation("NorthEast", screenPos, spriteBatch, Color.White, parameter, CurrentBlinkFrame);
                        break;
                    case Directions.NORTHWEST:
                        animator.DrawAnimation("NorthWest", screenPos, spriteBatch, Color.White, parameter, CurrentBlinkFrame);
                        break;
                    case Directions.SOUTHEAST:
                        animator.DrawAnimation("SouthEast", screenPos, spriteBatch, Color.White, parameter, CurrentBlinkFrame);
                        break;
                    case Directions.SOUTHWEST:
                        animator.DrawAnimation("SouthWest", screenPos, spriteBatch, Color.White, parameter, CurrentBlinkFrame);
                        break;
                }
            }

            Weapon.Draw(spriteBatch, parameter);

            base.Draw(spriteBatch, parameter);
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