// <copyright file="MenuDemo.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using FarseerPhysics;
using HoardeGame.Entities.Base;
using HoardeGame.Entities.Enemies;
using HoardeGame.Entities.Player;
using HoardeGame.Extensions;
using HoardeGame.Gameplay.Level;
using HoardeGame.Gameplay.Player;
using HoardeGame.Gameplay.Themes;
using HoardeGame.Graphics;
using HoardeGame.Resources;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GameStates
{
    /// <summary>
    /// Background state for main menu
    /// </summary>
    public class MenuDemo : GameState, IPlayerProvider
    {
        /// <summary>
        /// Gets the player entity
        /// In this case it is fake and controlled by AI
        /// </summary>
        public IPlayer Player { get; }

        private readonly SpriteBatch spriteBatch;
        private readonly GraphicsDevice graphicsDevice;
        private readonly IResourceProvider resourceProvider;
        private readonly GameServiceContainer serviceContainer;

        private readonly Camera camera;
        private readonly DungeonLevel dungeon;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuDemo"/> class.
        /// </summary>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        public MenuDemo(GameServiceContainer serviceContainer)
        {
            this.serviceContainer = serviceContainer;

            spriteBatch = serviceContainer.GetService<ISpriteBatchService>().SpriteBatch;
            resourceProvider = serviceContainer.GetService<IResourceProvider>();
            graphicsDevice = serviceContainer.GetService<IGraphicsDeviceService>().GraphicsDevice;

            serviceContainer.RemoveService(typeof(IPlayerProvider));
            serviceContainer.AddService<IPlayerProvider>(this);

            camera = new Camera
            {
                Zoom = 3f,
                Size = new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight)
            };

            dungeon = new DungeonLevel(serviceContainer, serviceContainer.GetService<IThemeProvider>().GetTheme("temple"), false);

            int[,] map = new int[18, 10];

            for (int x = 0; x < map.GetLength(0); x++)
            {
                map[x, 0] = 1;
                map[x, map.GetLength(1) - 1] = 1;
            }

            for (int y = 0; y < map.GetLength(1); y++)
            {
                map[0, y] = 1;
                map[map.GetLength(0) - 1, y] = 1;
            }

            dungeon.LoadLevel(map.GetLength(0), map.GetLength(1), map);

            Random random = new Random();

            Player = new EntityFakePlayer(dungeon, serviceContainer)
            {
                Body =
                {
                    Position = new Vector2(2, 5)
                }
            };

            dungeon.AddEntity((EntityBase)Player);

            for (int i = 0; i < 3; i++)
            {
                EntityBat bat = new EntityBat(dungeon, serviceContainer)
                {
                    Body =
                    {
                        Position = new Vector2(13, 5) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                    }
                };

                dungeon.AddEntity(bat);
            }

            for (int i = 0; i < 2; i++)
            {
                EntitySnake snake = new EntitySnake(dungeon, serviceContainer)
                {
                    Body =
                    {
                        Position = new Vector2(13, 5) + random.Vector2(-0.3f, -0.3f, 0.3f, 0.3f)
                    }
                };

                dungeon.AddEntity(snake);
            }

            camera.Position = ConvertUnits.ToDisplayUnits(new Vector2(9, 5));
        }

        /// <inheritdoc/>
        public override void Resume()
        {
            serviceContainer.RemoveService(typeof(IPlayerProvider));
            serviceContainer.AddService<IPlayerProvider>(this);
            base.Resume();
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            dungeon.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, float interpolation)
        {
            dungeon.Draw(spriteBatch, camera, graphicsDevice);
        }
    }
}