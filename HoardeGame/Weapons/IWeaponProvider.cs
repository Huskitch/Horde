// <copyright file="IWeaponProvider.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace HoardeGame.Weapons
{
    /// <summary>
    /// Definition of a <see cref="Weapon"/> provider
    /// </summary>
    public interface IWeaponProvider
    {
        /// <summary>
        /// Gets the list of <see cref="Weapon"/>s
        /// </summary>
        Dictionary<string, Weapon> Weapons { get; }

        /// <summary>
        /// Gets a <see cref="Weapon"/> with a specified name
        /// </summary>
        /// <param name="name">Name of the weapon</param>
        /// <returns><see cref="Weapon"/> or null</returns>
        Weapon GetWeapon(string name);
    }
}