// <copyright file="IDrawable.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Graphics.Rendering
{
    public interface IDrawable
    {
        Vector2 Position { get; }

        Vector2 ScreenPosition { get; }

        float Depth { get; }

        void Draw(SpriteBatch spriteBatch, EffectParameter parameter);
    }
}