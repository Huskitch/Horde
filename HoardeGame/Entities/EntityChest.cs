// <copyright file="EntityChest.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Level;
using HoardeGame.Resources;
using HoardeGame.Tweening;
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
        private readonly Tweener tween = new Tweener();
        private readonly IPlayerProvider playerProvider;
        private float interractLabelPos = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityChest"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to use for loading resources</param>
        /// <param name="playerProvider"><see cref="IPlayerProvider"/> to use for acessing the player entity</param>
        public EntityChest(DungeonLevel level, IResourceProvider resourceProvider, IPlayerProvider playerProvider) : base(level)
        {
            this.resourceProvider = resourceProvider;
            this.playerProvider = playerProvider;

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
        public override void Update(GameTime gameTime)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);
            tween.Update(gameTime.ElapsedGameTime.Milliseconds / 1000f);

            if (Vector2.Distance(playerProvider.Player.Position, Position) < 3 && !playerProvider.Player.Dead)
            {
                tween.Tween(this, new { interractLabelPos = screenPos.Y - 10 }, 1f).Ease(Ease.LinearIn);
            }

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, Effect effect)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position);
            spriteBatch.Draw(resourceProvider.GetTexture("Chest"), screenPos, Color.White);
            spriteBatch.Draw(resourceProvider.GetTexture("OneByOneEmpty"), new Rectangle((int)screenPos.X + 11, (int)interractLabelPos, 10, 10), new Color(0, 0, 5, 0.3f));
            spriteBatch.DrawString(resourceProvider.GetFont("BasicFont"), "E", screenPos + new Vector2(14, -12), Color.White, 0f, Vector2.Zero, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);
        }
    }
}