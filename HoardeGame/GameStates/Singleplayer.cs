// <copyright file="SinglePlayer.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Linq;
using FarseerPhysics;
using HoardeGame.Entities.Player;
using HoardeGame.Extensions;
using HoardeGame.Gameplay.Cards;
using HoardeGame.Gameplay.Level;
using HoardeGame.Gameplay.Player;
using HoardeGame.Gameplay.Themes;
using HoardeGame.Gameplay.Weapons;
using HoardeGame.Graphics;
using HoardeGame.GUI;
using HoardeGame.Input;
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
    public class SinglePlayer : GameState, IPlayerProvider
    {
        /// <summary>
        /// Gets or sets a value indicating whether the player is drilling
        /// </summary>
        public bool Drilling { get; set; }

        /// <summary>
        /// Gets the player entity
        /// </summary>
        public IPlayer Player { get; private set; }

        /// <summary>
        /// Gets the drill entity
        /// </summary>
        public EntityDrill Drill { get; private set; }

        /// <summary>
        /// Gets the current <see cref="Camera"/>
        /// </summary>
        public Camera Camera { get; }

        private readonly Card testCard;

        private readonly SpriteBatch spriteBatch;
        private readonly IInputProvider inputProvider;
        private readonly ICardProvider cardProvider;
        private readonly IResourceProvider resourceProvider;
        private readonly IThemeProvider themeProvider;
        private readonly IWeaponProvider weaponProvider;
        private readonly StateManager stateManager;
        private readonly GraphicsDevice graphicsDevice;
        private readonly GameServiceContainer serviceContainer;

        private readonly Minimap minimap;
        private readonly Rectangle minimapRectangle;
        private readonly Rectangle minimapInner;
        private readonly Rectangle screenRectangle;

        private DungeonLevel dungeon;

        private HealthArmourBar playerHealthArmourBar;
        private HealthArmourBar drillHealthArmourBar;
        private Label ammoLabel;

        private Label rubyLabel;
        private Label emeraldLabel;
        private Label diamondLabel;
        private Label keyLabel;
        private Label fpsLabel;
        private int keyBlinkDuration;

        private DepthStencilState barDepthStencilState;
        private DepthStencilState drillDepthStencilState;

        /// <summary>
        /// Initializes a new instance of the <see cref="SinglePlayer"/> class.
        /// </summary>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        public SinglePlayer(GameServiceContainer serviceContainer)
        {
            this.serviceContainer = serviceContainer;

            graphicsDevice = serviceContainer.GetService<IGraphicsDeviceService>().GraphicsDevice;
            spriteBatch = serviceContainer.GetService<ISpriteBatchService>().SpriteBatch;
            stateManager = serviceContainer.GetService<IStateManagerService>().StateManager;
            inputProvider = serviceContainer.GetService<IInputProvider>();
            cardProvider = serviceContainer.GetService<ICardProvider>();
            resourceProvider = serviceContainer.GetService<IResourceProvider>();
            themeProvider = serviceContainer.GetService<IThemeProvider>();
            weaponProvider = serviceContainer.GetService<IWeaponProvider>();

            serviceContainer.RemoveService(typeof(IPlayerProvider));
            serviceContainer.AddService<IPlayerProvider>(this);

            testCard = cardProvider.GetCard("testCard");

            GuiBase.Font = resourceProvider.GetFont("SmallFont");

            screenRectangle = new Rectangle(0, 0, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            Camera = new Camera
            {
                Zoom = 3f,
                Size = new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight)
            };

            minimap = new Minimap(this.serviceContainer);

            // minimap.Generate(graphicsDevice, dungeon.GetSearchMap());
            minimapRectangle = new Rectangle(graphicsDevice.PresentationParameters.BackBufferWidth - 280, graphicsDevice.PresentationParameters.BackBufferHeight - 280, 260, 260);
            minimapInner = new Rectangle(graphicsDevice.PresentationParameters.BackBufferWidth - 278, graphicsDevice.PresentationParameters.BackBufferHeight - 278, 256, 256);
        }

        /// <inheritdoc/>
        public override void Resume()
        {
            serviceContainer.RemoveService(typeof(IPlayerProvider));
            serviceContainer.AddService<IPlayerProvider>(this);
            base.Resume();
        }

        /// <inheritdoc/>
        public override void Start()
        {
            Paused = false;

            dungeon = new DungeonLevel(serviceContainer, themeProvider.GetTheme("temple"));
            dungeon.GenerateLevel(64, 64, 40);

            Player = new EntityPlayer(dungeon, this, serviceContainer);
            dungeon.AddEntity((EntityPlayer)Player);

            Drill = new EntityDrill(dungeon, serviceContainer, this);
            dungeon.AddEntity(Drill);

            /*
            EntityShootingSnake snake = new EntityShootingSnake(dungeon, resourceProvider, this, weaponProvider)
            {
                Body =
                {
                    Position = Player.Position + new Vector2(1, 1)
                }
            };

            dungeon.AddEntity(snake);
            */

            minimap.Generate(dungeon.GetMap());

            ammoLabel = new Label(this, "ammoLabel")
            {
                Position = new Vector2(20, graphicsDevice.PresentationParameters.BackBufferHeight - 115),
                Text = "Ammo: 100"
            };

            rubyLabel = new Label(this, "rubyLabel")
            {
                Position = new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth - 48, 19),
                Text = "0"
            };

            emeraldLabel = new Label(this, "emeraldLabel")
            {
                Position = new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth - 48, 59),
                Text = "0"
            };

            diamondLabel = new Label(this, "diamondLabel")
            {
                Position = new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth - 48, 99),
                Text = "0"
            };

            keyLabel = new Label(this, "keyLabel")
            {
                Position = new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth - 48, 142),
                Text = "0"
            };

            fpsLabel = new Label(this, "fpsLabel")
            {
                Position = new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth - 160, 185),
                Text = "16,66 ms"
            };

            playerHealthArmourBar = new HealthArmourBar(this, resourceProvider, "playerHealthArmourBar")
            {
                Armour = Player.Armour,
                ArmourColor = new Color(82, 141, 156),
                Color = Color.LightGray,
                Health = Player.Health,
                HealthColor = new Color(190, 74, 57),
                MaxArmour = EntityPlayer.MaxArmour,
                MaxHealth = EntityPlayer.MaxHealth,
                Position = new Vector2(30, 20),
                HealthWidth = 200,
                ArmourWidth = 144,
                ArmourOffset = new Vector2(-15, 20),
                HealthMagicValue = 0.17f,
                ArmourMagicValue = 0.23f
            };

            drillHealthArmourBar = new HealthArmourBar(this, resourceProvider, "drillHealthArmourBar", true)
            {
                Armour = Player.Armour,
                ArmourColor = new Color(82, 141, 186),
                Color = Color.LightGray,
                Health = Player.Health,
                HealthColor = new Color(190, 74, 57),
                MaxArmour = EntityDrill.MaxShield,
                MaxHealth = EntityDrill.MaxHealth,
                Position = new Vector2(675, 20),
                ArmourWidth = 280,
                HealthWidth = 250,
                ArmourOffset = new Vector2(-15, 35),
                HealthMagicValue = 0.19f,
                ArmourMagicValue = 0.17f
            };

            playerHealthArmourBar.ApplyChanges();
            drillHealthArmourBar.ApplyChanges();

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

            DepthStencilState beforeDrilDepthStencilState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 2
            };

            barDepthStencilState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Equal,
                ReferenceStencil = 1
            };

            drillDepthStencilState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Equal,
                ReferenceStencil = 2
            };

            using (spriteBatch.Use(SpriteSortMode.Deferred, null, null, beforeDepthStencilState, null, alphaTestEffect))
            {
                spriteBatch.Draw(resourceProvider.GetTexture("healthBG"), new Rectangle(36, 23, 200, 48), Color.White);
                spriteBatch.Draw(resourceProvider.GetTexture("healthBG"), new Rectangle(21, 43, 144, 48), Color.White);
            }

            using (spriteBatch.Use(SpriteSortMode.Deferred, null, null, beforeDrilDepthStencilState, null, alphaTestEffect))
            {
                spriteBatch.Draw(resourceProvider.GetTexture("healthBG"), new Rectangle(678, 22, 253, 48), Color.White);
                spriteBatch.Draw(resourceProvider.GetTexture("healthBG"), new Rectangle(662, 58, 284, 48), Color.White);
            }
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (Paused)
            {
                return;
            }

            DoCheck(gameTime, new Point(inputProvider.MouseState.X, inputProvider.MouseState.Y), inputProvider.LeftClicked);

            dungeon.Update(gameTime);

            Camera.Position = ConvertUnits.ToDisplayUnits(Player.Position);

            playerHealthArmourBar.Health = Player.Health;
            playerHealthArmourBar.Armour = Player.Armour;

            drillHealthArmourBar.Check(gameTime, Point.Zero, false);
            drillHealthArmourBar.Health = Drill.Health;
            drillHealthArmourBar.Armour = Drill.Shield;

            ammoLabel.Text = $"Ammo: {Player.Weapon.Ammo}";

            rubyLabel.Text = Player.Gems.RedGems.ToString();
            emeraldLabel.Text = Player.Gems.GreenGems.ToString();
            diamondLabel.Text = Player.Gems.BlueGems.ToString();

            fpsLabel.Text = $"{gameTime.ElapsedGameTime.Ticks / 10000f} ms";

            if (keyBlinkDuration > 0)
            {
                keyBlinkDuration--;

                if (keyBlinkDuration == 0)
                {
                    keyLabel.Color = Color.White;
                }
            }

            if (Player.Gems.Keys == -1)
            {
                keyBlinkDuration = 15;
                keyLabel.Color = Color.Red;
                Player.Gems.Keys = 0;
            }

            keyLabel.Text = Player.Gems.Keys.ToString();

            if (!Drilling && inputProvider.KeybindPressed("PauseGame"))
            {
                stateManager.Switch(stateManager.GameStates.First(state => state.GetType() == typeof(MainMenu)));
            }

            if (Drilling && (inputProvider.KeyPressed(Keys.Escape) || inputProvider.ButtonPressed(Buttons.B)))
            {
                Drilling = false;
            }
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, float interp)
        {
            graphicsDevice.Clear(ClearOptions.Target, Color.Black, 0f, 0);

            // GAME SPRITEBATCH
            dungeon.Draw(spriteBatch, Camera, graphicsDevice);

            // MINIMAP SPRITEBATCH
            using (spriteBatch.Use(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone))
            {
                minimap.Draw(spriteBatch, minimapRectangle, minimapInner, Camera);
            }

            using (spriteBatch.Use(SpriteSortMode.Deferred, null, null, barDepthStencilState))
            {
                spriteBatch.Draw(resourceProvider.GetTexture("healthBG"), graphicsDevice.Viewport.Bounds, Color.Black);
            }

            if (Vector2.Distance(Player.Position, Drill.Position) < 3)
            {
                using (spriteBatch.Use(SpriteSortMode.Deferred, null, null, drillDepthStencilState))
                {
                    spriteBatch.Draw(resourceProvider.GetTexture("healthBG"), graphicsDevice.Viewport.Bounds, Color.Black);
                }
            }

            // GUI SPRITEBATCH
            using (spriteBatch.Use(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone))
            {
                DoDraw(gameTime, spriteBatch, interp);

                if (Vector2.Distance(Player.Position, Drill.Position) < 3)
                {
                    drillHealthArmourBar.Draw(gameTime, spriteBatch, interp);
                }

                // CARDS
                if (Drilling)
                {
                    spriteBatch.Draw(resourceProvider.GetTexture("OneByOneEmpty"), screenRectangle, new Color(0, 0, 0, 150));

                    testCard.Draw(new Vector2(175, 247), 0.75f, gameTime, spriteBatch, interp);
                    testCard.Draw(new Vector2(650, 247), 0.75f, gameTime, spriteBatch, interp);
                    testCard.Draw(new Vector2(1075, 247), 0.75f, gameTime, spriteBatch, interp);
                }

                spriteBatch.Draw(resourceProvider.GetTexture("GemAnimation"), new Rectangle(graphicsDevice.PresentationParameters.BackBufferWidth - 88, 16, 32, 32), new Rectangle(64, 0, 16, 16), Color.White);
                spriteBatch.Draw(resourceProvider.GetTexture("EmeraldSheet"), new Rectangle(graphicsDevice.PresentationParameters.BackBufferWidth - 88, 56, 32, 32), new Rectangle(64, 0, 16, 16), Color.White);
                spriteBatch.Draw(resourceProvider.GetTexture("DiamondSheet"), new Rectangle(graphicsDevice.PresentationParameters.BackBufferWidth - 94, 88, 44, 44), new Rectangle(144, 0, 24, 24), Color.White);
                spriteBatch.Draw(resourceProvider.GetTexture("EntityKey"), new Rectangle(graphicsDevice.PresentationParameters.BackBufferWidth - 88, 136, 32, 32), Color.White);

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