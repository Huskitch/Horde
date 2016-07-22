﻿// <copyright file="EntityBaseEnemy.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using HoardeGame.Extensions;
using HoardeGame.Level;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Base class for all enemies
    /// </summary>
    public class EntityBaseEnemy : EntityBase
    {
        /// <summary>
        /// Gets or sets the minimal amount of gems that will drop from this entity when killed
        /// </summary>
        public int[] MinGemDrop { get; protected set; } = new int[3];

        /// <summary>
        /// Gets or sets the maximum amount of gems that will drop from this entity when killed
        /// </summary>
        public int[] MaxGemDrop { get; protected set; } = new int[3];

        /// <summary>
        /// Gets or sets the current direction of this entity
        /// </summary>
        protected Vector2 Direction { get; set; }

        private readonly IResourceProvider resourceProvider;
        private readonly IPlayerProvider playerProvider;

        private float walkTimer;
        private Random rng = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBaseEnemy"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to load resources with</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> for accessing the player entity</param>
        public EntityBaseEnemy(DungeonLevel level, IResourceProvider resourceProvider, IPlayerProvider playerProvider) : base(level)
        {
            this.resourceProvider = resourceProvider;
            this.playerProvider = playerProvider;
        }

        /// <summary>
        /// Updates the AI state
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public void UpdateAI(GameTime gameTime)
        {
            walkTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Vector2.Distance(playerProvider.Player.Position, Position) > 5)
            {
                if (walkTimer > rng.Next(100, 3000))
                {
                    Direction = new Vector2(rng.Next(-1, 2), rng.Next(-1, 2));
                    walkTimer = 0;
                }
            }
            else
            {
                Direction = playerProvider.Player.Position - Position;
                Direction = Vector2.Normalize(Direction);
            }

            Body.ApplyForce(Direction * 20);
        }

        /// <inheritdoc/>
        public override void Start()
        {
            Body.OnCollision += OnShot;
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
            if (fixtureB.CollisionCategories == Category.Cat2)
            {
                Health -= playerProvider.Player.Weapon.Damage;
                Hit();

                if (Health <= 0)
                {
                    Removed = true;

                    resourceProvider.GetSoundEffect("Death").Play();
                    Random random = new Random();

                    int gemCount1 = random.Next(MinGemDrop[0], MaxGemDrop[0]);
                    int gemCount2 = random.Next(MinGemDrop[1], MaxGemDrop[1]);
                    int gemCount3 = random.Next(MinGemDrop[2], MaxGemDrop[2]);

                    for (int i = 0; i < gemCount1; i++)
                    {
                        EntityGem gem = new EntityGem(Level, resourceProvider, playerProvider)
                        {
                            Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                        };

                        Level.AddEntity(gem);
                    }

                    for (int i = 0; i < gemCount2; i++)
                    {
                        EntityGem2 gem2 = new EntityGem2(Level, resourceProvider, playerProvider)
                        {
                            Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                        };

                        Level.AddEntity(gem2);
                    }

                    for (int i = 0; i < gemCount3; i++)
                    {
                        EntityGem3 gem3 = new EntityGem3(Level, resourceProvider, playerProvider)
                        {
                            Body =
                            {
                                Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8)) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                            }
                        };

                        Level.AddEntity(gem3);
                    }
                }
            }
            else if (fixtureB.CollisionCategories == Category.Cat1 && !playerProvider.Player.IsHit())
            {
                playerProvider.Player.Hit();
                resourceProvider.GetSoundEffect("Hurt").Play();
                playerProvider.Player.Health--;

                if (playerProvider.Player.Health == 0)
                {
                    playerProvider.Player.Dead = true;
                    resourceProvider.GetSoundEffect("PlayerDeath").Play();
                }
            }

            return true;
        }
    }
}