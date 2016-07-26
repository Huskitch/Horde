// <copyright file="MainMenu.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Linq;
using System.Reflection;
using HoardeGame.Extensions;
using HoardeGame.GUI;
using HoardeGame.Input;
using HoardeGame.Resources;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GameStates
{
    /// <summary>
    /// Main menu gamestate
    /// </summary>
    public class MainMenu : GameState
    {
        private readonly SpriteBatch spriteBatch;
        private readonly IInputProvider inputProvider;
        private readonly IResourceProvider resourceProvider;
        private readonly GraphicsDevice graphicsDevice;
        private readonly Main main;
        private readonly StateManager stateManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenu"/> class.
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> to draw with</param>
        /// <param name="graphicsDevice"><see cref="GraphicsDevice"/> to draw with</param>
        /// <param name="inputProvider"><see cref="IInputProvider"/> to use for input</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        /// <param name="game"><see cref="Main"/></param>
        /// <param name="stateManager"><see cref="StateManager"/> for switching states</param>
        public MainMenu(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, IInputProvider inputProvider, IResourceProvider resourceProvider, Main main, StateManager stateManager)
        {
            this.spriteBatch = spriteBatch;
            this.inputProvider = inputProvider;
            this.resourceProvider = resourceProvider;
            this.graphicsDevice = graphicsDevice;
            this.main = main;
            this.stateManager = stateManager;
        }

        /// <inheritdoc/>
        public override void Start()
        {
            new Label(this, "version")
            {
                Text = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                Position = new Vector2(graphicsDevice.Viewport.Width - 5, graphicsDevice.Viewport.Height) - resourceProvider.GetFont("SmallFont").MeasureString(Assembly.GetExecutingAssembly().GetName().Version.ToString()),
                Color = Color.White
            };

            new Label(this, "name")
            {
                Text = "Horde",
                Position = new Vector2(20, 20),
                Color = Color.White,
                CustomFont = resourceProvider.GetFont("BigFont")
            };

            new Button(this, "exitButton")
            {
                Color = Color.White,
                OnClick = () =>
                {
                    main.Exit();
                },
                Text = "Exit",
                Position = new Vector2(20, graphicsDevice.Viewport.Height - resourceProvider.GetFont("SmallFont").LineSpacing * 1.5f)
            };

            new Button(this, "playButton")
            {
                Color = Color.White,
                OnClick = () =>
                {
                    if (stateManager.GameStates.Contains(main.SinglePlayer))
                    {
                        stateManager.Switch(main.SinglePlayer);
                    }
                    else
                    {
                        stateManager.Push(main.SinglePlayer);
                    }
                },
                Text = "Play / Resume",
                Position = new Vector2(20, graphicsDevice.Viewport.Height - resourceProvider.GetFont("SmallFont").LineSpacing * 3f)
            };
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (Paused)
            {
                return;
            }

            DoCheck(gameTime, new Point(inputProvider.MouseState.X, inputProvider.MouseState.Y), inputProvider.LeftClicked);

            if (inputProvider.Back)
            {
                stateManager.Switch(stateManager.GameStates.First(state => state.GetType() == typeof(SinglePlayer)));
            }
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, float interpolation)
        {
            if (Paused)
            {
                return;
            }

            using (spriteBatch.Use())
            {
                spriteBatch.Draw(resourceProvider.GetTexture("OneByOneEmpty"), graphicsDevice.Viewport.Bounds, new Color(0, 0, 0, 150));
                DoDraw(gameTime, spriteBatch, interpolation);
            }
        }
    }
}