// <copyright file="Vector2Extensions.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using Microsoft.Xna.Framework;

namespace HoardeGame.Extensions
{
    /// <summary>
    /// Vector2 extension methods
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// Converts a <see cref="Vector2"/> to a <see cref="Point"/> (also rounds it)
        /// </summary>
        /// <param name="vec">Converted vector</param>
        /// <returns>Point</returns>
        public static Point ToPoint(this Vector2 vec)
        {
            return new Point((int)vec.X, (int)vec.Y);
        }
    }
}