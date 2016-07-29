// <copyright file="IDrawable.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Graphics
{
    /// <summary>
    /// Defines a drawable object
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Gets the simulation position of the object (in meters)
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Gets the screen position of the object (in pixels)
        /// </summary>
        Vector2 ScreenPosition { get; }

        /// <summary>
        /// Gets the depth of the object
        /// This is used for sorting the objects
        /// </summary>
        float Depth { get; }

        /// <summary>
        /// Draws the object
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> to use for drawing</param>
        /// <param name="parameter"><see cref="EffectParameter"/> for flashing effect</param>
        void Draw(SpriteBatch spriteBatch, EffectParameter parameter);
    }
}