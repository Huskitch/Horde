// <copyright file="Main.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using HoardeGame.GameStates;
using HoardeGame.Input;
using HoardeGame.State;
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
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private StateManager stateManager;
        private IInputProvider inputProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            inputProvider = new InputManager();
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

            stateManager.Push(new SinglePlayer(inputProvider, Content, spriteBatch, GraphicsDevice, Window));
        }

        /// <inheritdoc/>
        protected override void UnloadContent()
        {
        }

        /// <inheritdoc/>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

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