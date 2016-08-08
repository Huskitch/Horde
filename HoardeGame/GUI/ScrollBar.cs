// <copyright file="ScrollBar.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.Input;
using HoardeGame.Resources;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GUI
{
    /// <summary>
    /// Scrollbar control
    /// </summary>
    public class ScrollBar : GuiBase
    {
        /// <summary>
        /// Gets or sets the position of the slider
        /// Setting this property will also recalculate the hitbox
        /// </summary>
        public new Vector2 Position
        {
            get
            {
                return scrollBar.Location.ToVector2();
            }

            set
            {
                scrollBar.X = (int)value.X;
                scrollBar.Y = (int)value.Y;
                scrollBar = new Rectangle((int)value.X, (int)value.Y, 10, scrollBar.Height);
                hitBox = new Rectangle((int)value.X, (int)value.Y, 10, scrollBar.Height + 10);
            }
        }

        /// <summary>
        /// Gets or sets the height of this scrollbar
        /// Setting this property will also recalculate the hitbox
        /// </summary>
        public int Height
        {
            get
            {
                return scrollBar.Height;
            }

            set
            {
                scrollBar.Height = value;
                hitBox.Height = scrollBar.Height + 10;
            }
        }

        private readonly Texture2D sliderTexture;
        private readonly IInputProvider inputProvider;

        private Rectangle hitBox;
        private Rectangle scrollBar;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBar"/> class.
        /// </summary>
        /// <param name="state"><see cref="GameState"/> to which this progress bar belongs</param>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> to resolve DI</param>
        /// <param name="id">ID of this control for debugging</param>
        /// <param name="noSub">Whether to subscribe to events in gamestate</param>
        public ScrollBar(GameState state, GameServiceContainer serviceContainer, string id, bool noSub = false) : base(state, id, noSub)
        {
            sliderTexture = serviceContainer.GetService<IResourceProvider>().GetTexture("OneByOneEmpty");
            inputProvider = serviceContainer.GetService<IInputProvider>();
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
            base.Draw(gameTime, spriteBatch, interpolation);
        }
    }
}