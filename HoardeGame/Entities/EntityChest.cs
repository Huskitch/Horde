// <copyright file="EntityChest.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
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

        private float interractLabelPos = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityChest"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to use for loading resources</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> to use for acessing the player entity</param>
        /// <param name="info"><see cref="ChestInfo"/> loot info</param>
        public EntityChest(DungeonLevel level, IResourceProvider resourceProvider, IPlayerProvider playerProvider, ChestInfo info) : base(level)
        {
            this.resourceProvider = resourceProvider;
            this.playerProvider = playerProvider;
            this.info = info;

            FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(25), ConvertUnits.ToSimUnits(25), 1f, Vector2.Zero, Body);
            Body.Position = level.GetSpawnPosition();
            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = Category.All;
            Body.BodyType = BodyType.Dynamic;
            Body.LinearDamping = 70f;
            Body.FixedRotation = true;

            Health = 3;
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, Effect effect)
        {
            spriteBatch.Draw(resourceProvider.GetTexture("Chest"), ScreenPosition, Color.White);

            if (Vector2.Distance(playerProvider.Player.Position, Position) < 3)
            {
                spriteBatch.Draw(resourceProvider.GetTexture("OneByOneEmpty"), new Rectangle((int)ScreenPosition.X + 11, (int)interractLabelPos, 10, 10), new Color(0, 0, 5, 0.3f));
                spriteBatch.DrawString(resourceProvider.GetFont("BasicFont"), "E", new Vector2((int)ScreenPosition.X, (int)interractLabelPos) + new Vector2(14, 0), Color.White, 0f, Vector2.Zero, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);
            }
        }
    }
}