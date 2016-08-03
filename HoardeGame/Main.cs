// <copyright file="Main.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Linq;
using FarseerPhysics;
using HoardeGame.Gameplay.Cards;
using HoardeGame.Gameplay.Themes;
using HoardeGame.Gameplay.Weapons;
using HoardeGame.GameStates;
using HoardeGame.Graphics;
using HoardeGame.Input;
using HoardeGame.Resources;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame
{
    /// <summary>
    /// Core game class
    /// </summary>
    public class Main : Game, ISpriteBatchService, IStateManagerService
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

        /// <summary>
        /// Gets or sets the SpriteBatch
        /// </summary>
        public SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// Gets the StateManager
        /// </summary>
        public StateManager StateManager { get; }

        private readonly GraphicsDeviceManager graphics;
        private readonly IInputProvider inputProvider;
        private readonly ICardProvider cardProvider;
        private readonly IResourceProvider resourceProvider;
        private readonly IThemeProvider themeProvider;
        private readonly IWeaponProvider weaponProvider;

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

            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();

            Window.Title = "HORDE PROTOTYPE";
            IsMouseVisible = true;

            resourceProvider = new ResourceManager();
            inputProvider = new InputManager();
            cardProvider = new CardManager(Services);
            themeProvider = new ThemeManager();
            weaponProvider = new WeaponManager();
            StateManager = new StateManager();

            Services.AddService(resourceProvider);
            Services.AddService(inputProvider);
            Services.AddService(cardProvider);
            Services.AddService(themeProvider);
            Services.AddService(weaponProvider);
            Services.AddService<ISpriteBatchService>(this);
            Services.AddService<IStateManagerService>(this);

            inputProvider.LockCursor = false;

            // 32 pixels = 1 meter
            ConvertUnits.SetDisplayUnitToSimUnitRatio(32f);
        }

        /// <inheritdoc/>
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            ((ResourceManager)resourceProvider).Init(graphics.GraphicsDevice);
            ((ResourceManager)resourceProvider).LoadContent(Content);
            ((CardManager)cardProvider).LoadXmlFile("Content/CARDS.xml");
            ((ThemeManager)themeProvider).LoadXmlFile("Content/THEMES.xml", Services);
            ((WeaponManager)weaponProvider).LoadXmlFile("Content/WEAPONS.xml", Services);
            ((InputManager)inputProvider).Init(Services);

            string[] arguments = Environment.GetCommandLineArgs();

            SinglePlayer = new SinglePlayer(Services);
            MainMenu = new MainMenu(Services, this);
            MenuDemo = new MenuDemo(Services);

            if (arguments.FirstOrDefault(s => s.ToLower() == "-skipmenu") != null)
            {
                StateManager.Push(MainMenu);
                StateManager.Push(SinglePlayer);
                IsMouseVisible = false;
            }
            else
            {
                StateManager.Push(MenuDemo);
                StateManager.Push(MainMenu);
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
            StateManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <inheritdoc/>
        protected override void Draw(GameTime gameTime)
        {
            StateManager.Draw(gameTime, 0);

            base.Draw(gameTime);
        }
    }
}