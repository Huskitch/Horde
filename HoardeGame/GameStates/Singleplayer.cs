using System;
using System.Collections.Generic;
using System.Diagnostics;
using FarseerPhysics;
using HoardeGame.Entities;
using HoardeGame.Gameplay;
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

        private Camera camera;
        private DungeonLevel dungeon;

        private ProgressBar _bar;
        private Rectangle _minimap;
        private Rectangle _minimapInner;
        private Card _testCard;

        public SinglePlayer(ContentManager content, SpriteBatch batch, GraphicsDevice device, GameWindow window)
        {
            graphicsDevice = device;
            spriteBatch = batch;

            ResourceManager.Init(graphicsDevice);
            ResourceManager.LoadContent(content);

            CardManager.Init();
            CardManager.LoadXmlFile("Content/CARDS.xml");
            CardManager.LoadBackgrounds();

            _testCard = CardManager.GetCard("testCard");

            GuiBase.Font = ResourceManager.Font("BasicFont");

            camera = new Camera
            {
                Zoom = 2f
            };

            dungeon = new DungeonLevel();
            dungeon.GenerateLevel(64, 64, 40);

            dungeon.AddEntity<EntityPlayer>();
            dungeon.AddEntity<EntityBat>();

            _minimap = new Rectangle(graphicsDevice.PresentationParameters.BackBufferWidth - 260, graphicsDevice.PresentationParameters.BackBufferHeight - 260, 260, 260);
            _minimapInner = new Rectangle(graphicsDevice.PresentationParameters.BackBufferWidth - 258, graphicsDevice.PresentationParameters.BackBufferHeight - 258, 256, 256);

            Minimap.GenerateMinimap(device, dungeon.GetMap());
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
                    Random rnd = new Random();

                    _bar.Color = new Color(new Vector3((float) rnd.NextDouble(), (float) rnd.NextDouble(), (float) rnd.NextDouble()));
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

            DoCheck(gameTime, new Point(state.X, state.Y), Main.JustPressed());

            dungeon.Update(gameTime);
            camera.Position = ConvertUnits.ToDisplayUnits(EntityPlayer.Player.Position);
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

                //CARD
                _testCard.Draw(new Vector2(10, 150), 1f, gameTime, spriteBatch, interp);
            spriteBatch.End();

            //MINIMAP SPRITEBATCH
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(ResourceManager.Texture("OneByOneEmpty"), _minimap, Color.Gray);
            spriteBatch.Draw(Minimap.CurrentMinimap, _minimapInner, Color.White);
            spriteBatch.End();
        }
    }
}
