// <copyright file="ParticleSystem.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.Generic;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HoardeGame.Settings;

namespace HoardeGame.Graphics.ParticleSystem
{
    /// <summary>
    /// A particle system
    /// </summary>
    public class ParticleSystem : IDrawable
    {
        /// <summary>
        /// Gets the list of particles
        /// </summary>
        public List<Particle> Particles { get; } = new List<Particle>();

        /// <summary>
        /// Gets or sets the maximum amount of particles
        /// </summary>
        public int MaxParticles { get; set; } = 50;

        /// <summary>
        /// Gets or sets the offset for the particles
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <inheritdoc/>
        public Vector2 Position => parent.Position;

        /// <inheritdoc/>
        public Vector2 ScreenPosition => parent.ScreenPosition;

        /// <inheritdoc/>
        public float Depth => parent.Depth;

        private SpriteBatch spriteBatch;
        private IDrawable parent;
        private Texture2D texture;
        private Settings.Settings settings;

        /// <summary>
        /// Adds a particle
        /// </summary>
        /// <param name="particle">Particle to add</param>
        public void AddParticle(Particle particle)
        {
            if (!settings.ParticleEffects)
            {
                return;
            }

            if (Particles.Count == MaxParticles)
            {
                Particles.RemoveAt(0);
            }

            Particles.Add(particle);
        }

        /// <summary>
        /// Draws all the particles
        /// </summary>
        /// <param name="param">Does nothing</param>
        public void Draw(EffectParameter param)
        {
            if (!settings.ParticleEffects)
            {
                return;
            }

            foreach (var particle in Particles)
            {
                spriteBatch.Draw(texture, particle.Position + ScreenPosition + Offset, null, null, null, 0, new Vector2(particle.Size), particle.Color);
            }
        }

        /// <summary>
        /// Updates the state of this particle system
        /// </summary>
        /// <param name="time"><see cref="GameTime"/></param>
        public void Update(GameTime time)
        {
            if (!settings.ParticleEffects)
            {
                Particles.Clear();
                return;
            }

            Particles.RemoveAll(particle => particle.IsDead);

            foreach (var particle in Particles)
            {
                particle.Position += particle.Velocity;
                particle.Age += time.ElapsedGameTime.TotalMilliseconds;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleSystem"/> class.
        /// </summary>
        /// <param name="parent">Parent drawable of this system</param>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        public ParticleSystem(IDrawable parent, GameServiceContainer serviceContainer)
        {
            spriteBatch = serviceContainer.GetService<ISpriteBatchService>().SpriteBatch;
            texture = serviceContainer.GetService<IResourceProvider>().GetTexture("OneByOneEmpty");
            settings = serviceContainer.GetService<ISettingsService>().Settings;
            this.parent = parent;
        }
    }
}