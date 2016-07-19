// <copyright file="EntityChest.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Chest entity
    /// </summary>
    public class EntityChest : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityChest"/> class.
        /// </summary>
        /// <param name="world"><see cref="World"/> to place this entity in</param>
        public EntityChest(World world) : base(world)
        {
            Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(25), ConvertUnits.ToSimUnits(25), 1f, ConvertUnits.ToSimUnits(new Vector2(450, 500)));

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
            spriteBatch.Draw(ResourceManager.GetTexture("Chest"), screenPos, Color.White);
        }
    }
}