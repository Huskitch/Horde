// <copyright file="EntityBat.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using HoardeGame.Graphics.Rendering;
using HoardeGame.Level;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Bat entity
    /// </summary>
    public class EntityBat : EntityBase
    {
        private readonly AnimatedSprite animator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBat"/> class.
        /// </summary>
        /// <param name="level"><see cref="World"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to load resources with</param>
        public EntityBat(DungeonLevel level, IResourceProvider resourceProvider) : base(level)
        {
            Body = BodyFactory.CreateCircle(Level.World, ConvertUnits.ToSimUnits(10), 1f, ConvertUnits.ToSimUnits(new Vector2(500, 500)));
            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = Category.All;
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;
            Body.OnCollision += OnShot;

            Health = 3;

            animator = new AnimatedSprite(resourceProvider.GetTexture("BatSheet"));
            animator.AddAnimation("Flap", 32, 0, 2, 100);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            animator.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);
            animator.DrawAnimation("Flap", screenPos, spriteBatch);
        }

        private bool OnShot(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories != Category.Cat2)
            {
                return true;
            }

            if (Health < 0)
            {
                return false;
            }

            Health--;

            if (Health == 0)
            {
                Removed = true;
            }

            return true;
        }
    }
}