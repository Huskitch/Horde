// <copyright file="MenuDemo.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.Entities;
using HoardeGame.Extensions;
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
    public class MenuDemo : GameState, IPlayerProvider
    {
        private readonly SpriteBatch spriteBatch;
        private readonly IResourceProvider resourceProvider;

        private readonly Camera camera;
        private readonly DungeonLevel dungeon;

        public MenuDemo(SpriteBatch spriteBatch, IResourceProvider resourceProvider, GraphicsDevice graphicsDevice, IInputProvider inputProvider, IThemeProvider themeProvider)
        {
            this.spriteBatch = spriteBatch;
            this.resourceProvider = resourceProvider;

            camera = new Camera
            {
                Zoom = 2.6f,
                Size = new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight)
            };

            dungeon = new DungeonLevel(resourceProvider, this, inputProvider, themeProvider.GetTheme("temple"));
            dungeon.GenerateLevel(64, 64, 40);
        }

        public override void Draw(GameTime gameTime, float interpolation)
        {
            using (spriteBatch.Use())
            {
                spriteBatch.Draw(resourceProvider.GetTexture("DiamondSheet"), new Vector2(100, 100), Color.White);
            }
        }
    }
}