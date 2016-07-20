// <copyright file="EntityBullet.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
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
        /// <param name="world"><see cref="World"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        /// <param name="startPos">Starting position</param>
        /// <param name="direction">Direction</param>
        public EntityBullet(World world, IResourceProvider resourceProvider, Vector2 startPos, Vector2 direction) : base(world)
        {
            this.resourceProvider = resourceProvider;

            Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(5), ConvertUnits.ToSimUnits(5), 1f, ConvertUnits.ToSimUnits(startPos));

            Body.CollisionCategories = Category.Cat2;
            Body.CollidesWith = Category.Cat3 | Category.Cat4;

            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 0f;
            Body.ApplyForce(direction * 10);
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
    }
}