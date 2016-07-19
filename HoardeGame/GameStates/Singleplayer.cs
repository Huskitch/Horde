// <copyright file="SinglePlayer.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using FarseerPhysics;
using HoardeGame.Entities;
using HoardeGame.Extensions;
using HoardeGame.Gameplay;
using HoardeGame.Graphics.Rendering;
using HoardeGame.GUI;
using HoardeGame.Input;
using HoardeGame.Level;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.GameStates
{
    /// <summary>
    /// Singleplayer gamestate
    /// </summary>
    public class SinglePlayer : GameState
    {
        private readonly Rectangle minimap;
        private readonly Rectangle minimapInner;
        private readonly Card testCard;

        private readonly SpriteBatch spriteBatch;
        private readonly GraphicsDevice graphicsDevice;

        private readonly Camera camera;
        private readonly DungeonLevel dungeon;

        private ProgressBar bar;

        /// <summary>
        /// Initializes a new instance of the <see cref="SinglePlayer"/> class.
        /// </summary>
        /// <param name="content"><see cref="ContentManager"/> to load resources with</param>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> to draw with</param>
        /// <param name="graphicsDevice"><see cref="GraphicsDevice"/> to draw with</param>
        /// <param name="window"><see cref="GameWindow"/> to draw in</param>
        public SinglePlayer(ContentManager content, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, GameWindow window)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;

            ResourceManager.Init(graphicsDevice);
            ResourceManager.LoadContent(content);

            CardManager.Init();
            CardManager.LoadXmlFile("Content/CARDS.xml");
            CardManager.LoadBackgrounds();

            testCard = CardManager.GetCard("testCard");

            GuiBase.Font = ResourceManager.GetFont("BasicFont");

            camera = new Camera
            {
                Zoom = 2f
            };

            dungeon = new DungeonLevel();
            dungeon.GenerateLevel(64, 64, 40);

            dungeon.AddEntity<EntityPlayer>();
            dungeon.AddEntity<EntityBat>();
            dungeon.AddEntity<EntityChest>();

            minimap = new Rectangle(graphicsDevice.PresentationParameters.BackBufferWidth - 260, graphicsDevice.PresentationParameters.BackBufferHeight - 260, 260, 260);
            minimapInner = new Rectangle(graphicsDevice.PresentationParameters.BackBufferWidth - 258, graphicsDevice.PresentationParameters.BackBufferHeight - 258, 256, 256);

            Minimap.GenerateMinimap(graphicsDevice, dungeon.GetMap());
        }

        /// <inheritdoc/>
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
                TextOffset = new Vector2(10, 5),
                ButtonTexture = ResourceManager.GetTexture("BasicButton"),
                ButtonTextureOver = ResourceManager.GetTexture("BasicButtonHover"),
                Text = "Click me!",
                OverColor = Color.Red,
                OnClick = () =>
                {
                    Random rnd = new Random();

                    bar.Color = new Color(new Vector3((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()));
                }
            };

            bar = new ProgressBar(this, "helloWorldProgressBar")
            {
                Position = new Vector2(10, 70),
                ProgressBarTexture = ResourceManager.GetTexture("BasicProgressBar"),
                BarHeight = 23,
                BarStart = new Vector2(1, 1),
                BarWidth = 63,
                TargetRectangle = new Rectangle(10, 70, 65, 25),
                Color = Color.Blue,
                Progress = 1f
            };
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();

            bar.Progress = gameTime.TotalGameTime.Milliseconds / 1000f;

            DoCheck(gameTime, new Point(state.X, state.Y), InputManager.LeftClicked);

            dungeon.Update(gameTime);
            camera.Position = ConvertUnits.ToDisplayUnits(EntityPlayer.Player.Position);
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, float interp)
        {
            graphicsDevice.Clear(Color.Black);

            // GAME SPRITEBATCH
            using (spriteBatch.Use(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, camera.Transformation(graphicsDevice)))
            {
                dungeon.Draw(spriteBatch);
            }

            // GUI SPRITEBATCH
            using (spriteBatch.Use(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone))
            {
                DoDraw(gameTime, spriteBatch, interp);

                // CARD
                testCard.Draw(new Vector2(10, 150), 1f, gameTime, spriteBatch, interp);
            }

            // MINIMAP SPRITEBATCH
            using (spriteBatch.Use(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone))
            {
                spriteBatch.Draw(ResourceManager.GetTexture("OneByOneEmpty"), minimap, Color.Gray);
                spriteBatch.Draw(Minimap.CurrentMinimap, minimapInner, Color.White);
            }
        }
    }
}