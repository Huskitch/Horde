// <copyright file="ProgressBar.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GUI
{
    public class ProgressBar : GuiBase
    {
        public Texture2D ProgressBarTexture;
        public Rectangle TargetRectangle;

        private Rectangle _bar;
        private float _progress;

        public float Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                _bar = new Rectangle((int) (BarStart.X + Position.X), (int) (BarStart.Y + Position.Y), (int) (BarWidth * _progress), BarHeight);
            }
        }

        public Vector2 BarStart;
        public int BarHeight;
        public float BarWidth;

        public ProgressBar(GameState state, string id, bool noSub = false) : base(state, id, noSub)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
            spriteBatch.Draw(ProgressBarTexture, TargetRectangle, Color.White);
            spriteBatch.Draw(ResourceManager.Texture("OneByOneEmpty"), _bar, Color);
        }
    }
}