// <copyright file="EntityChest.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Graphics.Rendering;
using HoardeGame.Level;
using HoardeGame.Resources;
using HoardeGame.Themes;
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
        private readonly IPlayerProvider playerProvider;
        private readonly ChestInfo info;
        private readonly List<string> contentsList = new List<string>();
        private readonly AnimatedSprite animator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityChest"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to use for loading resources</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> to use for acessing the player entity</param>
        /// <param name="info"><see cref="ChestInfo"/> loot info</param>
        public EntityChest(DungeonLevel level, IResourceProvider resourceProvider, IPlayerProvider playerProvider, ChestInfo info) : base(level)
        {
            if (info == null)
            {
                throw new ArgumentException("Chest info cannot be null!", nameof(info));
            }

            this.resourceProvider = resourceProvider;
            this.playerProvider = playerProvider;
            this.info = info;

            FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(25), ConvertUnits.ToSimUnits(25), 1f, Vector2.Zero, Body);
            Body.Position = level.GetSpawnPosition(2);
            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = Category.All;
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 70f;
            Body.FixedRotation = true;

            animator = new AnimatedSprite(resourceProvider.GetTexture("ChestSheet"));
            animator.AddAnimation("Open", 32, 0, 2, 500);

            Health = 3;

            if (info.MaxGems[0] > 0)
            {
                contentsList.Add("Red gems");
            }

            if (info.MaxGems[1] > 0)
            {
                contentsList.Add("Green gems");
            }

            if (info.MaxGems[2] > 0)
            {
                contentsList.Add("Blue gems");
            }

            if (info.MaxAmmo > 0)
            {
                contentsList.Add("Ammo");
            }

            if (info.MaxArmour > 0)
            {
                contentsList.Add("Armour");
            }

            if (info.MaxHealth > 0)
            {
                contentsList.Add("Health");
            }

            if (info.WeaponDropType != ChestInfo.WeaponChance.None && info.WeaponDropChance > 0)
            {
                contentsList.Add("A weapon");
            }
        }

        public override void Update(GameTime gameTime)
        {
            animator.Update(gameTime);
            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, EffectParameter parameter)
        {
            int positionOnTheYAxisOfTheCoordinateSystem = contentsList.Count * 10 + 20;

            if (Vector2.Distance(playerProvider.Player.Position, Position) < 3)
            {
                animator.DrawAnimation("Open", ScreenPosition, spriteBatch, Color.White);
                spriteBatch.Draw(resourceProvider.GetTexture("OneByOneEmpty"), new Rectangle((int)ScreenPosition.X - 14, (int)ScreenPosition.Y - positionOnTheYAxisOfTheCoordinateSystem, 60, positionOnTheYAxisOfTheCoordinateSystem), new Color(0, 0, 5, 0.3f));
                spriteBatch.DrawString(resourceProvider.GetFont("BigFont"), "Contents", new Vector2((int)ScreenPosition.X - 8, (int)ScreenPosition.Y - positionOnTheYAxisOfTheCoordinateSystem), Color.White, 0f, Vector2.Zero, new Vector2(0.35f, 0.35f), SpriteEffects.None, 0f);

                for (int i = 0; i < contentsList.Count; i++)
                {
                    spriteBatch.DrawString(resourceProvider.GetFont("BigFont"), contentsList[i], new Vector2((int)ScreenPosition.X - 8, (int)ScreenPosition.Y - 75 + i * 10), Color.White, 0f, Vector2.Zero, new Vector2(0.3f, 0.3f), SpriteEffects.None, 0f);
                }
            }
            else
            {
                spriteBatch.Draw(resourceProvider.GetTexture("Chest"), ScreenPosition, Color.White);
            }
        }
    }
}