// <copyright file="EntityBaseEnemy.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using HoardeGame.Level;
using Microsoft.Xna.Framework;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Base class for all enemies
    /// </summary>
    public class EntityBaseEnemy : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBaseEnemy"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        public EntityBaseEnemy(DungeonLevel level) : base(level)
        {
        }

        /// <inheritdoc/>
        public override void Start()
        {
            Body.OnCollision += OnShot;
        }

        private bool OnShot(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories != Category.Cat2)
            {
                return true;
            }

            Health--;

            if (Health <= 0)
            {
                Removed = true;
            }

            return true;
        }
    }
}