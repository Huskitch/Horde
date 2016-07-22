// <copyright file="RandomExtensions.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using Microsoft.Xna.Framework;

namespace HoardeGame.Extensions
{
    public static class RandomExtensions
    {
        public static Vector2 Vector2(this Random random, float minX, float minY, float maxX, float maxY)
        {
            float x = maxX - minX;
            x = x * (float)random.NextDouble();
            x += minX;

            float y = maxY - minY;
            y *= (float)random.NextDouble();
            y += minY;

            return new Vector2(x, y);
        }
    }
}