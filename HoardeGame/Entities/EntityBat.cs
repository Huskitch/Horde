﻿// <copyright file="EntityBat.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Gameplay;
using HoardeGame.Graphics.Rendering;
using HoardeGame.Level;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Bat entity
    /// </summary>
    public class EntityBat : EntityBaseEnemy
    {
        private readonly AnimatedSprite animator;
        private IPlayerProvider playerProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBat"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to load resources with</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> for acessing the player entity</param>
        public EntityBat(DungeonLevel level, IResourceProvider resourceProvider, IPlayerProvider playerProvider) : base(level, resourceProvider, playerProvider)
        {
            FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(10f), 1f, Body);
            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = Category.All;
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 20f;
            Body.FixedRotation = true;

            this.playerProvider = playerProvider;

            Damage = 1;
            Health = 5;

            GemDrop = new GemDropInfo
            {
                MinDrop =
                {
                    RedGems = 3,
                    GreenGems = 2,
                    BlueGems = 1,
                    Keys = 0
                },
                MaxDrop =
                {
                    RedGems = 6,
                    GreenGems = 4,
                    BlueGems = 2,
                    Keys = 0
                }
            };

            animator = new AnimatedSprite(resourceProvider.GetTexture("BatSheet"));
            animator.AddAnimation("Flap", 32, 0, 2, 100);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            UpdateAI(gameTime);
            animator.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, EffectParameter parameter)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);
            animator.DrawAnimation("Flap", screenPos, spriteBatch, Color.White, parameter, CurrentBlinkFrame);
        }
    }
}