// <copyright file="WeaponInfo.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;

namespace HoardeGame.Gameplay.Weapons
{
    /// <summary>
    /// Definition of weapon types
    /// </summary>
    [Flags]
    public enum WeaponType
    {
        /// <summary>
        /// All pistol-like weapons
        /// </summary>
        Pistol,

        /// <summary>
        /// All shotgun-like weapons
        /// </summary>
        Shotgun,

        /// <summary>
        /// All weapons which use fire
        /// </summary>
        Fire,

        /// <summary>
        /// Matches all weapons
        /// </summary>
        Any
    }

    /// <summary>
    /// Definition of a weapon
    /// </summary>
    public class WeaponInfo
    {
        /// <summary>
        /// Gets or sets the readable name of the weapon
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ID of the weapon
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the weapon texture
        /// </summary>
        public string Texture { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this weapon has a laser pointer
        /// </summary>
        public bool HasLaserPointer { get; set; }

        /// <summary>
        /// Gets or sets the list of fireable bullets
        /// </summary>
        public List<BulletInfo> Bullets { get; set; }

        /// <summary>
        /// Gets or sets the weapon type
        /// </summary>
        public WeaponType WeaponType { get; set; }

        /// <summary>
        /// Validates the weapon
        /// </summary>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        public void Validate(GameServiceContainer serviceContainer)
        {
            var resourceProvider = serviceContainer.GetService<IResourceProvider>();

            if (resourceProvider.GetTexture(Texture) == null)
            {
                throw new FileNotFoundException($"Texture {Texture} of weapon {Id} does not exist!");
            }

            if (Bullets == null)
            {
                throw new NullReferenceException($"Bullets for weapon {Id} cannot be null!");
            }

            foreach (var bullet in Bullets)
            {
                bullet.Validate(serviceContainer, Id);
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Name} ({Id})";
        }
    }
}