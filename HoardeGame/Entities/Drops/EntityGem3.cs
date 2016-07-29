// <copyright file="EntityGem3.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using HoardeGame.Entities.Base;
using HoardeGame.Gameplay.Level;
using HoardeGame.Gameplay.Player;
using HoardeGame.Graphics;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities.Drops
{
    /// <summary>
    /// Gem3 entity
    /// </summary>
    public class EntityGem3 : EntityBase
    {
        private readonly AnimatedSprite animator;
        private readonly IPlayerProvider playerProvider;
        private readonly IResourceProvider resourceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityGem3"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> for accessing the player entity</param>
        public EntityGem3(DungeonLevel level, IResourceProvider resourceProvider, IPlayerProvider playerProvider) : base(level)
        {
            this.playerProvider = playerProvider;
            this.resourceProvider = resourceProvider;

            FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(10f), 1f, Body, ConvertUnits.ToSimUnits(-new Vector2(8, 8)));
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;
            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = Category.Cat1 | Category.Cat3 | Category.Cat4;

            Body.OnCollision += Collect;

            animator = new AnimatedSprite(resourceProvider.GetTexture("EmeraldSheet"));
            animator.AddAnimation("Bounce", 16, 0, 6, 150);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            animator.Update(gameTime);

            if (Vector2.Distance(Position, playerProvider.Player.Position) < 2)
            {
                Vector2 direction = playerProvider.Player.Position - Position;
                direction.Normalize();

                Body.ApplyForce(direction * 100f);
            }

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, EffectParameter parameter)
        {
            animator.DrawAnimation("Bounce", ScreenPosition, spriteBatch, Color.White);
            base.Draw(spriteBatch, parameter);
        }

        private bool Collect(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body == playerProvider.Player.Body)
            {
                resourceProvider.GetSoundEffect("Gem").Play();
                playerProvider.Player.Gems.BlueGems++;
                Removed = true;

                return false;
            }

            return true;
        }
    }
}