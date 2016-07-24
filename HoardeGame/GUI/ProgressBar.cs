// <copyright file="ProgressBar.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.Resources;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GUI
{
    /// <summary>
    /// UI progress bar
    /// </summary>
    public class ProgressBar : GuiBase
    {
        /// <summary>
        /// Gets or sets the texture of the <see cref="ProgressBar"/>
        /// </summary>
        public Texture2D ProgressBarTexture { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Rectangle"/> where the <see cref="ProgressBar"/> should be drawn
        /// </summary>
        public Rectangle TargetRectangle { get; set; }

        /// <summary>
        /// Gets or sets the position from where the inner part of the <see cref="ProgressBar"/> starts
        /// </summary>
        public Vector2 BarStart { get; set; }

        /// <summary>
        /// Gets or sets the height the inner part of the <see cref="ProgressBar"/>
        /// </summary>
        public int BarHeight { get; set; }

        /// <summary>
        /// Gets or sets the width of the inner part of the <see cref="ProgressBar"/>
        /// </summary>
        public float BarWidth { get; set; }

        private readonly IResourceProvider resourceProvider;

        private Rectangle bar;
        private float progress;

        /// <summary>
        /// Gets or sets the progress
        /// 0 - 1f
        /// </summary>
        public float Progress
        {
            get
            {
                return progress;
            }

            set
            {
                progress = value;
                bar = new Rectangle((int)(BarStart.X + Position.X), (int)(BarStart.Y + Position.Y), (int)(BarWidth * progress), BarHeight);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> class.
        /// </summary>
        /// <param name="state"><see cref="GameState"/> to which this progress bar belongs</param>
        /// <param name="id">ID of this control for debugging</param>
        /// <param name="noSub">Whether to subscribe to events in gamestate</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> to use for loading resources</param>
        public ProgressBar(GameState state, IResourceProvider resourceProvider, string id, bool noSub = false) : base(state, id, noSub)
        {
            this.resourceProvider = resourceProvider;
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
            spriteBatch.Draw(ProgressBarTexture, TargetRectangle, Color.White);
            spriteBatch.Draw(resourceProvider.GetTexture("OneByOneEmpty"), bar, Color);
        }
    }
}