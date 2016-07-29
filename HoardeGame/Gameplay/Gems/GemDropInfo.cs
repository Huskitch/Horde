// <copyright file="GemDropInfo.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;

namespace HoardeGame.Gameplay.Gems
{
    /// <summary>
    /// Definition of drops
    /// </summary>
    public class GemDropInfo
    {
        /// <summary>
        /// Gets or sets the minimum drop
        /// </summary>
        public GemInfo MinDrop { get; set; } = new GemInfo();

        /// <summary>
        /// Gets or sets the maximal drop
        /// </summary>
        public GemInfo MaxDrop { get; set; } = new GemInfo();

        private readonly Random random = new Random();

        /// <summary>
        /// Generates a random amount of red gems within min and max
        /// </summary>
        /// <returns>Radnom amount of red gems within min and max</returns>
        public int GetRandomRedGems()
        {
            return random.Next(MinDrop.RedGems, MaxDrop.RedGems + 1);
        }

        /// <summary>
        /// Generates a random amount of red gems within min and max
        /// </summary>
        /// <returns>Radnom amount of red gems within min and max</returns>
        public int GetRandomGreenGems()
        {
            return random.Next(MinDrop.GreenGems, MaxDrop.GreenGems + 1);
        }

        /// <summary>
        /// Generates a random amount of red gems within min and max
        /// </summary>
        /// <returns>Radnom amount of red gems within min and max</returns>
        public int GetRandomBlueGems()
        {
            return random.Next(MinDrop.BlueGems, MaxDrop.BlueGems + 1);
        }

        /// <summary>
        /// Generates a random amount of red gems within min and max
        /// </summary>
        /// <returns>Radnom amount of red gems within min and max</returns>
        public int GetRandomKeys()
        {
            return random.Next(MinDrop.Keys, MaxDrop.Keys + 1);
        }
    }
}