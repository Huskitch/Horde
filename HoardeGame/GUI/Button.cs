// <copyright file="Button.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GUI
{
    /// <summary>
    /// UI button
    /// </summary>
    public class Button : GuiBase
    {
        /// <summary>
        /// Collision box of the button for clicking
        /// </summary>
        private Rectangle button;

        /// <summary>
        /// Current colour of the button
        /// </summary>
        private Color color;

        /// <summary>
        /// Currently displayed text
        /// </summary>
        private string text = string.Empty;

        /// <summary>
        /// Gets or sets the offset for the displayed text
        /// </summary>
        public Vector2 TextOffset { get; set; } = Vector2.Zero;

        /// <summary>
        /// Gets or sets the <see cref="Color"/> that the button changes color to when the user hovers over the button
        /// </summary>
        public Color OverColor { get; set; } = Color.Red;

        /// <summary>
        /// Gets or sets the background <see cref="Texture2D"/> of the button
        /// </summary>
        public Texture2D ButtonTexture { get; set; }

        /// <summary>
        /// Gets or sets the bacgkround <see cref="Texture2D"/> of the button when the user hovers over the control
        /// </summary>
        public Texture2D ButtonTextureOver { get; set; }

        /// <summary>
        /// Gets or sets the background <see cref="Texture2D"/>  of the button when the button is disabled
        /// </summary>
        public Texture2D ButtonTextureDisabled { get; set; }

        /// <summary>
        /// Gets or sets the target <see cref="Rectangle"/> for the <see cref="Texture2D"/>
        /// </summary>
        public Rectangle TargetRectangle { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="state"><see cref="GameState"/> to which this button belongs</param>
        /// <param name="id">ID of this control for debugging</param>
        /// <param name="noSub">Whether to subscribe to events in gamestate</param>
        public Button(GameState state, string id, bool noSub = false) : base(state, id, noSub)
        {
        }

        /// <summary>
        /// Gets or sets the position of the button
        /// </summary>
        public new Vector2 Position
        {
            get
            {
                return new Vector2(button.X, button.Y);
            }

            set
            {
                button.X = (int)value.X;
                button.Y = (int)value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the text of this button and resizes the collision box
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
                button = new Rectangle((int)Position.X, (int)Position.Y, (int)Font.MeasureString(value).X, (int)Font.MeasureString(value).Y);
            }
        }

        /// <summary>
        /// Checks if the user has clicked / hovered over the button and runs the delegate in case the user did
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

            CustomUpdate?.Invoke(gameTime, point, click);

            color = Color;

            if (!button.Contains(point - new Point((int)TextOffset.X, (int)TextOffset.Y)) && !TargetRectangle.Contains(point))
            {
                return;
            }

            color = OverColor;

            if (!click)
            {
                return;
            }

            try
            {
                OnClick?.Invoke();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + ID + ": OnClick: " + Text.ToLower());
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Draws the button
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

            CustomDraw?.Invoke(gameTime, spriteBatch, interpolation);

            if (color == OverColor & Visibility == HiddenState.Visible)
            {
                try
                {
                    OnMouseOver?.Invoke();
                }
                catch
                {
                    Debug.WriteLine("Error: " + ID + ": OnMouseOver: " + Text.ToLower());
                }
            }

            if (Visibility == HiddenState.Disabled)
            {
                color = DisabledColor;
            }

            if (Visibility == HiddenState.Disabled && ButtonTextureDisabled != null)
            {
                spriteBatch.Draw(ButtonTextureDisabled, TargetRectangle, Color.White);
            }
            else if (color == OverColor && ButtonTextureOver != null)
            {
                spriteBatch.Draw(ButtonTextureOver, TargetRectangle, Color.White);
            }
            else if (ButtonTexture != null)
            {
                spriteBatch.Draw(ButtonTexture, TargetRectangle, Color.White);
            }

            if (text != string.Empty)
            {
                spriteBatch.DrawString(Font, text, new Vector2(button.X, button.Y) + TextOffset, color);
            }
        }
    }
}