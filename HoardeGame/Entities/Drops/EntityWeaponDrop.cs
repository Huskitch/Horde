// <copyright file="EntityWeaponDrop.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using HoardeGame.Entities.Base;
using HoardeGame.Gameplay.Level;
using HoardeGame.Gameplay.Player;
using HoardeGame.Gameplay.Weapons;
using HoardeGame.Graphics;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities.Drops
{
    /// <summary>
    /// Weapon frop entity
    /// </summary>
    public class EntityWeaponDrop : EntityBase
    {
        /// <summary>
        /// Gets or sets the <see cref="WeaponInfo"/>
        /// </summary>
        public WeaponInfo Weapon { get; set; }

        private readonly AnimatedSprite animator;
        private readonly IPlayerProvider playerProvider;
        private readonly IResourceProvider resourceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityWeaponDrop"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        public EntityWeaponDrop(DungeonLevel level, GameServiceContainer serviceContainer) : base(level, serviceContainer)
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
            if (Vector2.Distance(Position, playerProvider.Player.Position) < 2)
            {
                // TODO: Show popup
            }

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(EffectParameter parameter)
        {
            SpriteBatch.Draw(resourceProvider.GetTexture(Weapon.Texture), ScreenPosition, Color.White);
            base.Draw(parameter);
        }

        private bool Collect(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            // TODO: Implement weapon drop collection
            return true;
        }
    }
}