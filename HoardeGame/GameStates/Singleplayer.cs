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
using HoardeGame.Themes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GameStates
{
    /// <summary>
    /// Singleplayer gamestate
    /// </summary>
    public class SinglePlayer : GameState, IPlayerProvider
    {
        /// <summary>
        /// Gets or sets a value indicating whether the player is drilling
        /// </summary>
        public bool Drilling { get; set; }

        /// <summary>
        /// Gets the player entity
        /// </summary>
        public EntityPlayer Player { get; private set; }

        private readonly Card testCard;

        private readonly SpriteBatch spriteBatch;
        private readonly GraphicsDevice graphicsDevice;
        private readonly IInputProvider inputProvider;
        private readonly ICardProvider cardProvider;
        private readonly IResourceProvider resourceProvider;
        private readonly IThemeProvider themeProvider;

        private readonly Camera camera;
        private readonly DungeonLevel dungeon;
        private readonly Minimap minimap;
        private readonly Rectangle minimapRectangle;
        private readonly Rectangle minimapInner;
        private readonly Rectangle screenRectangle;

        private ProgressBar healthBar;
        private ProgressBar armorBar;
        private Label healthLabel;
        private Label armorLabel;
        private Label ammoLabel;
        private DepthStencilState barDepthStencilState;

        /// <summary>
        /// Initializes a new instance of the <see cref="SinglePlayer"/> class.
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> to draw with</param>
        /// <param name="graphicsDevice"><see cref="GraphicsDevice"/> to draw with</param>
        /// <param name="window"><see cref="GameWindow"/> to draw in</param>
        /// <param name="inputProvider"><see cref="IInputProvider"/> to use for input</param>
        /// <param name="cardProvider"><see cref="ICardProvider"/> for managing cards</param>
        /// <param name="themeProvider"><see cref="IThemeProvider"/> for managing level themes</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        public SinglePlayer(IResourceProvider resourceProvider, IInputProvider inputProvider, ICardProvider cardProvider, IThemeProvider themeProvider, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, GameWindow window)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
            this.inputProvider = inputProvider;
            this.cardProvider = cardProvider;
            this.resourceProvider = resourceProvider;
            this.themeProvider = themeProvider;

            testCard = cardProvider.GetCard("testCard");

            GuiBase.Font = resourceProvider.GetFont("SmallFont");

            screenRectangle = new Rectangle(0, 0, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            camera = new Camera
            {
                Zoom = 2.6f,
                Size = new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight)
            };

            dungeon = new DungeonLevel(resourceProvider, this, themeProvider.GetTheme("temple"));
            dungeon.GenerateLevel(64, 64, 40);

            Player = new EntityPlayer(dungeon, inputProvider, resourceProvider);
            dungeon.AddEntity(Player);

            EntityChest chest = new EntityChest(dungeon, resourceProvider, this, themeProvider.GetTheme("temple").ChestInfo)
            {
                Body =
                {
                    Position = Player.Position + new Vector2(1, 1)
                }
            };
            dungeon.AddEntity(chest);

            EntityDrill drill = new EntityDrill(dungeon, inputProvider, resourceProvider, this);
            dungeon.AddEntity(drill);

            minimap = new Minimap(resourceProvider);
            minimap.Generate(graphicsDevice, dungeon.GetMap());

            // minimap.Generate(graphicsDevice, dungeon.GetSearchMap());
            minimapRectangle = new Rectangle(graphicsDevice.PresentationParameters.BackBufferWidth - 280, graphicsDevice.PresentationParameters.BackBufferHeight - 280, 260, 260);
            minimapInner = new Rectangle(graphicsDevice.PresentationParameters.BackBufferWidth - 278, graphicsDevice.PresentationParameters.BackBufferHeight - 278, 256, 256);
        }

        /// <inheritdoc/>
        public override void Start()
        {
            healthBar = new ProgressBar(this, resourceProvider, "healthProgressBar")
            {
                Position = new Vector2(30, 20),
                ProgressBarTexture = resourceProvider.GetTexture("BasicProgressBar"),
                BarHeight = 45,
                BarStart = new Vector2(1, 1),
                BarWidth = 200,
                TargetRectangle = new Rectangle(0, 0, 0, 0),
                Color = new Color(190, 74, 57),
                Progress = 1f
            };

            healthLabel = new Label(this, "healthLabel")
            {
                Position = new Vector2(176, 29),
                Color = Color.LightGray,
                Text = "10",
            };

            armorBar = new ProgressBar(this, resourceProvider, "armorProgressBar")
            {
                Position = new Vector2(15, 45),
                ProgressBarTexture = resourceProvider.GetTexture("BasicProgressBar"),
                BarHeight = 45,
                BarStart = new Vector2(1, 1),
                BarWidth = 144,
                TargetRectangle = new Rectangle(0, 0, 0, 0),
                Color = new Color(82, 141, 156),
                Progress = 1f
            };

            armorLabel = new Label(this, "armorLabel")
            {
                Position = new Vector2(130 * armorBar.Progress, 55),
                Color = Color.LightGray,
                Text = "10"
            };

            ammoLabel = new Label(this, "ammoLabel")
            {
                Position = new Vector2(20, graphicsDevice.PresentationParameters.BackBufferHeight - 115),
                Text = "Ammo: 100"
            };

            AlphaTestEffect alphaTestEffect = new AlphaTestEffect(graphicsDevice)
            {
                DiffuseColor = Color.White.ToVector3(),
                AlphaFunction = CompareFunction.Greater,
                ReferenceAlpha = 0,
                World = Matrix.Identity,
                View = Matrix.Identity,
                Projection = Matrix.CreateTranslation(-0.5f, -0.5f, 0) *
                             Matrix.CreateOrthographicOffCenter(0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, 0, 0, 1)
            };

            DepthStencilState beforeDepthStencilState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1
            };

            barDepthStencilState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Equal,
                ReferenceStencil = 1
            };

            using (spriteBatch.Use(SpriteSortMode.Deferred, null, null, beforeDepthStencilState, null, alphaTestEffect))
            {
                spriteBatch.Draw(resourceProvider.GetTexture("healthBG"), new Rectangle(36, 23, 200, 48), Color.White);
                spriteBatch.Draw(resourceProvider.GetTexture("healthBG"), new Rectangle(21, 48, 144, 48), Color.White);
            }
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            DoCheck(gameTime, new Point(inputProvider.MouseState.X, inputProvider.MouseState.Y), inputProvider.LeftClicked);

            dungeon.Update(gameTime);

            camera.Position = ConvertUnits.ToDisplayUnits(Player.Position);

            healthLabel.Text = Player.Health.ToString();
            healthLabel.Position = healthBar.Position + new Vector2(8, 10) + new Vector2(healthBar.BarWidth * Math.Max(healthBar.Progress - 0.17f, 0f), 0);
            healthBar.Progress = Player.Health / 10f;

            armorLabel.Text = Player.Armour.ToString();
            armorBar.Progress = Player.Armour / 10f;
            armorLabel.Position = armorBar.Position + new Vector2(8, 10) + new Vector2(armorBar.BarWidth * Math.Max(armorBar.Progress - 0.23f, 0f), 0);

            ammoLabel.Text = $"Ammo: {Player.Ammo}";
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, float interp)
        {
            graphicsDevice.Clear(ClearOptions.Target, Color.Black, 0f, 0);

            // GAME SPRITEBATCH
            dungeon.Draw(spriteBatch, camera, graphicsDevice);

            // MINIMAP SPRITEBATCH
            using (spriteBatch.Use(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone))
            {
                minimap.Draw(spriteBatch, minimapRectangle, minimapInner, camera);
            }

            using (spriteBatch.Use(SpriteSortMode.Deferred, null, null, barDepthStencilState))
            {
                spriteBatch.Draw(resourceProvider.GetTexture("healthBG"), graphicsDevice.Viewport.Bounds, Color.Black);
            }

            // GUI SPRITEBATCH
            using (spriteBatch.Use(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone))
            {
                DoDraw(gameTime, spriteBatch, interp);

                // CARDS
                if (Drilling)
                {
                    spriteBatch.Draw(resourceProvider.GetTexture("OneByOneEmpty"), screenRectangle, new Color(0, 0, 0, 150));

                    testCard.Draw(new Vector2(175, 247), 0.75f, gameTime, spriteBatch, interp);
                    testCard.Draw(new Vector2(650, 247), 0.75f, gameTime, spriteBatch, interp);
                    testCard.Draw(new Vector2(1075, 247), 0.75f, gameTime, spriteBatch, interp);
                }

                if (Player.Dead)
                {
                    Vector2 screenCenter = new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth / 2f, graphicsDevice.PresentationParameters.BackBufferHeight / 2f);
                    Vector2 textSize = resourceProvider.GetFont("BasicFont").MeasureString("YOU DIED") * 4;
                    screenCenter = screenCenter - textSize / 2;

                    spriteBatch.DrawString(resourceProvider.GetFont("BasicFont"), "YOU DIED", screenCenter, Color.DarkRed, 0f, Vector2.Zero, new Vector2(4, 4), SpriteEffects.None, 0f);
                }
            }
        }
    }
}