using System;
using System.Diagnostics;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GUI
{
    public class Button : GuiBase
    {
        /// <summary>
        /// Collision box of the button for clicking
        /// </summary>
        private Rectangle _button;

        /// <summary>
        /// Current colour of the button
        /// </summary>
        private Color _color;

        /// <summary>
        /// Currently displayed text
        /// </summary>
        private string _text = "";

        /// <summary>
        /// Color that the button changes color to when the user hovers over the button
        /// </summary>
        public Color OverColor = Color.Red;

        /// <summary>
        /// Background texture of the button
        /// </summary>
        public Texture2D ButtonTexture;

        /// <summary>
        /// Creates new button
        /// </summary>
        /// <param name="state">State to which this button belongs</param>
        /// <param name="id">ID of this control for debugging</param>
        /// <param name="noSub">Whether to subscribe to events in gamestate</param>
        public Button(GameState state, string id, bool noSub = false) : base(state, id, noSub)
        {
        }

        /// <summary>
        /// Gets / sets the position of the button
        /// </summary>
        public new Vector2 Position
        {
            set
            {
                _button.X = (int) value.X;
                _button.Y = (int) value.Y;
            }
            get { return new Vector2(_button.X, _button.Y); }
        }

        /// <summary>
        /// Sets the text of this button and resizes the collision box
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                _button = new Rectangle((int) Position.X, (int) Position.Y, (int)Font.MeasureString(value).X,
                    (int) Font.MeasureString(value).Y);
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
            if (Visibility == HiddenState.Hidden | Visibility == HiddenState.Disabled) return;

            CustomUpdate(gameTime, point, click);

            _color = Color;

            if (!_button.Contains(point))
            {
                return;
            }

            _color = OverColor;

            if (!click) return;
            try
            {
                OnClick.Invoke();
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
            if (Visibility == HiddenState.Hidden) return;

            CustomDraw(gameTime, spriteBatch, interpolation);

            if (_color == OverColor & Visibility == HiddenState.Visible)
            {
                try
                {
                    OnMouseOver.Invoke();
                }
                catch
                {
                    Debug.WriteLine("Error: " + ID + ": OnMouseOver: " + Text.ToLower());
                }
            }

            if (Visibility == HiddenState.Disabled) _color = DisabledColor;

            if (ButtonTexture != null) spriteBatch.Draw(ButtonTexture, new Vector2(_button.X, _button.Y), Color.White);
            spriteBatch.DrawString(Font, _text, new Vector2(_button.X, _button.Y), _color);
        }
    }
}