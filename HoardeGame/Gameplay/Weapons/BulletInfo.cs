// <copyright file="BulletInfo.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.IO;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;

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
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        /// <param name="weaponId">ID of weapon to which this bullet belongs</param>
        public void Validate(GameServiceContainer serviceContainer, string weaponId)
        {
            if (serviceContainer.GetService<IResourceProvider>().GetTexture(Texture) == null)
            {
                throw new FileNotFoundException($"Texture {Texture} of bullet of weapon {weaponId} does not exist!");
            }
        }
    }
}