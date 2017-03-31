// <copyright file="Particle.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using Microsoft.Xna.Framework;

namespace HoardeGame.Graphics.ParticleSystem
{
    /// <summary>
    /// Single particle for <see cref="ParticleSystem"/>
    /// </summary>
    public class Particle
    {
        /// <summary>
        /// Gets or sets the position of this particle
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the velocity of this particle
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Gets or sets the color of this particle
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the size of this particle
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// Gets or sets the lifetime of this particle in ms
        /// </summary>
        public double Lifetime { get; set; }

        /// <summary>
        /// Gets or sets the age of this particle in ms
        /// </summary>
        public double Age { get; set; }

        /// <summary>
        /// Gets a value indicating whether this particle is dead
        /// </summary>
        public bool IsDead
        {
            get
            {
                return Age >= Lifetime;
            }
        }
    }
}