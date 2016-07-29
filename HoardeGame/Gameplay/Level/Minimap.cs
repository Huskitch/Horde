// <copyright file="Minimap.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using HoardeGame.Graphics;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Gameplay.Level
{
    /// <summary>
    /// Minimap generator
    /// </summary>
    public class Minimap
    {
        private static readonly Color FloorColor = new Color(0, 0, 0, 50);
        private static readonly Color WallColor = new Color(194, 137, 64);
        private static readonly Color CustomColor = new Color(255, 0, 255);

        /// <summary>
        /// Gets the generated minimap to be drawn
        /// </summary>
        public Texture2D Image { get; private set; }

        private readonly IResourceProvider resourceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Minimap"/> class.
        /// </summary>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        public Minimap(IResourceProvider resourceProvider)
        {
            this.resourceProvider = resourceProvider;
        }

        /// <summary>
        /// Generates a new minimap <see cref="Texture2D"/>
        /// </summary>
        /// <param name="device"><see cref="GraphicsDevice"/> used for creating the <see cref="Texture2D"/></param>
        /// <param name="map">Data to generate the <see cref="Texture2D"/> from</param>
        public void Generate(GraphicsDevice device, int[,] map)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            Color[] colorData = new Color[width * height];
            for (int i = 0; i < colorData.Length; i++)
            {
                if (map[i % width, i / width] == 1)
                {
                    colorData[i] = WallColor;
                }
                else if (map[i % width, i / width] == 2)
                {
                    colorData[i] = CustomColor;
                }
                else
                {
                    colorData[i] = FloorColor;
                }
            }

            Image?.Dispose();
            Image = new Texture2D(device, width, height);
            Image.SetData(colorData);
        }

        /// <summary>
        /// Draw the minimap
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/></param>
        /// <param name="outer">Outer dimensions of the border</param>
        /// <param name="inner">Inner dimensions of the minimap</param>
        /// <param name="camera">Camera to draw outline</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle outer, Rectangle inner, Camera camera)
        {
            Texture2D white = resourceProvider.GetTexture("OneByOneEmpty");

            // spriteBatch.Draw(white, outer, Color.Gray);
            float relativeWidth = camera.Size.X / Image.Width;
            float relativeHeight = camera.Size.Y / Image.Height;
            float relativeX = camera.Position.X / (Image.Width / 2);
            float relativeY = camera.Position.Y / (Image.Height / 2);

            float max = Math.Max(relativeWidth, relativeHeight);
            spriteBatch.Draw(Image, inner, new Rectangle((int)relativeX - (int)(max / 2), (int)relativeY - (int)(max / 2), (int)max, (int)max), Color.White);
            spriteBatch.Draw(white, new Rectangle(5 + inner.X + inner.Width / 2, 5 + inner.Y + inner.Height / 2, 10, 10), Color.Black);
        }
    }
}