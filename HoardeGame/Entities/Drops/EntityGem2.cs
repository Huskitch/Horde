﻿// <copyright file="EntityGem2.cs" company="Kuub Studios">
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
    /// Gem2 entity
    /// </summary>
    public class EntityGem2 : EntityBase
    {
        private readonly AnimatedSprite animator;
        private readonly IPlayerProvider playerProvider;
        private readonly IResourceProvider resourceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityGem2"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        public EntityGem2(DungeonLevel level, GameServiceContainer serviceContainer) : base(level, serviceContainer)
        {
            playerProvider = serviceContainer.GetService<IPlayerProvider>();
            resourceProvider = serviceContainer.GetService<IResourceProvider>();

            FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(10f), 1f, Body, ConvertUnits.ToSimUnits(-new Vector2(8, 8)));
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;
            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = Category.Cat1 | Category.Cat3 | Category.Cat4;

            Body.OnCollision += Collect;

            animator = new AnimatedSprite(resourceProvider.GetTexture("DiamondSheet"), serviceContainer);
            animator.AddAnimation("Bounce", 24, 0, 13, 150);
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
        public override void Draw(EffectParameter parameter)
        {
            animator.DrawAnimation("Bounce", ScreenPosition, Color.White);
            base.Draw(parameter);
        }

        private bool Collect(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body == playerProvider.Player.Body)
            {
                resourceProvider.GetSoundEffect("Gem").Play();
                playerProvider.Player.Gems.GreenGems++;
                Removed = true;

                return false;
            }

            return true;
        }
    }
}