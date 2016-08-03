// <copyright file="EntityArmour.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using HoardeGame.Entities.Base;
using HoardeGame.Entities.Player;
using HoardeGame.Gameplay.Level;
using HoardeGame.Gameplay.Player;
using HoardeGame.Graphics;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities.Drops
{
    /// <summary>
    /// Armour drop entity
    /// </summary>
    public class EntityArmour : EntityBase
    {
        private readonly AnimatedSprite animator;
        private readonly IPlayerProvider playerProvider;
        private readonly IResourceProvider resourceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityArmour"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        public EntityArmour(DungeonLevel level, GameServiceContainer serviceContainer) : base(level, serviceContainer)
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
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (Vector2.Distance(Position, playerProvider.Player.Position) < 1.5f && playerProvider.Player.Armour < EntityPlayer.MaxArmour)
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
            SpriteBatch.Draw(resourceProvider.GetTexture("Armour"), ScreenPosition, Color.White);
            base.Draw(parameter);
        }

        private bool Collect(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body == playerProvider.Player.Body)
            {
                if (playerProvider.Player.Armour >= EntityPlayer.MaxArmour)
                {
                    return false;
                }

                resourceProvider.GetSoundEffect("Gem").Play();
                playerProvider.Player.Armour++;
                Removed = true;

                return false;
            }

            return true;
        }
    }
}