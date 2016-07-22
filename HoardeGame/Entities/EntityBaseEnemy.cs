// <copyright file="EntityBaseEnemy.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
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
        public int MinGemDrop { get; protected set; } = 1;

        /// <summary>
        /// Gets or sets the maximum amount of gems that will drop from this entity when killed
        /// </summary>
        public int MaxGemDrop { get; protected set; } = 1;

        private readonly IResourceProvider resourceProvider;
        private readonly IPlayerProvider playerProvider;

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

                    int gemCount = random.Next(MinGemDrop, MaxGemDrop);

                    for (int i = 0; i < gemCount; i++)
                    {
                        EntityGem gem = new EntityGem(Level, resourceProvider, playerProvider)
                        {
                            Body =
                        {
                            Position = Position + ConvertUnits.ToSimUnits(new Vector2(8, 8))
                        }
                        };

                        Level.AddEntity(gem);
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