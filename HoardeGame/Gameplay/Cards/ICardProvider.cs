// <copyright file="ICardProvider.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

namespace HoardeGame.Gameplay.Cards
{
    /// <summary>
    /// Defines a provider for managing cards
    /// </summary>
    public interface ICardProvider
    {
        /// <summary>
        /// Gets a card
        /// </summary>
        /// <param name="key">Name of the card</param>
        /// <returns>Found card or <c>null</c></returns>
        Card GetCard(string key);
    }
}