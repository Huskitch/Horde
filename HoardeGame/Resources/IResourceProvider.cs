// <copyright file="IResourceProvider.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace HoardeGame.Resources
{
    /// <summary>
    /// Defines a provider for game assets
    /// </summary>
    public interface IResourceProvider
    {
        /// <summary>
        /// Gets a <see cref="Texture2D"/> with provided key
        /// </summary>
        /// <param name="key">Name of the <see cref="Texture2D"/></param>
        /// <returns><see cref="Texture2D"/> with matching key or null</returns>
        Texture2D GetTexture(string key);

        /// <summary>
        /// Gets a <see cref="SpriteFont"/> with provided key
        /// </summary>
        /// <param name="key">Name of the <see cref="SpriteFont"/></param>
        /// <returns><see cref="SpriteFont"/> with matching key or null</returns>
        SpriteFont GetFont(string key);

        /// <summary>
        /// Gets a <see cref="SoundEffect"/> with provided key
        /// </summary>
        /// <param name="key">Name of the <see cref="SoundEffect"/></param>
        /// <returns><see cref="SoundEffect"/> with matching key or null</returns>
        SoundEffect GetSoundEffect(string key);

        /// <summary>
        /// Gets a <see cref="Song"/> with provided key
        /// </summary>
        /// <param name="key">Name of the <see cref="Song"/></param>
        /// <returns><see cref="Song"/> with matching key or null</returns>
        Song GetSong(string key);
    }
}