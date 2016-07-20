// <copyright file="EntityBullet.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using HoardeGame.Level;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Bullet entity
    /// </summary>
    public class EntityBullet : EntityBase
    {
        private readonly IResourceProvider resourceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBullet"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        /// <param name="startPos">Starting position</param>
        /// <param name="direction">Direction</param>
        public EntityBullet(DungeonLevel level, IResourceProvider resourceProvider, Vector2 startPos, Vector2 direction) : base(level)
        {
            this.resourceProvider = resourceProvider;

            FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(5), ConvertUnits.ToSimUnits(5), 1f, Vector2.Zero, Body);
            Body.Position = ConvertUnits.ToSimUnits(startPos);
            Body.CollisionCategories = Category.Cat2;
            Body.CollidesWith = Category.Cat3 | Category.Cat4;
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 0f;
            Body.ApplyForce(direction * 20);
            Body.OnCollision += OnShoot;
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);
            spriteBatch.Draw(resourceProvider.GetTexture("Bullet"), screenPos, Color.White);
        }

        /// <summary>
        /// Gets automatically called when the bullet collides with another entity
        /// Removes the bullet
        /// </summary>
        /// <param name="fixtureA">Fisrt <see cref="Fixture"/></param>
        /// <param name="fixtureB">Second <see cref="Fixture"/></param>
        /// <param name="contact"><see cref="Contact"/></param>
        /// <returns>true</returns>
        private bool OnShoot(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == Category.Cat4 || fixtureB.CollisionCategories == Category.Cat3)
            {
                Removed = true;
            }

            return true;
        }
    }
}