using System;
using System.Diagnostics;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GUI
{
    public class Label : GuiBase
    {
        private Rectangle _label;
        private string _text;

        public Label(GameState state, string id, bool noSub = false) : base(state, id, noSub)
        {
        }

        public new Vector2 Position
        {
            set
            {
                _label.X = (int) value.X;
                _label.Y = (int) value.Y;
            }
            get { return new Vector2(_label.X, _label.Y); }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _label.Width = (int) Font.MeasureString(value).X;
                _text = value;
            }
        }

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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
            if (Visibility == HiddenState.Hidden) return;

            spriteBatch.DrawString(Font, _text, Position, Visibility == HiddenState.Disabled ? DisabledColor : Color);
        }
    }
}