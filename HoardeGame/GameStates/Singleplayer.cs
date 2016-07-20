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
using HoardeGame.Resources;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.GameStates
{
    /// <summary>
    /// Singleplayer gamestate
    /// </summary>
    public class SinglePlayer : GameState
    {
        /// <summary>
        /// Gets or sets a value indicating whether the player is drilling
        /// </summary>
        public bool Drilling { get; set; }

        private readonly Card testCard;

        private readonly SpriteBatch spriteBatch;
        private readonly GraphicsDevice graphicsDevice;
        private readonly IInputProvider inputProvider;
        private readonly ICardProvider cardProvider;
        private readonly IResourceProvider resourceProvider;

        private readonly Camera camera;
        private readonly DungeonLevel dungeon;
        private readonly Minimap minimap;
        private readonly Rectangle minimapRectangle;
        private readonly Rectangle minimapInner;
        private readonly Rectangle screenRectangle;

        private ProgressBar bar;

        /// <summary>
        /// Initializes a new instance of the <see cref="SinglePlayer"/> class.
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> to draw with</param>
        /// <param name="graphicsDevice"><see cref="GraphicsDevice"/> to draw with</param>
        /// <param name="window"><see cref="GameWindow"/> to draw in</param>
        /// <param name="inputProvider"><see cref="IInputProvider"/> to use for input</param>
        /// <param name="cardProvider"><see cref="ICardProvider"/> for managing cards</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        public SinglePlayer(IResourceProvider resourceProvider, IInputProvider inputProvider, ICardProvider cardProvider, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, GameWindow window)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
            this.inputProvider = inputProvider;
            this.cardProvider = cardProvider;
            this.resourceProvider = resourceProvider;

            testCard = cardProvider.GetCard("testCard");

            GuiBase.Font = resourceProvider.GetFont("BasicFont");

            screenRectangle = new Rectangle(0, 0, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            camera = new Camera
            {
                Zoom = 2.4f,
                Size = new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight)
            };

            dungeon = new DungeonLevel(resourceProvider);
            dungeon.GenerateLevel(64, 64, 40);

            EntityPlayer player = new EntityPlayer(dungeon, inputProvider, resourceProvider);
            dungeon.AddEntity(player);

            EntityBat bat = new EntityBat(dungeon, resourceProvider);
            dungeon.AddEntity(bat);

            EntityChest chest = new EntityChest(dungeon, resourceProvider);
            dungeon.AddEntity(chest);

            EntityDrill drill = new EntityDrill(dungeon, inputProvider, resourceProvider, this);
            dungeon.AddEntity(drill);

            minimap = new Minimap(resourceProvider);
            minimap.Generate(graphicsDevice, dungeon.GetMap());

            minimapRectangle = new Rectangle(graphicsDevice.PresentationParameters.BackBufferWidth - 280, graphicsDevice.PresentationParameters.BackBufferHeight - 280, 260, 260);
            minimapInner = new Rectangle(graphicsDevice.PresentationParameters.BackBufferWidth - 278, graphicsDevice.PresentationParameters.BackBufferHeight - 278, 256, 256);
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
                ButtonTexture = resourceProvider.GetTexture("BasicButton"),
                ButtonTextureOver = resourceProvider.GetTexture("BasicButtonHover"),
                Text = "Click me!",
                OverColor = Color.Red,
                OnClick = () =>
                {
                    Random rnd = new Random();

                    bar.Color = new Color(new Vector3((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()));
                }
            };

            bar = new ProgressBar(this, resourceProvider, "helloWorldProgressBar")
            {
                Position = new Vector2(10, 70),
                ProgressBarTexture = resourceProvider.GetTexture("BasicProgressBar"),
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

            DoCheck(gameTime, new Point(state.X, state.Y), inputProvider.LeftClicked);

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

                // CARDS
                if (Drilling)
                {
                    spriteBatch.Draw(resourceProvider.GetTexture("OneByOneEmpty"), screenRectangle, new Color(0, 0, 0, 150));

                    testCard.Draw(new Vector2(40, 300), .75f, gameTime, spriteBatch, interp);
                    testCard.Draw(new Vector2(500, 300), .75f, gameTime, spriteBatch, interp);
                    testCard.Draw(new Vector2(970, 300), .75f, gameTime, spriteBatch, interp);
                }
            }

            // MINIMAP SPRITEBATCH
            using (spriteBatch.Use(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone))
            {
                minimap.Draw(spriteBatch, minimapRectangle, minimapInner, camera);
            }
        }
    }
}