// <copyright file="SinglePlayer.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

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

        private readonly Camera camera;
        private readonly DungeonLevel dungeon;
        private readonly Minimap minimap;
        private readonly Rectangle minimapRectangle;
        private readonly Rectangle minimapInner;
        private readonly Rectangle screenRectangle;

        private ProgressBar heatlthBar;
        private ProgressBar armorBar;
        private Label healthLabel;
        private Label armorLabel;

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

            Player = new EntityPlayer(dungeon, inputProvider, resourceProvider);
            dungeon.AddEntity(Player);

            EntityBat bat = new EntityBat(dungeon, resourceProvider, this);
            bat.Body.Position = Player.Position;
            dungeon.AddEntity(bat);

            EntityChest chest = new EntityChest(dungeon, resourceProvider, this);
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
            heatlthBar = new ProgressBar(this, resourceProvider, "healthProgressBar")
            {
                Position = new Vector2(20, graphicsDevice.PresentationParameters.BackBufferHeight - 85),
                ProgressBarTexture = resourceProvider.GetTexture("BasicProgressBar"),
                BarHeight = 38,
                BarStart = new Vector2(1, 1),
                BarWidth = 118,
                TargetRectangle = new Rectangle(20, graphicsDevice.PresentationParameters.BackBufferHeight - 85, 120, 40),
                Color = new Color(190, 74, 57),
                Progress = 1f
            };

            healthLabel = new Label(this, "healthLabel")
            {
                Position = new Vector2(107, graphicsDevice.PresentationParameters.BackBufferHeight - 85),
                Color = Color.LightGray,
                Text = "100"
            };

            armorBar = new ProgressBar(this, resourceProvider, "armorProgressBar")
            {
                Position = new Vector2(15, graphicsDevice.PresentationParameters.BackBufferHeight - 65),
                ProgressBarTexture = resourceProvider.GetTexture("BasicProgressBar"),
                BarHeight = 28,
                BarStart = new Vector2(1, 1),
                BarWidth = 108,
                TargetRectangle = new Rectangle(15, graphicsDevice.PresentationParameters.BackBufferHeight - 65, 110, 30),
                Color = new Color(82, 141, 156),
                Progress = 1f
            };

            armorLabel = new Label(this, "armorLabel")
            {
                Position = new Vector2(93, graphicsDevice.PresentationParameters.BackBufferHeight - 65),
                Color = Color.LightGray,
                Text = "100"
            };
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            DoCheck(gameTime, new Point(inputProvider.MouseState.X, inputProvider.MouseState.Y), inputProvider.LeftClicked);

            dungeon.Update(gameTime);
            camera.Position = ConvertUnits.ToDisplayUnits(Player.Position);
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, float interp)
        {
            graphicsDevice.Clear(Color.Black);

            // GAME SPRITEBATCH
            dungeon.Draw(spriteBatch, camera, graphicsDevice);

            // MINIMAP SPRITEBATCH
            using (spriteBatch.Use(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone))
            {
                minimap.Draw(spriteBatch, minimapRectangle, minimapInner, camera);
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
            }
        }
    }
}