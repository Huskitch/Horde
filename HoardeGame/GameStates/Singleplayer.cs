using System;
using HoardeGame.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.GameStates
{
    public class SinglePlayer : State.GameState
    {
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;

        private Label _helloWorldLabel;
        private Button _helloWorldButton;

        public SinglePlayer(ContentManager content, SpriteBatch batch, GraphicsDevice device, GameWindow window)
        {
            graphicsDevice = device;
            spriteBatch = batch;

            ResourceManager.Init();
            ResourceManager.LoadContent(content);

            GuiBase.Font = ResourceManager.Font("BasicFont");
        }

        public override void Start()
        {
            _helloWorldLabel = new Label(this, nameof(_helloWorldLabel))
            {
                Position = new Vector2(10, 10),
                Text = "Hello world!"
            };

            _helloWorldButton = new Button(this, nameof(_helloWorldButton))
            {
                Position = new Vector2(10, 30),
                Text = "Click me!",
                OverColor = Color.Red,
                OnClick = () =>
                {
                    Environment.Exit(0);
                }
            };
        }

        public override void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();

            DoCheck(gameTime, new Point(state.X, state.Y), state.LeftButton == ButtonState.Pressed);
        }

        public override void Draw(GameTime gameTime, float interp)
        {
            graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            DoDraw(gameTime, spriteBatch, interp);

            spriteBatch.End();
        }
    }
}
