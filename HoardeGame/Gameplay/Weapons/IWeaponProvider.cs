// <copyright file="IWeaponProvider.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace HoardeGame.Gameplay.Weapons
{
    /// <summary>
    /// Definition of a <see cref="WeaponInfo"/> provider
    /// </summary>
    public interface IWeaponProvider
    {
        /// <summary>
        /// Gets the list of <see cref="WeaponInfo"/>s
        /// </summary>
        Dictionary<string, WeaponInfo> Weapons { get; }

        /// <summary>
        /// Gets a <see cref="WeaponInfo"/> with a specified name
        /// </summary>
        /// <param name="name">Name of the weapon</param>
        /// <returns><see cref="WeaponInfo"/> or null</returns>
        WeaponInfo GetWeapon(string name);

        /// <summary>
        /// Gets a random <see cref="WeaponInfo"/> with an optional filter
        /// </summary>
        /// <param name="type">Filter for types</param>
        /// <returns>A random <see cref="WeaponInfo"/> or null</returns>
        WeaponInfo GetRandomWeapon(WeaponType type);
    }
}