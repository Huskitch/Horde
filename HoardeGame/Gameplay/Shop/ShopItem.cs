// <copyright file="ShopItem.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.Gameplay.Gems;
using HoardeGame.Gameplay.Player;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Gameplay.Shop
{
    /// <summary>
    /// Definition of an item sold by the shop
    /// </summary>
    public class ShopItem
    {
        /// <summary>
        /// Delegate for buying items
        /// </summary>
        /// <param name="player"><see cref="IPlayer"/> the player entity</param>
        /// <returns>Whether the transaction was successful</returns>
        public delegate bool BuyDelegate(IPlayer player);

        /// <summary>
        /// Gets or sets the name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the  price of the item
        /// </summary>
        public GemInfo Price { get; set; }

        /// <summary>
        /// Gets or sets the action that should be executed when this item is bought
        /// </summary>
        public BuyDelegate OnBought { get; set; }

        /// <summary>
        /// Gets or sets the icon for this item
        /// </summary>
        public Texture2D Icon { get; set; }
    }
}