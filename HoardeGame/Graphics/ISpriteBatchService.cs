// <copyright file="ISpriteBatchService.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Graphics
{
    /// <summary>
    /// Service providing access to SpriteBatch
    /// </summary>
    public interface ISpriteBatchService
    {
        /// <summary>
        /// Gets the spritebatch
        /// </summary>
        SpriteBatch SpriteBatch { get; }
    }
}