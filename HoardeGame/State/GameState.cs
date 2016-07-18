using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.State
{
    public abstract class GameState
    {
        public delegate void GuiDrawEventHandler(GameTime gameTime, SpriteBatch batch, float interpolation);
        public delegate void GuiCheckEventHandler(GameTime gameTime, Point point, bool click);

        public event GuiCheckEventHandler OnCheck;
        public event GuiDrawEventHandler OnDraw;

        protected virtual void DoCheck(GameTime gameTime, Point point, bool click)
        {
            OnCheck?.Invoke(gameTime, point, click);
        }

        protected virtual void DoDraw(GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
            OnDraw?.Invoke(gameTime, spriteBatch, interpolation);
        }

        public virtual void Start() { }
        public virtual void End() { }
        public virtual void Pause() { }
        public virtual void Resume() { }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime, float interpolation) { }
    }
}
