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
        /// <summary>
        /// Delegate used for drawing GUI elements / controls
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        /// <param name="spriteBatch">Current spirtebatch to use for drawing</param>
        /// <param name="interpolation">No idea</param>
        public delegate void GuiDrawEventHandler(GameTime gameTime, SpriteBatch spriteBatch, float interpolation);

        /// <summary>
        /// Delefare used for updating GUI elements / controls
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        /// <param name="point">Point where the mouse is located</param>
        /// <param name="click">Whether the user clicked this frame</param>
        public delegate void GuiCheckEventHandler(GameTime gameTime, Point point, bool click);

        /// <summary>
        /// Event that gets fired whenever the gamestate updates
        /// </summary>
        public event GuiCheckEventHandler OnCheck;

        /// <summary>
        /// Event that gets fired whenever the gamestate draws
        /// </summary>
        public event GuiDrawEventHandler OnDraw;

        /// <summary>
        /// Checks and invokes the OnCheck event
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        /// <param name="point">Point where the mouse is located</param>
        /// <param name="click">Whether the user clicked this frame</param>
        protected virtual void DoCheck(GameTime gameTime, Point point, bool click)
        {
            OnCheck?.Invoke(gameTime, point, click);
        }


        /// <summary>
        /// Checks and invokes teh OnDraw event
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        /// <param name="spriteBatch">Current spirtebatch to use for drawing</param>
        /// <param name="interpolation">No idea</param>
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
