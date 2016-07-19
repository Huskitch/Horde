// <copyright file="EntityPlayer.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Graphics.Rendering;
using HoardeGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Player entity
    /// </summary>
    public class EntityPlayer : EntityBase
    {
        /// <summary>
        /// Gets instance of <see cref="EntityPlayer"/>
        /// </summary>
        public static EntityPlayer Player { get; private set; }

        private AnimatedSprite animator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPlayer"/> class.
        /// </summary>
        /// <param name="world"><see cref="World"/> to place this entity in</param>
        public EntityPlayer(World world) : base(world)
        {
            Body = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(10), 1f, ConvertUnits.ToSimUnits(new Vector2(400, 400)));
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;

            animator = new AnimatedSprite(ResourceManager.Texture("PlayerSheet"));
            animator.AddAnimation("WalkLeft", 32, 0, 5, 150);

            Player = this;
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            Vector2 velocity = Vector2.Zero;

            if (InputManager.KeyboardState.IsKeyDown(Keys.S))
            {
                velocity.Y++;
            }

            if (InputManager.KeyboardState.IsKeyDown(Keys.W))
            {
                velocity.Y--;
            }

            if (InputManager.KeyboardState.IsKeyDown(Keys.D))
            {
                velocity.X++;
            }

            if (InputManager.KeyboardState.IsKeyDown(Keys.A))
            {
                velocity.X--;
            }

            Body.ApplyForce(velocity * 50);

            animator.Update(gameTime);

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);

            animator.DrawAnimation("WalkLeft", screenPos, spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}