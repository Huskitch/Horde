// <copyright file="Slider.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using HoardeGame.Input;
using HoardeGame.Resources;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoardeGame.GUI
{
    /// <summary>
    /// Slider control
    /// </summary>
    public class Slider : GuiBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether text should be drawn in the slider
        /// </summary>
        public bool DrawText { get; set; } = false;

        /// <summary>
        /// Gets or sets the progress (0f - 1f)
        /// </summary>
        public float Progress { get; set; }

        /// <summary>
        /// Gets or sets the slider colour
        /// </summary>
        public Color SliderColor { get; set; } = Color.LightGray;

        /// <summary>
        /// Gets or sets the colour of the active part of the slider
        /// </summary>
        public Color SliderColorActive { get; set; } = Color.Lime;

        /// <summary>
        /// Gets or sets the suffix after the value (% -> 100%)
        /// </summary>
        public string TextAppend { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the colour of the text
        /// </summary>
        public Color TextColor { get; set; } = Color.White;

        private readonly Texture2D sliderTexture;
        private readonly IInputProvider inputProvider;

        private Rectangle hitBox;
        private Rectangle slider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Slider"/> class.
        /// </summary>
        /// <param name="state"><see cref="GameState"/> to which this progress bar belongs</param>
        /// <param name="id">ID of this control for debugging</param>
        /// <param name="noSub">Whether to subscribe to events in gamestate</param>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> to resolve DI</param>
        public Slider(GameState state, GameServiceContainer serviceContainer, string id, bool noSub = false) : base(state, id, noSub)
        {
            sliderTexture = serviceContainer.GetService<IResourceProvider>().GetTexture("OneByOneEmpty");
            inputProvider = serviceContainer.GetService<IInputProvider>();
        }

        /// <summary>
        /// Gets or sets the position of the slider
        /// Setting this property will also recalculate the hitbox
        /// </summary>
        public new Vector2 Position
        {
            get
            {
                return new Vector2(slider.X, slider.Y);
            }

            set
            {
                slider.X = (int)value.X;
                slider.Y = (int)value.Y;
                slider = new Rectangle((int)value.X, (int)value.Y, slider.Width, 10);
                hitBox = new Rectangle((int)value.X - 10, (int)value.Y, slider.Width + 20, 10);
            }
        }

        /// <summary>
        /// Gets or sets the width of the slider
        /// Setting this property will also recalculate the hitbox
        /// </summary>
        public int Width
        {
            get
            {
                return slider.Width;
            }

            set
            {
                slider.Width = value;
                hitBox.Width = slider.Width + 20;
            }
        }

        /// <inheritdoc/>
        public override void Check(GameTime gameTime, Point point, bool click)
        {
            if (Visibility == HiddenState.Hidden | Visibility == HiddenState.Disabled)
            {
                return;
            }

            if (!hitBox.Contains(point))
            {
                return;
            }

            OnMouseOver?.Invoke();

            if (inputProvider.MouseState.LeftButton != ButtonState.Pressed)
            {
                return;
            }

            OnClick?.Invoke();

            Progress = (point.X - slider.X) / (float)slider.Width;

            Progress = MathHelper.Clamp(Progress, 0f, 1f);
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
            if (Visibility == HiddenState.Hidden)
            {
                return;
            }

            spriteBatch.Draw(sliderTexture, new Vector2(slider.X, slider.Y), null, SliderColor, 0, new Vector2(0, 0), new Vector2(slider.Width, 10), SpriteEffects.None, 0);
            spriteBatch.Draw(sliderTexture, new Vector2(slider.X, slider.Y), null, SliderColorActive, 0, new Vector2(0, 0), new Vector2(slider.Width * Progress, 10), SpriteEffects.None, 0);

            if (DrawText)
            {
                spriteBatch.DrawString(
                    Font,
                    Math.Floor(Progress * 100) + TextAppend,
                    new Vector2(slider.X + (slider.Width / 2) - (Font.MeasureString(Math.Floor(Progress * 100) + TextAppend).X - 15) / 2, slider.Y - 1),
                    TextColor,
                    0,
                    new Vector2(0, 0),
                    0.6f,
                    SpriteEffects.None,
                    0);
            }
        }
    }
}