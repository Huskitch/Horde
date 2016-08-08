// <copyright file="MainMenu.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Reflection;
using HoardeGame.Extensions;
using HoardeGame.Graphics;
using HoardeGame.GUI;
using HoardeGame.Input;
using HoardeGame.Resources;
using HoardeGame.Settings;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private readonly GameServiceContainer serviceContainer;
        private readonly ISettingsService settingsService;

        private Button playButton;
        private Button exitButton;
        private Slider volumeSlider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenu"/> class.
        /// </summary>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        /// <param name="main"><see cref="Main"/></param>
        public MainMenu(GameServiceContainer serviceContainer, Main main)
        {
            this.main = main;
            this.serviceContainer = serviceContainer;

            spriteBatch = serviceContainer.GetService<ISpriteBatchService>().SpriteBatch;
            inputProvider = serviceContainer.GetService<IInputProvider>();
            resourceProvider = serviceContainer.GetService<IResourceProvider>();
            graphicsDevice = serviceContainer.GetService<IGraphicsDeviceService>().GraphicsDevice;
            stateManager = serviceContainer.GetService<IStateManagerService>().StateManager;
            settingsService = serviceContainer.GetService<ISettingsService>();
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

            exitButton = new Button(this, "exitButton")
            {
                Color = Color.White,
                OnClick = () =>
                {
                    main.Exit();
                },
                Text = "Exit [Escape / B]",
                Position = new Vector2(20, graphicsDevice.Viewport.Height - resourceProvider.GetFont("SmallFont").LineSpacing * 1.5f)
            };

            playButton = new Button(this, "playButton")
            {
                Color = Color.White,
                OnClick = () => Play(),
                Text = "Play [Enter / A]",
                Position = new Vector2(20, graphicsDevice.Viewport.Height - resourceProvider.GetFont("SmallFont").LineSpacing * 3f)
            };

            new Label(this, "volumeLabel")
            {
                Text = "Volume:",
                Position = new Vector2(20, 60)
            };

            volumeSlider = new Slider(this, serviceContainer, "volumeSlider")
            {
                Position = new Vector2(117, 68),
                Width = 200,
                DrawText = true,
                TextAppend = "%",
                Progress = 0.5f
            };
        }

        /// <inheritdoc/>
        public override void Resume()
        {
            main.IsMouseVisible = true;
            base.Resume();
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (Paused)
            {
                return;
            }

            DoCheck(gameTime, new Point(inputProvider.MouseState.X, inputProvider.MouseState.Y), inputProvider.LeftClicked);

            SoundEffect.MasterVolume = volumeSlider.Progress;
            settingsService.Settings.Volume = volumeSlider.Progress;

            if (stateManager.GameStates.Contains(main.SinglePlayer))
            {
                playButton.Text = "Resume [Esc / Start]";
                exitButton.Text = "Exit [Alt + F4 / B]";
            }
            else
            {
                playButton.Text = "Play [Enter / A]";
                exitButton.Text = "Exit [Esc / B]";
            }

            if (stateManager.GameStates.Contains(main.SinglePlayer))
            {
                if (inputProvider.KeybindPressed("PauseGame"))
                {
                    Play();
                }

                if (inputProvider.KeybindPressed("ExitPausedGame"))
                {
                    main.Exit();
                }
            }
            else
            {
                if (inputProvider.KeybindPressed("StartGame"))
                {
                    Play();
                }

                if (inputProvider.KeybindPressed("ExitGame"))
                {
                    main.Exit();
                }
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

        private void Play()
        {
            if (!main.SinglePlayer.Drilling && !main.SinglePlayer.Shopping)
            {
                main.IsMouseVisible = false;
            }

            if (stateManager.GameStates.Contains(main.SinglePlayer))
            {
                stateManager.Switch(main.SinglePlayer);
            }
            else
            {
                stateManager.GameStates.Remove(main.MenuDemo);
                stateManager.Push(main.SinglePlayer);
            }
        }
    }
}