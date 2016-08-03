// <copyright file="Theme.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using HoardeGame.Resources;

namespace HoardeGame.Gameplay.Themes
{
    /// <summary>
    /// Definition of a level theme
    /// </summary>
    public class Theme
    {
        /// <summary>
        /// Gets or sets the readable name of the theme
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ID of the theme
        /// </summary>
        [XmlAttribute]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the floor texture
        /// </summary>
        public string FloorTextureName { get; set; }

        /// <summary>
        /// Gets or sets the name of the wall texture
        /// </summary>
        public string WallTextureName { get; set; }

        /// <summary>
        /// Gets or sets the special argument passed to the worldgen
        /// </summary>
        public string Special { get; set; }

        /// <summary>
        /// Gets or sets the list of enemies that can spawn in this theme
        /// </summary>
        public List<EntitySpawnInfo> EntitySpawns { get; set; }

        /// <summary>
        /// Gets or sets the info about the loot on this level
        /// </summary>
        public ChestInfo ChestInfo { get; set; }

        /// <summary>
        /// Gets or sets the list of songs that plays in this theme
        /// </summary>
        public List<string> Songs { get; set; }

        /// <summary>
        /// Validates the theme
        /// </summary>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for checking assets</param>
        public void Validate(IResourceProvider resourceProvider)
        {
            if (resourceProvider.GetTexture(FloorTextureName) == null)
            {
                throw new FileNotFoundException($"Texture {FloorTextureName} of theme {ID} does not exist!");
            }

            if (resourceProvider.GetTexture(WallTextureName) == null)
            {
                throw new FileNotFoundException($"Texture {FloorTextureName} of theme {ID} does not exist!");
            }

            foreach (var song in Songs)
            {
                if (resourceProvider.GetSong(song) == null)
                {
                    throw new FileNotFoundException($"Song {song} in theme {ID} does not exist!");
                }
            }

            foreach (var spawn in EntitySpawns)
            {
                if (Type.GetType(spawn.EntityType) == null)
                {
                    throw new ArgumentException($"Entity {spawn.EntityType} in theme {ID} is not valid!", nameof(spawn.EntityType));
                }
            }

            if (ChestInfo.WeaponDropType == ChestInfo.WeaponChance.Specific && Type.GetType(ChestInfo.WeaponType) == null)
            {
                throw new ArgumentException($"Entity {ChestInfo.WeaponType} in theme {ID} is not valid!", nameof(ChestInfo.WeaponType));
            }
        }
    }
}