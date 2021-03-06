﻿// <copyright file="IPlayer.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using FarseerPhysics.Dynamics;
using HoardeGame.Entities.Misc;
using HoardeGame.Gameplay.Consumeables;
using HoardeGame.Gameplay.Gems;
using HoardeGame.Gameplay.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace HoardeGame.Gameplay.Player
{
    /// <summary>
    /// Definition of a player
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Gets the position of the player in meters
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Gets or sets the armour of the player
        /// </summary>
        int Armour { get; set; }

        /// <summary>
        /// Gets or sets the health of the player
        /// </summary>
        int Health { get; set; }

        /// <summary>
        /// Gets the amount of gems the player has
        /// </summary>
        GemInfo Gems { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the player is dead
        /// Can't just remove the entity because that would break everything
        /// </summary>
        bool Dead { get; set; }

        /// <summary>
        /// Gets or sets the weapon of the player
        /// </summary>
        EntityWeapon Weapon { get; set; }

        /// <summary>
        /// Gets or sets the weapons in the invenory of the player
        /// </summary>
        EntityWeapon[] InventoryWeapons { get; set; }

        /// <summary>
        /// Gets or sets the items in the inventory of the player
        /// </summary>
        Consumeable[] InventoryItems { get; set; }

        /// <summary>
        /// Gets the physics body of the player
        /// </summary>
        Body Body { get; }

        /// <summary>
        /// Gets the player audio listener
        /// </summary>
        AudioListener Listener { get; }

        /// <summary>
        /// Determines whether the played being hit
        /// </summary>
        /// <returns>Whether the played being hit</returns>
        bool IsHit();

        /// <summary>
        /// Damages the player
        /// </summary>
        /// <param name="damage">Amount of damage</param>
        void Damage(int damage);

        /// <summary>
        /// Adds an weapon to player's inventory as the inactive weapon
        /// </summary>
        /// <param name="weapon"><see cref="EntityWeapon"/> weapon to add</param>
        void AddWeapon(EntityWeapon weapon);

        /// <summary>
        /// Adds an item to the player's inventory
        /// </summary>
        /// <param name="consumeable"><see cref="Consumeable"/> item to add</param>
        /// <returns>Whether the item was added or not</returns>
        bool AddItem(Consumeable consumeable);
    }
}