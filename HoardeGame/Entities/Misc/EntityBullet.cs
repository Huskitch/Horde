// <copyright file="EntityBullet.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using HoardeGame.Entities.Base;
using HoardeGame.Gameplay.Level;
using HoardeGame.Gameplay.Weapons;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities.Misc
{
    /// <summary>
    /// Bullet entity
    /// </summary>
    public class EntityBullet : EntityBase
    {
        private readonly IResourceProvider resourceProvider;
        private readonly float rotation;
        private readonly Vector2 direction;
        private readonly float speed;
        private readonly BulletOwnershipInfo info;
        private readonly float lifetime;
        private Vector2 startPos;
        private float totalDistance;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBullet"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        /// <param name="startPos">Starting position</param>
        /// <param name="direction">Direction</param>
        /// <param name="speed">Speed of the bullet</param>
        /// <param name="lifetime">Life of the bullet in ms</param>
        /// <param name="info"><see cref="BulletOwnershipInfo"/></param>
        public EntityBullet(DungeonLevel level, IResourceProvider resourceProvider, Vector2 startPos, Vector2 direction, float speed, float lifetime, BulletOwnershipInfo info) : base(level)
        {
            this.resourceProvider = resourceProvider;
            this.direction = direction;
            this.info = info;
            this.speed = speed;
            this.lifetime = lifetime;

            FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(5), ConvertUnits.ToSimUnits(5), 1f, Vector2.Zero, Body);
            Body.Position = ConvertUnits.ToSimUnits(startPos);
            Body.CollisionCategories = Category.Cat2;
            Body.CollidesWith = Category.Cat3 | Category.Cat4;
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 0f;
            Body.ApplyForce(direction * speed);
            Body.OnCollision += OnShoot;
            Body.UserData = info;

            this.startPos = ConvertUnits.ToSimUnits(startPos);

            if (info.Faction == Faction.Enemies)
            {
                Body.CollidesWith = Category.Cat1 | Category.Cat4;
            }

            rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            totalDistance += Vector2.Distance(startPos, Position);

            if (totalDistance > lifetime)
            {
                Removed = true;
            }

            startPos = Position;
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, EffectParameter parameter)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);

            spriteBatch.Draw(resourceProvider.GetTexture("Bullet"), new Rectangle((int)screenPos.X + 16, (int)screenPos.Y + 16, 32, 32), null, Color.White, rotation, new Vector2(16, 16), SpriteEffects.None, 0f);
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
            if (info.Faction == Faction.Player && (fixtureB.CollisionCategories == Category.Cat4 || fixtureB.CollisionCategories == Category.Cat3))
            {
                Removed = true;
            }
            else if (info.Faction == Faction.Enemies && (fixtureB.CollisionCategories == Category.Cat4 || fixtureB.CollisionCategories == Category.Cat1))
            {
                Removed = true;
            }

            if (info.Faction == Faction.Player && fixtureB.CollisionCategories == Category.Cat3)
            {
                fixtureB.Body.ApplyForce(direction * 250);
            }
            else if (info.Faction == Faction.Enemies && fixtureB.CollisionCategories == Category.Cat1)
            {
                fixtureB.Body.ApplyForce(direction * 250);
            }

            return true;
        }
    }
}