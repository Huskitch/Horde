﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class EntityKey : EntityBase
    {
        private readonly AnimatedSprite animator;
        private readonly IPlayerProvider playerProvider;
        private readonly IResourceProvider resourceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="HoardeGame.Entities.EntityGem"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> for accessing the player entity</param>
        public EntityKey(DungeonLevel level, IResourceProvider resourceProvider, IPlayerProvider playerProvider) : base(level)
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
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (Vector2.Distance(Position, playerProvider.Player.Position) < 2)
            {
                Vector2 direction = playerProvider.Player.Position - Position;
                direction.Normalize();

                Body.ApplyForce(direction * 100f);
            }

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, Effect effect)
        {
            spriteBatch.Draw(resourceProvider.GetTexture("EntityKey"), ScreenPosition, Color.White);
            base.Draw(spriteBatch, effect);
        }

        private bool Collect(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body == playerProvider.Player.Body)
            {
                resourceProvider.GetSoundEffect("Gem").Play();
                playerProvider.Player.Gems[0]++;
                Removed = true;

                return false;
            }

            return true;
        }
    }
}