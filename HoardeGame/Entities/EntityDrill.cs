// <copyright file="EntityDrill.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.GameStates;
using HoardeGame.Input;
using HoardeGame.Level;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities
{
    /// <summary>
    /// Drill entity
    /// </summary>
    public class EntityDrill : EntityBase
    {
        private readonly IInputProvider inputProvider;
        private readonly IResourceProvider resourceProvider;
        private readonly SinglePlayer sp;

        private int Shield;

        // This probably isn't the best solution since you can't swap out sp for an interface but having both sp and playerProvider seems useless

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDrill"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="inputProvider"><see cref="IInputProvider"/> to use for input</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> used for loading resources</param>
        /// <param name="sp"><see cref="SinglePlayer"/> gamestate</param>
        public EntityDrill(DungeonLevel level, IInputProvider inputProvider, IResourceProvider resourceProvider, SinglePlayer sp) : base(level)
        {
            this.resourceProvider = resourceProvider;
            this.inputProvider = inputProvider;
            this.sp = sp;

            Health = 100;
            Shield = 100;

            FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(96), ConvertUnits.ToSimUnits(96), 1f, Vector2.Zero, Body);
            Body.CollisionCategories = Category.Cat4;
            Body.CollidesWith = Category.All;
            Body.IsStatic = true;
            Body.Position = sp.Player.Position;
        }

        /// <summary>
        /// Activates the drill
        /// </summary>
        public void Activate()
        {
            sp.Drilling = true;
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (Vector2.Distance(sp.Player.Position, Position) < 3 && inputProvider.Activate && !sp.Player.Dead)
            {
                Activate();
            }

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, EffectParameter parameter)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position) - new Vector2(32, 32);
            spriteBatch.Draw(resourceProvider.GetTexture("Drill"), screenPos, Color.White);
        }
    }
}