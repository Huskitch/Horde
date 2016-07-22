// <copyright file="ChestInfo.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

namespace HoardeGame.Themes
{
    /// <summary>
    /// Information about loot in chests
    /// </summary>
    public class ChestInfo
    {
        /// <summary>
        /// Defines ways the chest can decide which wepaons to spawn
        /// </summary>
        public enum WeaponChance
        {
            /// <summary>
            /// No weapon will spawn
            /// </summary>
            None,

            /// <summary>
            /// A random weapon will spawn
            /// </summary>
            Random,

            /// <summary>
            /// A weapon specified in the <see cref="ChestInfo.WeaponType"/> will spawn
            /// </summary>
            Specific
        }

        /// <summary>
        /// Gets or sets the minimal amount of ammo the chest will contain
        /// </summary>
        public int MinAmmo { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of ammo the chest will contain
        /// </summary>
        public int MaxAmmo { get; set; }

        /// <summary>
        /// Gets or sets the minimal amount of health the chest will contain
        /// </summary>
        public int MinHealth { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of health the chest will contain
        /// </summary>
        public int MaxHealth { get; set; }

        /// <summary>
        /// Gets or sets the minimal amount of armour the chest will contain
        /// </summary>
        public int MinArmour { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of armour the chest will contain
        /// </summary>
        public int MaxArmour { get; set; }

        /// <summary>
        /// Gets or sets the minimal amount of gems the chest will contain
        /// </summary>
        public int[] MinGems { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of gems the chest will contain
        /// </summary>
        public int[] MaxGems { get; set; }

        /// <summary>
        /// Gets or sets the specific weapon type the weapon will contain in case the <see cref="WeaponDropType"/> is set to <see cref="WeaponChance.Specific"/>
        /// </summary>
        public string WeaponType { get; set; }

        /// <summary>
        /// Gets or sets the chance that any weapon will drop in percent
        /// </summary>
        public int WeaponDropChance { get; set; }

        /// <summary>
        /// Gets or sets the style how the chest will pick which weapons to spawn
        /// </summary>
        public WeaponChance WeaponDropType { get; set; }
    }
}