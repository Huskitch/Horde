// <copyright file="RandomExtensions.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using Microsoft.Xna.Framework;

namespace HoardeGame.Extensions
{
    /// <summary>
    /// Extensions for <see cref="Random"/>
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Generates a random <see cref="Microsoft.Xna.Framework.Vector2"/>
        /// </summary>
        /// <param name="random"><see cref="Random"/></param>
        /// <param name="minX">Minimal X coordinate</param>
        /// <param name="minY">Minimal Y coordinate</param>
        /// <param name="maxX">Maximum X cooridnate</param>
        /// <param name="maxY">Maximum Y coordinates</param>
        /// <returns>Random <see cref="Microsoft.Xna.Framework.Vector2"/></returns>
        public static Vector2 NextVector2(this Random random, float minX, float minY, float maxX, float maxY)
        {
            float x = maxX - minX;
            x = x * (float)random.NextDouble();
            x += minX;

            float y = maxY - minY;
            y *= (float)random.NextDouble();
            y += minY;

            return new Vector2(x, y);
        }

        /// <summary>
        /// Generates a random float
        /// </summary>
        /// <param name="random"><see cref="Random"/></param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns>Random float</returns>
        public static float NextFloat(this Random random, float min, float max)
        {
            float diff = max - min;
            diff = diff * (float)random.NextDouble();
            return diff + min;
        }

        /// <summary>
        /// Generates a random <see cref="Microsoft.Xna.Framework.Color"/>
        /// </summary>
        /// <param name="random"><see cref="Random"/></param>
        /// <param name="alpha">Alpha of the <see cref="Microsoft.Xna.Framework.Color"/></param>
        /// <returns>Random <see cref="Microsoft.Xna.Framework.Color"/></returns>
        public static Color NextColor(this Random random, int alpha = 255)
        {
            return new Color(random.Next(256), random.Next(256), random.Next(256), alpha);
        }
    }
}