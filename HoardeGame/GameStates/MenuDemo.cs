// <copyright file="MenuDemo.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics;
using HoardeGame.Entities;
using HoardeGame.Extensions;
using HoardeGame.Gameplay;
using HoardeGame.Graphics.Rendering;
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
        private readonly IResourceProvider resourceProvider;

        private readonly Camera camera;
        private readonly DungeonLevel dungeon;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuDemo"/> class.
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> to draw with</param>
        /// <param name="graphicsDevice"><see cref="GraphicsDevice"/> to draw with</param>
        /// <param name="inputProvider"><see cref="IInputProvider"/> to use for input</param>
        /// <param name="themeProvider"><see cref="IThemeProvider"/> for managing level themes</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        public MenuDemo(SpriteBatch spriteBatch, IResourceProvider resourceProvider, GraphicsDevice graphicsDevice, IInputProvider inputProvider, IThemeProvider themeProvider)
        {
            this.spriteBatch = spriteBatch;
            this.resourceProvider = resourceProvider;

            camera = new Camera
            {
                Zoom = 3f,
                Size = new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight)
            };

            dungeon = new DungeonLevel(resourceProvider, this, inputProvider, themeProvider.GetTheme("temple"));
            dungeon.GenerateLevel(64, 64, 40);

            Player = new EntityFakePlayer(dungeon, resourceProvider);
            dungeon.AddEntity((EntityBase)Player);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            dungeon.Update(gameTime);
            camera.Position = ConvertUnits.ToDisplayUnits(Player.Position);
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, float interpolation)
        {
            using (spriteBatch.Use())
            {
                spriteBatch.Draw(resourceProvider.GetTexture("DiamondSheet"), new Vector2(100, 100), Color.White);
            }
        }
    }
}