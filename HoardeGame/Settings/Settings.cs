// <copyright file="Settings.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

namespace HoardeGame.Settings
{
    /// <summary>
    /// Definition of settings
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Gets or sets the master volume of the game (0f - 1f)
        /// </summary>
        public float Volume { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether particle effects are enabled
        /// </summary>
        public bool ParticleEffects { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the game should be fullscreen or windowed
        /// </summary>
        public bool FullScreen { get; set; }
    }
}