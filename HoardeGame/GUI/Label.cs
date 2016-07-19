// <copyright file="Label.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Diagnostics;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GUI
{
    public class Label : GuiBase
    {
        /// <summary>
        /// Collision box of the label for clicking
        /// </summary>
        private Rectangle _label;

        /// <summary>
        /// Currently displayed text
        /// </summary>
        private string _text;

        /// <summary>
        /// Creates new label
        /// </summary>
        /// <param name="state">State to which this label belongs</param>
        /// <param name="id">ID of this control for debugging</param>
        /// <param name="noSub">Whether to subscribe to events in gamestate</param>
        public Label(GameState state, string id, bool noSub = false) : base(state, id, noSub)
        {
        }

        /// <summary>
        /// Gets / sets the position of the label
        /// </summary>
        public new Vector2 Position
        {
            set
            {
                _label.X = (int) value.X;
                _label.Y = (int) value.Y;
            }
            get { return new Vector2(_label.X, _label.Y); }
        }

        /// <summary>
        /// Sets the text of this label and resizes the collision box
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                _label.Width = (int) Font.MeasureString(value).X;
                _text = value;
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
            if (Visibility == HiddenState.Hidden | Visibility == HiddenState.Disabled) return;

            if (!_label.Contains(point)) return;

            try
            {
                OnMouseOver.Invoke();
            }
            catch
            {
                Debug.WriteLine("Error: " + ID + ": OnMouseOver: " + Text.ToLower());
            }

            if (!click) return;

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
            if (Visibility == HiddenState.Hidden) return;

            spriteBatch.DrawString(Font, _text, Position, Visibility == HiddenState.Disabled ? DisabledColor : Color);
        }
    }
}