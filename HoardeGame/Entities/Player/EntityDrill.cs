﻿// <copyright file="EntityDrill.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using HoardeGame.Entities.Base;
using HoardeGame.Gameplay.Level;
using HoardeGame.GameStates;
using HoardeGame.GUI;
using HoardeGame.Input;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Entities.Player
{
    /// <summary>
    /// Drill entity
    /// </summary>
    public class EntityDrill : EntityBase
    {
        /// <summary>
        /// Maximum health for the drill
        /// </summary>
        public const int MaxHealth = 100;

        /// <summary>
        /// Maximum shield for the drill
        /// </summary>
        public const int MaxShield = 100;

        /// <summary>
        /// Gets or sets the shield of the drill
        /// </summary>
        public int Shield { get; set; }

        private readonly IInputProvider inputProvider;
        private readonly IResourceProvider resourceProvider;
        private readonly SinglePlayer sp;
        private readonly Game game;

        private ScrollBar shopScrollBar;

        // This probably isn't the best solution since you can't swap out sp for an interface but having both sp and playerProvider seems useless

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDrill"/> class.
        /// </summary>
        /// <param name="level"><see cref="DungeonLevel"/> to place this entity in</param>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        /// <param name="sp"><see cref="SinglePlayer"/> gamestate</param>
        public EntityDrill(DungeonLevel level, GameServiceContainer serviceContainer, SinglePlayer sp) : base(level, serviceContainer)
        {
            resourceProvider = serviceContainer.GetService<IResourceProvider>();
            inputProvider = serviceContainer.GetService<IInputProvider>();
            game = serviceContainer.GetService<Game>();
            this.sp = sp;

            Health = MaxHealth;
            Shield = MaxShield;

            FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(96), ConvertUnits.ToSimUnits(96), 1f, Vector2.Zero, Body);
            Body.CollisionCategories = Category.Cat4;
            Body.CollidesWith = Category.All;
            Body.IsStatic = true;
            Body.Position = sp.Player.Position;

            shopScrollBar = new ScrollBar(sp, serviceContainer, "shopScrollBar")
            {
                Position = new Vector2(500, 100),
                PinColor = Color.CadetBlue,
                Height = 300,
                Visibility = HiddenState.Hidden
            };
        }

        /// <summary>
        /// Activates the drill
        /// </summary>
        public void Activate()
        {
            sp.Shopping = true;
            game.IsMouseVisible = true;

            shopScrollBar.Visibility = HiddenState.Visible;
        }

        /// <summary>
        /// Deactivates the drill
        /// </summary>
        public void Deactivate()
        {
            sp.Shopping = false;
            game.IsMouseVisible = false;

            shopScrollBar.Visibility = HiddenState.Hidden;
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (Vector2.Distance(sp.Player.Position, Position) < 3 && inputProvider.KeybindPressed("Activate") && !sp.Player.Dead)
            {
                Activate();
            }

            if (sp.Shopping && inputProvider.KeybindPressed("PauseGame") && !sp.Player.Dead)
            {
                Deactivate();
            }

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(EffectParameter parameter)
        {
            Vector2 screenPos = ConvertUnits.ToDisplayUnits(Position) - new Vector2(32, 32);
            SpriteBatch.Draw(resourceProvider.GetTexture("Drill"), screenPos, Color.White);
        }
    }
}