using System.Collections.Generic;
using System.Linq;
using FarseerPhysics;
using HoardeGame.GameStates;
using HoardeGame.GUI;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private StateManager stateManager;

        public static KeyboardState KState;
        public static KeyboardState LastKState;
        public static MouseState LastMState;
        public static MouseState MState;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            stateManager = new StateManager();

            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();

            Window.Title = "HORDE PROTOTYPE";
            IsMouseVisible = true;

            ConvertUnits.SetDisplayUnitToSimUnitRatio(32f);
        }

        public static bool JustPressed(bool leftMouse = true)
        {
            if (leftMouse)
                return LastMState.LeftButton == ButtonState.Released & MState.LeftButton == ButtonState.Pressed;

            return LastMState.RightButton == ButtonState.Released & MState.RightButton == ButtonState.Pressed;
        }

        public static bool JustKeyDown(Keys key)
        {
            return LastKState.IsKeyDown(key) == false & KState.IsKeyDown(key);
        }

        public static List<Keys> JustKeysDown()
        {
            return KState.GetPressedKeys().Where(keyState => LastKState.IsKeyUp(keyState)).ToList();
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            stateManager.Push(new SinglePlayer(Content, spriteBatch, GraphicsDevice, Window));
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            LastMState = MState;
            MState = Mouse.GetState();

            LastKState = KState;
            KState = Keyboard.GetState();

            stateManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            stateManager.Draw(gameTime, 0);
            base.Draw(gameTime);
        }
    }
}
