// <copyright file="BulletInfo.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.Entities;

namespace HoardeGame.Gameplay
{
    /// <summary>
    /// List of all factions
    /// </summary>
    public enum Faction
    {
        /// <summary>
        /// Player and friendly entities
        /// </summary>
        Player,

        /// <summary>
        /// Enemy entities
        /// </summary>
        Enemies
    }

    /// <summary>
    /// Info about a bullet
    /// </summary>
    public class BulletInfo
    {
        /// <summary>
        /// Gets the faction that fired this bullet
        /// </summary>
        public Faction Faction { get; }

        /// <summary>
        /// Gets the weapon that shot this bullet
        /// </summary>
        public EntityWeapon Weapon { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BulletInfo"/> class.
        /// </summary>
        /// <param name="faction"><see cref="Faction"/> that fired this bullet</param>
        /// <param name="weapon"><see cref="EntityWeapon"/> that fired this bullet</param>
        public BulletInfo(Faction faction, EntityWeapon weapon)
        {
            Faction = faction;
            Weapon = weapon;
        }
    }
}