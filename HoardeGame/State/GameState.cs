// <copyright file="GameState.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.State
{
    /// <summary>
    /// GameState
    /// </summary>
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
        /// Gets or sets a value indicating whether this gamestate is paused
        /// </summary>
        public bool Paused { get; set; }

        /// <summary>
        /// Start this GameState
        /// </summary>
        public virtual void Start()
        {
        }

        /// <summary>
        /// Stop this GameState
        /// </summary>
        public virtual void End()
        {
        }

        /// <summary>
        /// Pause this GameState
        /// </summary>
        public virtual void Pause()
        {
            Paused = true;
        }

        /// <summary>
        /// Resume this GameState
        /// </summary>
        public virtual void Resume()
        {
            Paused = false;
        }

        /// <summary>
        /// Update this GameState
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draw this GameState
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        /// <param name="interpolation">Interpolation</param>
        public virtual void Draw(GameTime gameTime, float interpolation)
        {
        }

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
    }
}