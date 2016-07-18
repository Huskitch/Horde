using System;
using System.Diagnostics;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GUI
{
    public class Button : GuiBase
    {
        private Rectangle _button;
        private Color _color;
        private string _text = "";
        public Color OverColor = Color.Red;
        public Texture2D ButtonTexture;

        public Button(GameState state, string id, bool noSub = false) : base(state, id, noSub)
        {
        }

        public new Vector2 Position
        {
            set
            {
                _button.X = (int) value.X;
                _button.Y = (int) value.Y;
            }
            get { return new Vector2(_button.X, _button.Y); }
        }

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

            spriteBatch.DrawString(Font, _text, new Vector2(_button.X, _button.Y), _color);
            if (ButtonTexture != null) spriteBatch.Draw(ButtonTexture, new Vector2(_button.X, _button.Y), Color.White);
        }
    }
}