// <copyright file="Minimap.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Level
{
    /// <summary>
    /// Generator of minimaps
    /// </summary>
    internal static class Minimap
    {
        /// <summary>
        /// Gets the currently generated minimap
        /// </summary>
        public static Texture2D CurrentMinimap { get; private set; }

        private static readonly Color FloorColor = new Color(113, 88, 71);
        private static readonly Color WallColor = new Color(194, 137, 64);

        /// <summary>
        /// Generates a new minimap <see cref="Texture2D"/>
        /// </summary>
        /// <param name="device"><see cref="GraphicsDevice"/> used for creating the <see cref="Texture2D"/></param>
        /// <param name="map">Data to generate the <see cref="Texture2D"/> from</param>
        /// <returns><see cref="Texture2D"/> of the minimap</returns>
        public static Texture2D GenerateMinimap(GraphicsDevice device, int[,] map)
        {
            CurrentMinimap?.Dispose();

            int width = map.GetLength(0);
            int height = map.GetLength(1);

            Texture2D minimap = new Texture2D(device, width, height);

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

            minimap.SetData(colorData);

            CurrentMinimap = minimap;
            return minimap;
        }
    }
}