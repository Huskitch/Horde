// <copyright file="EntityGem.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
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
    /// Gem entity
    /// </summary>
    public class EntityGem : EntityBase
    {
        private readonly AnimatedSprite animator;
        private readonly IPlayerProvider playerProvider;
        private readonly IResourceProvider resourceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityGem"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> for accessing the player entity</param>
        public EntityGem(DungeonLevel level, IResourceProvider resourceProvider, IPlayerProvider playerProvider) : base(level)
        {
            this.playerProvider = playerProvider;
            this.resourceProvider = resourceProvider;

            FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(10f), 1f, Body, ConvertUnits.ToSimUnits(-new Vector2(8, 8)));
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;
            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = Category.All;

            Body.OnCollision += Collect;

            animator = new AnimatedSprite(resourceProvider.GetTexture("GemAnimation"));
            animator.AddAnimation("Bounce", 16, 0, 6, 150);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            animator.Update(gameTime);

            if (Vector2.Distance(Position, playerProvider.Player.Position) < 2)
            {
                Body.ApplyForce((playerProvider.Player.Position - Position) * 20f);
            }

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, Effect effect)
        {
            animator.DrawAnimation("Bounce", ScreenPosition, spriteBatch);
            base.Draw(spriteBatch, effect);
        }

        private bool Collect(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body == playerProvider.Player.Body)
            {
                resourceProvider.GetSoundEffect("Gem").Play();
                playerProvider.Player.Gems++;
                Removed = true;
            }

            return true;
        }
    }
}