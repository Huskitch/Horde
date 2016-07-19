// <copyright file="SpriteBatchExtensions.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Extensions
{
    /// <summary>
    /// Spritebatch extension methods
    /// </summary>
    public static class SpriteBatchExtensions
    {
        /// <summary>
        ///     Draw a line between two points
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/></param>
        /// <param name="begin">Start position</param>
        /// <param name="end">End position</param>
        /// <param name="color">Color of the line</param>
        /// <param name="width">Width of line in pixels</param>
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y)
            {
                angle = MathHelper.TwoPi - angle;
            }

            spriteBatch.Draw(ResourceManager.GetTexture("OneByOneEmpty"), r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Helper method to use <see cref="SpriteBatch"/> with <see langword="using" />
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> to use</param>
        /// <returns>Disposable <see cref="SpriteBatch"/></returns>
        public static UsedSpriteBatch Use(this SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            return new UsedSpriteBatch(spriteBatch);
        }

        /// <summary>
        /// Helper method to use <see cref="SpriteBatch"/> with <see langword="using" />
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> to use</param>
        /// <param name="sortMode">SortMode</param>
        /// <param name="blendState">BlendState</param>
        /// <param name="samplerState">SamplerState</param>
        /// <param name="depthStencilState">DepthStencilState</param>
        /// <param name="rasterizerState">RasterizerState</param>
        /// <param name="effect">Effect</param>
        /// <param name="transformMatrix">TransformMatrix</param>
        /// <returns>Disposable <see cref="SpriteBatch"/></returns>
        public static UsedSpriteBatch Use(this SpriteBatch spriteBatch, SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
        {
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
            return new UsedSpriteBatch(spriteBatch);
        }
    }

    /// <summary>
    /// Helper class for SpriteBatch.Use
    /// </summary>
    public struct UsedSpriteBatch : IDisposable
    {
        /// <summary>
        ///     Gets spritebatch to end
        /// </summary>
        public SpriteBatch SpriteBatch { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsedSpriteBatch"/> struct.
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/></param>
        public UsedSpriteBatch(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            SpriteBatch.End();
        }
    }
}