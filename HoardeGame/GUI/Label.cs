// <copyright file="Label.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Diagnostics;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GUI
{
    /// <summary>
    /// UI text control
    /// </summary>
    public class Label : GuiBase
    {
        private SpriteFont font;

        /// <summary>
        /// Collision box of the label for clicking
        /// </summary>
        private Rectangle label;

        /// <summary>
        /// Currently displayed text
        /// </summary>
        private string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="state">State to which this label belongs</param>
        /// <param name="id">ID of this control for debugging</param>
        /// <param name="noSub">Whether to subscribe to events in gamestate</param>
        public Label(GameState state, string id, bool noSub = false) : base(state, id, noSub)
        {
        }

        /// <summary>
        /// Gets or sets the position of the label
        /// </summary>
        public new Vector2 Position
        {
            get
            {
                return new Vector2(label.X, label.Y);
            }

            set
            {
                label.X = (int)value.X;
                label.Y = (int)value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the text of this label and resizes the collision box
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                if (font != null)
                {
                    label.Width = (int)font.MeasureString(value).X;
                }
                else
                {
                    label.Width = (int)Font.MeasureString(value).X;
                }

                text = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom font for this label
        /// </summary>
        public SpriteFont CustomFont
        {
            get
            {
                return font;
            }

            set
            {
                label.Width = (int)value.MeasureString(text).X;
                font = value;
            }
        }

        /// <summary>
        /// Checks if the user has clicked / hovered over the label and runs the delegate in case the user did
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        /// <param name="point">Point where the mouse is located</param>
        /// <param name="click">Whether the user clicked this frame</param>
        public override void Check(GameTime gameTime, Point point, bool click)
        {
            if (Visibility == HiddenState.Hidden | Visibility == HiddenState.Disabled)
            {
                return;
            }

            if (!label.Contains(point))
            {
                return;
            }

            try
            {
                OnMouseOver.Invoke();
            }
            catch
            {
                Debug.WriteLine("Error: " + ID + ": OnMouseOver: " + Text.ToLower());
            }

            if (!click)
            {
                return;
            }

            try
            {
                OnClick.Invoke();
            }
            catch
            {
                Debug.WriteLine("Error: " + ID + ": OnClick: " + Text.ToLower());
            }
        }

        /// <summary>
        /// Draws the label
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        /// <param name="spriteBatch">Current spirtebatch to use for drawing</param>
        /// <param name="interpolation">No idea</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
            if (Visibility == HiddenState.Hidden)
            {
                return;
            }

            spriteBatch.DrawString(font ?? Font, text, Position, Visibility == HiddenState.Disabled ? DisabledColor : Color);
        }
    }
}