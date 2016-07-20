// <copyright file="EntityChest.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Level;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Chest entity
    /// </summary>
    public class EntityChest : EntityBase
    {
        private readonly IResourceProvider resourceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityChest"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to use for loading resources</param>
        public EntityChest(DungeonLevel level, IResourceProvider resourceProvider) : base(level)
        {
            this.resourceProvider = resourceProvider;

            Body = BodyFactory.CreateRectangle(Level.World, ConvertUnits.ToSimUnits(25), ConvertUnits.ToSimUnits(25), 1f, ConvertUnits.ToSimUnits(new Vector2(450, 500)));

            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = Category.All;
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 70f;
            Body.FixedRotation = true;

            Health = 3;
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);
            spriteBatch.Draw(resourceProvider.GetTexture("Chest"), screenPos, Color.White);
        }
    }
}