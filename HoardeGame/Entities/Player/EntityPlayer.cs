﻿// <copyright file="EntityPlayer.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using HoardeGame.Entities.Base;
using HoardeGame.Entities.Misc;
using HoardeGame.Extensions;
using HoardeGame.Gameplay.Consumeables;
using HoardeGame.Gameplay.Gems;
using HoardeGame.Gameplay.Level;
using HoardeGame.Gameplay.Player;
using HoardeGame.Gameplay.Weapons;
using HoardeGame.GameStates;
using HoardeGame.Graphics;
using HoardeGame.Graphics.ParticleSystem;
using HoardeGame.Input;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.Entities.Player
{
    /// <summary>
    /// Player entity
    /// </summary>
    public class EntityPlayer : EntityBase, IPlayer
    {
        /// <summary>
        /// Count of inventory slots to be occupied by weapons
        /// BUT I LIKED MY DPAD SELECTION
        /// NEVER FORGET
        /// NEVER FORGIVE
        /// WE ARE KUUBNONYMOUS
        /// </summary>
        public const int MaxWeaponSlots = 2;

        /// <summary>
        /// Count of inventory slots to be occupied by items
        /// </summary>
        public const int MaxItemSlots = 2;

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
        public int Armour { get; set; }

        /// <inheritdoc/>
        public EntityWeapon Weapon { get; set; }

        /// <inheritdoc/>
        public EntityWeapon[] InventoryWeapons { get; set; }

        /// <inheritdoc/>
        public Consumeable[] InventoryItems { get; set; }

        /// <inheritdoc/>
        public AudioListener Listener { get; } = new AudioListener();

        private readonly IResourceProvider resourceProvider;
        private readonly SinglePlayer singlePlayer;
        private readonly GameServiceContainer serviceContainer;
        private readonly Random rng = new Random();

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
        private WeaponInfo currentWeapon;
        private GraphicsDevice graphicsDevice;
        private ParticleSystem particleSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPlayer"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> in which the player will spawn</param>
        /// <param name="singlePlayer"><see cref="SinglePlayer"/></param>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        public EntityPlayer(DungeonLevel level, SinglePlayer singlePlayer, GameServiceContainer serviceContainer) : base(level, serviceContainer)
        {
            this.serviceContainer = serviceContainer;
            this.singlePlayer = singlePlayer;

            inputProvider = serviceContainer.GetService<IInputProvider>();
            resourceProvider = serviceContainer.GetService<IResourceProvider>();
            weaponProvider = serviceContainer.GetService<IWeaponProvider>();
            graphicsDevice = serviceContainer.GetService<IGraphicsDeviceService>().GraphicsDevice;

            particleSystem = new ParticleSystem(this, serviceContainer)
            {
                Offset = new Vector2(16, 32)
            };

            FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(10), 1f, Body);
            Body.Position = Level.GetSpawnPosition();
            Body.CollisionCategories = Category.Cat1;
            Body.CollidesWith = Category.Cat3 | Category.Cat4 | Category.Cat2;

            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;
            Body.FixtureList[0].UserData = "player";
            Body.OnCollision += OnShot;

            Health = MaxHealth;
            Armour = MaxArmour;
            BlinkMultiplier = 1;

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

            InventoryWeapons = new EntityWeapon[MaxWeaponSlots];
            InventoryItems = new Consumeable[MaxItemSlots];

            currentWeapon = weaponProvider.GetWeapon("testWeapon");
            Weapon = new EntityWeapon(level, serviceContainer, this, currentWeapon)
            {
                Ammo = 100
            };

            InventoryWeapons[0] = Weapon;
        }

        /// <inheritdoc/>
        public void AddWeapon(EntityWeapon weapon)
        {
            InventoryWeapons[1] = weapon;
        }

        /// <inheritdoc/>
        public bool AddItem(Consumeable consumeable)
        {
            for (int i = 0; i < MaxItemSlots; i++)
            {
                if (InventoryItems[i] == null)
                {
                    InventoryItems[i] = consumeable;
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (Dead)
            {
                Health = 0;
                return;
            }

            particleSystem.Update(gameTime);
            Listener.Position = new Vector3(Position, 0);

            if (inputProvider.KeybindPressed("Weapon1"))
            {
                if (InventoryWeapons[0] != null)
                {
                    Weapon = InventoryWeapons[0];
                }
            }

            if (inputProvider.KeybindPressed("Weapon2"))
            {
                if (InventoryWeapons[1] != null)
                {
                    Weapon = InventoryWeapons[1];
                }
            }

            if (inputProvider.KeybindPressed("Item1"))
            {
                InventoryItems[0]?.OnUse?.Invoke(this);
                if (InventoryItems[0] != null)
                {
                    resourceProvider.GetSoundEffect("Consume").Play();
                }

                InventoryItems[0] = null;
            }

            if (inputProvider.KeybindPressed("Item2"))
            {
                InventoryItems[1]?.OnUse?.Invoke(this);
                if (InventoryItems[1] != null)
                {
                    resourceProvider.GetSoundEffect("Consume").Play();
                }

                InventoryItems[1] = null;
            }

            if (inputProvider.KeybindPressed("SwitchWeapon"))
            {
                if (Weapon == InventoryWeapons[0] && InventoryWeapons[1] != null)
                {
                    Weapon = InventoryWeapons[1];
                }

                if (Weapon == InventoryWeapons[1] && InventoryWeapons[0] != null)
                {
                    Weapon = InventoryWeapons[0];
                }
            }

            if (inputProvider.MouseState.RightButton == ButtonState.Pressed && !singlePlayer.Shopping && !singlePlayer.Drilling)
            {
                singlePlayer.Camera.Zoom = MathHelper.Lerp(3.5f, 3f, 0.001f);
                Weapon.ShowLaser = true;
            }
            else
            {
                singlePlayer.Camera.Zoom = MathHelper.Lerp(3f, 3.5f, 0.001f);
                Weapon.ShowLaser = false;
            }

            if (singlePlayer.Shopping || singlePlayer.Drilling)
            {
                animator.Update(gameTime);
                Weapon.Update(gameTime);

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

            ShootingDirection = new Vector2(inputProvider.GamePadState.ThumbSticks.Right.X, -inputProvider.GamePadState.ThumbSticks.Right.Y);

            if (!inputProvider.GamePadState.IsConnected || ShootingDirection == Vector2.Zero)
            {
                Vector2 target = singlePlayer.Camera.GetWolrdPosFromScreenPos(inputProvider.MouseState.Position.ToVector2(), graphicsDevice) - Position;
                target.Normalize();

                ShootingDirection = target;
            }
            else
            {
                direction = GetDirection(ShootingDirection);
            }

            ShootingDirection.Normalize();

            if (velocity != Vector2.Zero)
            {
                particleSystem.AddParticle(new Particle()
                {
                    Color = Color.SaddleBrown,
                    Size = rng.NextFloat(1.5f, 2f),
                    Lifetime = 300,
                    Velocity = velocity * -1 + rng.NextVector2(0, 0, 0.5f, 0.5f),
                    Position = rng.NextVector2(-2, -2, 2, 2) + new Vector2(0, -4)
                });

                direction = GetDirection(ShootingDirection);
            }

            if ((inputProvider.MouseState.LeftButton == ButtonState.Pressed || inputProvider.GamePadState.IsButtonDown(Buttons.RightShoulder)) && Weapon.Ammo > 0)
            {
                float offset = MathHelper.ToRadians(currentWeapon.Bullets[0].Offset) +
                               MathHelper.ToRadians(rng.Next(0, currentWeapon.Bullets[0].Spread));

                float targetRad = (float)Math.Atan2(ShootingDirection.Y, ShootingDirection.X) + offset;

                Vector2 realShootingDirection = new Vector2((float)Math.Cos(targetRad), (float)Math.Sin(targetRad));

                if (Weapon.Shoot(realShootingDirection))
                {
                    Weapon.Ammo--;
                }
            }

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

            particleSystem.Draw(parameter);

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

        /// <inheritdoc/>
        public void Damage(int damage)
        {
            EntityFlyingDamageIndicator flyingDamageIndicator = new EntityFlyingDamageIndicator(Level, serviceContainer)
            {
                Color = Color.Red,
                Damage = damage,
                LifeTime = 60,
                Body =
                    {
                        Position = Position + new Vector2((float)rng.NextDouble(), (float)rng.NextDouble())
                    },
                Velocity = -new Vector2(-0.01f, 0.01f)
            };

            Level.AddEntity(flyingDamageIndicator);

            int particles = rng.Next(10);
            for (int i = 0; i < particles; i++)
            {
                particleSystem.AddParticle(new Particle()
                {
                    Velocity = rng.NextVector2(-1, -1, 1, 1),
                    Color = Color.Red,
                    Lifetime = 300,
                    Size = rng.NextFloat(1, 2),
                });
            }

            resourceProvider.GetSoundEffect("Hurt").Play();

            if (Armour > 0)
            {
                int remainingDamage = damage - Armour;
                Armour -= damage;

                if (remainingDamage > 0)
                {
                    Health -= remainingDamage;
                }

                if (Armour == 0)
                {
                    resourceProvider.GetSoundEffect("ArmourGone").Play();
                }
            }
            else
            {
                Health -= damage;
            }

            if (Armour < 0)
            {
                Armour = 0;
            }

            if (Health <= 0)
            {
                Health = 0;
                Dead = true;
                Body.CollidesWith = Category.None;
                resourceProvider.GetSoundEffect("PlayerDeath").Play();
            }

            if (!IsHit())
            {
                Hit();
            }
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

        /// <summary>
        /// Gets automatically called when the entity collides with another entity
        /// Filters out bullets and does damage
        /// </summary>
        /// <param name="fixtureA">Fisrt <see cref="Fixture"/></param>
        /// <param name="fixtureB">Second <see cref="Fixture"/></param>
        /// <param name="contact"><see cref="Contact"/></param>
        /// <returns>true</returns>
        private bool OnShot(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            Random rng = new Random();

            if (fixtureB.CollisionCategories == Category.Cat2 && !IsHit() && ((BulletOwnershipInfo)fixtureB.Body.UserData).Faction == Faction.Enemies)
            {
                Damage(((BulletOwnershipInfo)fixtureB.Body.UserData).Weapon.CurrentAmmoType.Damage);
            }

            return true;
        }
    }
}