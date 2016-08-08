// <copyright file="Consumeable.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.Gameplay.Player;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Gameplay.Consumeables
{
    /// <summary>
    /// A consumeable
    /// </summary>
    public class Consumeable
    {
        /// <summary>
        /// Delegate definining an action that will be called when the player uses an item
        /// </summary>
        /// <param name="player"><see cref="IPlayer"/></param>
        public delegate void UseDelegate(IPlayer player);

        /// <summary>
        /// Gets or sets the name of this item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the texture of this item
        /// </summary>
        public string Texture { get; set; }

        /// <summary>
        /// Gets or sets the action that should be executed when the player uses this item
        /// </summary>
        public UseDelegate OnUse { get; set; }
    }
}