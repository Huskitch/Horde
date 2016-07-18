using System;
using HoardeGame.Graphics.Rendering;
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

        private Label _helloWorldLabel;
        private Button _helloWorldButton;

        private Camera camera;
        private DungeonLevel dungeon;

        public SinglePlayer(ContentManager content, SpriteBatch batch, GraphicsDevice device, GameWindow window)
        {
            graphicsDevice = device;
            spriteBatch = batch;

            ResourceManager.Init();
            ResourceManager.LoadContent(content);

            GuiBase.Font = ResourceManager.Font("BasicFont");

            camera = new Camera();
            camera.Position = new Vector2(400, 400);
            camera.Zoom = 2f;

            dungeon = new DungeonLevel();
            dungeon.GenerateLevel(64, 64, 40);
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
        }

        public override void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();

            DoCheck(gameTime, new Point(state.X, state.Y), state.LeftButton == ButtonState.Pressed);

            dungeon.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, float interp)
        {
            graphicsDevice.Clear(Color.Black);

            // GAME SPRITEBATCH
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, camera.Transformation(graphicsDevice));
            dungeon.Draw(spriteBatch);
            spriteBatch.End();

            // GUI SPRITEBATCH
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone);
            DoDraw(gameTime, spriteBatch, interp);
            spriteBatch.End();
        }
    }
}
