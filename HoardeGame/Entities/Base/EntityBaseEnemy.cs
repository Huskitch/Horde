﻿// <copyright file="EntityBaseEnemy.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using HoardeGame.Entities.Drops;
using HoardeGame.Entities.Misc;
using HoardeGame.Extensions;
using HoardeGame.Gameplay.Gems;
using HoardeGame.Gameplay.Level;
using HoardeGame.Gameplay.Player;
using HoardeGame.Gameplay.Weapons;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;

namespace HoardeGame.Entities.Base
{
    /// <summary>
    /// Base class for all enemies
    /// </summary>
    public class EntityBaseEnemy : EntityBase
    {
        /// <summary>
        /// Gets or sets the amount of gems that will drop from this entity when killed
        /// </summary>
        public GemDropInfo GemDrop { get; protected set; } = new GemDropInfo();

        /// <summary>
        /// Gets or sets the current direction of this entity
        /// </summary>
        protected Vector2 Direction { get; set; }

        /// <summary>
        /// Gets the <see cref="IPlayerProvider"/>
        /// </summary>
        protected IPlayerProvider PlayerProvider { get; }

        private readonly IResourceProvider resourceProvider;
        private readonly GameServiceContainer serviceContainer;
        private readonly Random rng = new Random(Guid.NewGuid().GetHashCode());

        private float walkTimer;

        /// <summary>
        /// Gets or sets the damage dealt by this entity
        /// </summary>
        protected int Damage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBaseEnemy"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        protected EntityBaseEnemy(DungeonLevel level, GameServiceContainer serviceContainer) : base(level, serviceContainer)
        {
            this.serviceContainer = serviceContainer;

            resourceProvider = serviceContainer.GetService<IResourceProvider>();
            PlayerProvider = serviceContainer.GetService<IPlayerProvider>();
        }

        /// <summary>
        /// Updates the AI state
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public virtual void UpdateAI(GameTime gameTime)
        {
            walkTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Vector2.Distance(PlayerProvider.Player.Position, Position) > 5 || PlayerProvider.Player.Dead)
            {
                if (walkTimer > rng.Next(100, 3000))
                {
                    Direction = new Vector2(rng.Next(-1, 2), rng.Next(-1, 2));
                    walkTimer = 0;
                }
            }
            else
            {
                Direction = PlayerProvider.Player.Position - Position;
                Direction = Vector2.Normalize(Direction);
            }

            Body.ApplyForce(Direction * 20);
        }

        /// <inheritdoc/>
        public override void Start()
        {
            Body.FixtureList[0].AfterCollision += OnShot;
        }

        /// <summary>
        /// Gets automatically called when the entity collides with another entity
        /// Filters out bullets and does damage
        /// </summary>
        /// <param name="fixtureA">Fisrt <see cref="Fixture"/></param>
        /// <param name="fixtureB">Second <see cref="Fixture"/></param>
        /// <param name="contact"><see cref="Contact"/></param>
        /// <param name="impulse"><see cref="ContactVelocityConstraint"/></param>
        private void OnShot(Fixture fixtureA, Fixture fixtureB, Contact contact, ContactVelocityConstraint impulse)
        {
            if (fixtureB.CollisionCategories == Category.Cat2 && ((BulletOwnershipInfo)fixtureB.Body.UserData).Faction == Faction.Player)
            {
                EntityFlyingDamageIndicator flyingDamageIndicator = new EntityFlyingDamageIndicator(Level, serviceContainer)
                {
                    Color = Color.White,
                    Damage = PlayerProvider.Player.Weapon.CurrentAmmoType.Damage,
                    LifeTime = 60,
                    Body =
                    {
                        Position = Position + new Vector2((float)rng.NextDouble(), (float)rng.NextDouble())
                    },
                    Velocity = -new Vector2(-0.01f, 0.01f)
                };

                Level.AddEntity(flyingDamageIndicator);

                Health -= PlayerProvider.Player.Weapon.CurrentAmmoType.Damage;

                if (!IsHit())
                {
                    Hit();
                }

                if (Health <= 0)
                {
                    Removed = true;

                    resourceProvider.GetSoundEffect("Death").Play();
                    Random random = new Random();

                    int gemCount1 = GemDrop.GetRandomRedGems();
                    int gemCount2 = GemDrop.GetRandomGreenGems();
                    int gemCount3 = GemDrop.GetRandomBlueGems();
                    int keyCount = GemDrop.GetRandomKeys();

                    for (int i = 0; i < gemCount1; i++)
                    {
                        EntityGem gem = new EntityGem(Level, serviceContainer)
                        {
                            Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.NextVector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                        };

                        Level.AddEntity(gem);
                    }

                    for (int i = 0; i < gemCount2; i++)
                    {
                        EntityGem2 gem2 = new EntityGem2(Level, serviceContainer)
                        {
                            Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.NextVector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                        };

                        Level.AddEntity(gem2);
                    }

                    for (int i = 0; i < gemCount3; i++)
                    {
                        EntityGem3 gem3 = new EntityGem3(Level, serviceContainer)
                        {
                            Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.NextVector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                        };

                        Level.AddEntity(gem3);
                    }

                    for (int i = 0; i < keyCount; i++)
                    {
                        EntityKey key = new EntityKey(Level, serviceContainer)
                        {
                            Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.NextVector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                        };

                        Level.AddEntity(key);
                    }
                }
            }
            else if (fixtureB.CollisionCategories == Category.Cat1 && !PlayerProvider.Player.IsHit() && Damage > 0)
            {
                PlayerProvider.Player.Damage(Damage);
            }
        }
    }
}