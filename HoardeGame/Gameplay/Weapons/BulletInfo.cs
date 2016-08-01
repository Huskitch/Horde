// <copyright file="BulletInfo.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.IO;
using HoardeGame.Resources;

namespace HoardeGame.Gameplay.Weapons
{
    /// <summary>
    /// Information about a bullet type
    /// </summary>
     public class BulletInfo
    {
        /// <summary>
        /// Gets or sets the texture of the bullet
        /// </summary>
        public string Texture { get; set; }

        /// <summary>
        /// Gets or sets the damage of the bullet
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Gets or sets the firing delay
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// Gets or sets the speed of the bullet
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the offset of the bullet
        /// </summary>
        public float Offset { get; set; }

        /// <summary>
        /// Gets or sets the amount of bullets that spawn per shot
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the life time of this bullet in ms
        /// </summary>
        public float Lifetime { get; set; }

        /// <summary>
        /// Gets or sets the spread of this bullet in deg
        /// </summary>
        public int Spread { get; set; }

        /// <summary>
        /// Validates the bukket
        /// </summary>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for validating the texture</param>
        public void Validate(IResourceProvider resourceProvider)
        {
            if (resourceProvider.GetTexture(Texture) == null)
            {
                throw new FileNotFoundException($"Texture {Texture} does not exist!");
            }
        }
    }
}