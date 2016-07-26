﻿// <copyright file="Main.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Linq;
using FarseerPhysics;
using HoardeGame.Gameplay;
using HoardeGame.GameStates;
using HoardeGame.Input;
using HoardeGame.Resources;
using HoardeGame.State;
using HoardeGame.Themes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame
{
    /// <summary>
    /// Core game class
    /// </summary>
    public class Main : Game
    {
        /// <summary>
        /// Gets the <see cref="MainMenu"/> <see cref="GameState"/>
        /// </summary>
        public MainMenu MainMenu { get; private set; }

        /// <summary>
        /// Gets the <see cref="MenuDemo"/> <see cref="GameState"/>
        /// </summary>
        public MenuDemo MenuDemo { get; private set; }

        /// <summary>
        /// Gets the <see cref="SinglePlayer"/> <see cref="GameState"/>
        /// </summary>
        public SinglePlayer SinglePlayer { get; private set; }

        private readonly GraphicsDeviceManager graphics;
        private readonly StateManager stateManager;
        private readonly IInputProvider inputProvider;
        private readonly ICardProvider cardProvider;
        private readonly IResourceProvider resourceProvider;
        private readonly IThemeProvider themeProvider;

        private SpriteBatch spriteBatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8
            };

            Content.RootDirectory = "Content";

            resourceProvider = new ResourceManager();
            inputProvider = new InputManager();
            cardProvider = new CardManager(resourceProvider);
            themeProvider = new ThemeManager();
            stateManager = new StateManager();

            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();

            Window.Title = "HORDE PROTOTYPE";
            IsMouseVisible = true;

            // 32 pixels = 1 meter
            ConvertUnits.SetDisplayUnitToSimUnitRatio(32f);
        }

        /// <inheritdoc/>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <inheritdoc/>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            (resourceProvider as ResourceManager).Init(graphics.GraphicsDevice);
            (resourceProvider as ResourceManager).LoadContent(Content);

            (cardProvider as CardManager).LoadXmlFile("Content/CARDS.xml");

            (themeProvider as ThemeManager).LoadXmlFile("Content/THEMES.xml", resourceProvider);

            string[] arguments = Environment.GetCommandLineArgs();

            SinglePlayer = new SinglePlayer(resourceProvider, inputProvider, cardProvider, themeProvider, spriteBatch, GraphicsDevice, stateManager);
            MainMenu = new MainMenu(spriteBatch, GraphicsDevice, inputProvider, resourceProvider, this, stateManager);
            MenuDemo = new MenuDemo(spriteBatch, resourceProvider, GraphicsDevice, inputProvider, themeProvider);

            if (arguments.FirstOrDefault(s => s.ToLower() == "-skipmenu") != null)
            {
                stateManager.Push(MainMenu);
                stateManager.Push(SinglePlayer);
            }
            else
            {
                stateManager.Push(MenuDemo);
                stateManager.Push(MainMenu);
            }
        }

        /// <inheritdoc/>
        protected override void UnloadContent()
        {
        }

        /// <inheritdoc/>
        protected override void Update(GameTime gameTime)
        {
            inputProvider.Update(gameTime);
            stateManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        protected override void Draw(GameTime gameTime)
        {
            stateManager.Draw(gameTime, 0);

            base.Draw(gameTime);
        }
    }
}