using System;
using HoardeGame.GUI;
using HoardeGame.Level;
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

        private DungeonLevel dungeon;

        private ProgressBar _bar;

        public SinglePlayer(ContentManager content, SpriteBatch batch, GraphicsDevice device, GameWindow window)
        {
            graphicsDevice = device;
            spriteBatch = batch;

            ResourceManager.Init(graphicsDevice);
            ResourceManager.LoadContent(content);

            GuiBase.Font = ResourceManager.Font("BasicFont");

            dungeon = new DungeonLevel();
            dungeon.GenerateLevel(64, 64, 40);
        }

        public override void Start()
        {
            new Label(this, "helloWorldLabel")
            {
                Position = new Vector2(10, 10),
                Text = "Hello world!"
            };

            new Button(this, "helloWorldButton")
            {
                Position = new Vector2(10, 30),
                TargetRectangle = new Rectangle(10, 30, 128, 32),
                TextOffset = new Vector2(10,5),
                ButtonTexture = ResourceManager.Texture("BasicButton"),
                ButtonTextureOver = ResourceManager.Texture("BasicButtonHover"),
                Text = "Click me!",
                OverColor = Color.Red,
                OnClick = () =>
                {
                    Environment.Exit(0);
                }
            };

            _bar = new ProgressBar(this, "helloWorldProgressBar")
            {
                Position = new Vector2(10, 70),
                ProgressBarTexture = ResourceManager.Texture("BasicProgressBar"),
                BarHeight = 23,
                BarStart = new Vector2(1, 1),
                BarWidth = 63,
                TargetRectangle = new Rectangle(10, 70, 65, 25),
                Color = Color.Blue,
                Progress = 1f
            };
        }

        public override void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();

            _bar.Progress = gameTime.TotalGameTime.Milliseconds / 1000f;

            DoCheck(gameTime, new Point(state.X, state.Y), state.LeftButton == ButtonState.Pressed);

            dungeon.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, float interp)
        {
            graphicsDevice.Clear(Color.Black);

            // GAME SPRITEBATCH
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null);
            dungeon.Draw(spriteBatch);
            spriteBatch.End();

            // GUI SPRITEBATCH
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone);
            DoDraw(gameTime, spriteBatch, interp);
            spriteBatch.End();
        }
    }
}
