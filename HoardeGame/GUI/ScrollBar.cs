// <copyright file="ScrollBar.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.Input;
using HoardeGame.Resources;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
                hitBox = new Rectangle((int)value.X, (int)value.Y - 10, 10, scrollBar.Height + 20);
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
                hitBox.Height = scrollBar.Height + 20;
            }
        }

        /// <summary>
        /// Gets or sets the colour of the draggable pin
        /// </summary>
        public Color PinColor { get; set; } = Color.Gray;

        /// <summary>
        /// Gets or sets the progress (0f - 1f)
        /// </summary>
        public float Progress { get; set; }

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
        public override void Check(GameTime gameTime, Point point, bool click)
        {
            if (Visibility == HiddenState.Hidden | Visibility == HiddenState.Disabled)
            {
                return;
            }

            if (!hitBox.Contains(point))
            {
                return;
            }

            OnMouseOver?.Invoke();

            if (inputProvider.MouseState.LeftButton != ButtonState.Pressed)
            {
                return;
            }

            OnClick?.Invoke();

            Progress = (point.Y - scrollBar.Y) / (float)scrollBar.Height;

            Progress = MathHelper.Clamp(Progress, 0f, 1f);
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
            if (Visibility == HiddenState.Hidden)
            {
                return;
            }

            spriteBatch.Draw(sliderTexture, Position, null, Color, 0, Vector2.Zero, new Vector2(10, scrollBar.Height), SpriteEffects.None, 0);
            spriteBatch.Draw(sliderTexture, Position + new Vector2(0, (scrollBar.Height - 10) * Progress), null, PinColor, 0, Vector2.Zero, new Vector2(10, 10), SpriteEffects.None, 0);
        }
    }
}