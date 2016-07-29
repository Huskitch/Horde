// <copyright file="Weapon.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using HoardeGame.Entities.Misc;
using HoardeGame.Resources;

namespace HoardeGame.Gameplay.Weapons
{
    /// <summary>
    /// Definition of a weapon
    /// </summary>
    public class Weapon
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
        /// Gets or sets the list bullets?
        /// </summary>
        public List<BulletInfo> Bullets { get; set; }

        /// <summary>
        /// Validates the weapon
        /// </summary>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for checking assets</param>
        public void Validate(IResourceProvider resourceProvider)
        {
            if (resourceProvider.GetTexture(Texture) == null)
            {
                throw new FileNotFoundException($"Texture {Texture} does not exist!");
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Name} ({Id})";
        }
    }
}