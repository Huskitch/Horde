// <copyright file="Camera.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Graphics.Rendering
{
    /// <summary>
    /// Transformation matrix generator used for rendering the <see cref="DungeonLevel"/>
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// Gets or sets the zoom of this <see cref="Camera"/>
        /// </summary>
        public float Zoom { get; set; }

        /// <summary>
        /// Gets or sets the position this <see cref="Camera"/>
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the rotation this <see cref="Camera"/>
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets the transformation matrix of this <see cref="Camera"/>
        /// </summary>
        public Matrix Transform { get; private set; }

        /// <summary>
        /// Move the <see cref="Camera"/>
        /// </summary>
        /// <param name="amount">Translation <see cref="Vector2"/></param>
        public void Translate(Vector2 amount)
        {
            Position += amount;
        }

        /// <summary>
        /// Lerp the <see cref="Camera"/> to a target by x amount
        /// </summary>
        /// <param name="target"><see cref="Vector2"/> to lerp to</param>
        /// <param name="x">Parameter of the lerp function</param>
        public void Focus(Vector2 target, float x)
        {
            Position = Vector2.Lerp(Position, target, x);
        }

        /// <summary>
        /// Creates a transformation matrix for this <see cref="Camera"/>
        /// </summary>
        /// <param name="graphicsDevice"><see cref="GraphicsDevice"/> to use for generating the transformation matrix</param>
        /// <returns><see cref="Camera"/> transformation matrix</returns>
        public Matrix Transformation(GraphicsDevice graphicsDevice)
        {
            Transform = Matrix.CreateTranslation(
                new Vector3(-Position.X, -Position.Y, 0)) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                        Matrix.CreateTranslation(new Vector3(
                            graphicsDevice.Viewport.Width * 0.5f,
                            graphicsDevice.Viewport.Height * 0.5f,
                            0));

            return Transform;
        }
    }
}