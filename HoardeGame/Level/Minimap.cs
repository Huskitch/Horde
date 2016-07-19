// <copyright file="Minimap.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.Graphics.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Level
{
    /// <summary>
    /// Minimap generator
    /// </summary>
    public class Minimap
    {
        private static readonly Color FloorColor = new Color(113, 88, 71);
        private static readonly Color WallColor = new Color(194, 137, 64);

        /// <summary>
        /// Gets the generated minimap to be drawn
        /// </summary>
        public Texture2D Image { get; private set; }

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
            Texture2D white = ResourceManager.GetTexture("OneByOneEmpty");
            spriteBatch.Draw(white, outer, Color.Gray);
            spriteBatch.Draw(Image, inner, Color.White);

            float relativeWidth = camera.Size.X / (Image.Width * 32) / 2;
            float relativeHeight = camera.Size.Y / (Image.Height * 32) / 2;
            relativeWidth = inner.Width * relativeWidth;
            relativeHeight = inner.Height * relativeHeight;

            float relativeX = camera.Position.X / (Image.Width * 32);
            float relativeY = camera.Position.Y / (Image.Height * 32);
            relativeX = inner.X + inner.Width * relativeX - relativeWidth / 2;
            relativeY = inner.Y + inner.Height * relativeY - relativeHeight / 2;

            spriteBatch.Draw(white, new Rectangle((int)relativeX + 1, (int)relativeY + 1, (int)relativeWidth, 2), Color.Black);
            spriteBatch.Draw(white, new Rectangle((int)relativeX + 1, (int)relativeY + 1, 2, (int)relativeHeight), Color.Black);
            spriteBatch.Draw(white, new Rectangle((int)relativeX + 1, (int)relativeY + (int)relativeHeight, (int)relativeWidth, 2), Color.Black);
            spriteBatch.Draw(white, new Rectangle((int)relativeX + (int)relativeWidth, (int)relativeY + 1, 2, (int)relativeHeight + 1), Color.Black);

            //spriteBatch.DrawString(ResourceManager.GetFont("BasicFont"), camera.Position.ToString(), new Vector2(20, 20), Color.White);
            //spriteBatch.DrawString(ResourceManager.GetFont("BasicFont"), (Image.Width*32).ToString(), new Vector2(20, 40), Color.White);
        }
    }
}