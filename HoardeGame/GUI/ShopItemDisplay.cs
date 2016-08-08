// <copyright file="ShopItemDisplay.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.Gameplay.Shop;
using HoardeGame.Resources;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GUI
{
    /// <summary>
    /// Control which displays a shop item
    /// </summary>
    public class ShopItemDisplay : GuiBase
    {
        /// <summary>
        /// Gets or sets the currently displayed item
        /// </summary>
        public ShopItem ShopItem { get; set; }

        private readonly Texture2D background;
        private readonly Texture2D redGem;
        private readonly Texture2D greenGem;
        private readonly Texture2D blueGem;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopItemDisplay"/> class.
        /// </summary>
        /// <param name="state"><see cref="GameState"/> to which this progress bar belongs</param>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        /// <param name="id">ID of this control for debugging</param>
        /// <param name="noSub">Whether to subscribe to events in gamestate</param>
        public ShopItemDisplay(GameState state, GameServiceContainer serviceContainer, string id, bool noSub = false) : base(state, id, noSub)
        {
            IResourceProvider resourceProvider = serviceContainer.GetService<IResourceProvider>();
            background = resourceProvider.GetTexture("OneByOneEmpty");
            redGem = resourceProvider.GetTexture("GemAnimation");
            greenGem = resourceProvider.GetTexture("EmeraldSheet");
            blueGem = resourceProvider.GetTexture("DiamondSheet");
        }

        /// <inheritdoc/>
        public override void Check(GameTime gameTime, Point point, bool click)
        {
            if (Visibility == HiddenState.Disabled || Visibility == HiddenState.Hidden)
            {
                return;
            }

            if (new Rectangle(Position.ToPoint(), new Point(300, 104)).Contains(point))
            {
                OnMouseOver?.Invoke();

                if (click)
                {
                    OnClick?.Invoke();

                    // TODO: BUY THE THING
                }
            }
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
            if (ShopItem == null || Visibility == HiddenState.Hidden)
            {
                return;
            }

            spriteBatch.Draw(background, new Rectangle(Position.ToPoint(), new Point(300, 104)), new Color(100, 100, 100, 100));

            if (ShopItem.Icon != null)
            {
                spriteBatch.Draw(ShopItem.Icon, new Rectangle(Position.ToPoint() + new Point(0, 20), new Point(64, 64)), Color);
            }

            spriteBatch.DrawString(Font, ShopItem.Name, Position + new Vector2(64, 5), Color);
            spriteBatch.DrawString(Font, ShopItem.Price?.RedGems.ToString(), Position + new Vector2(64, 30), Color);
            spriteBatch.DrawString(Font, ShopItem.Price?.GreenGems.ToString(), Position + new Vector2(64, 55), Color);
            spriteBatch.DrawString(Font, ShopItem.Price?.BlueGems.ToString(), Position + new Vector2(64, 80), Color);

            spriteBatch.Draw(redGem, new Rectangle((int)Position.X + 90, (int)Position.Y + 27, 32, 32), new Rectangle(64, 0, 16, 16), Color.White);
            spriteBatch.Draw(greenGem, new Rectangle((int)Position.X + 90, (int)Position.Y + 52, 32, 32), new Rectangle(64, 0, 16, 16), Color.White);
            spriteBatch.Draw(blueGem, new Rectangle((int)Position.X + 85, (int)Position.Y + 67, 44, 44), new Rectangle(144, 0, 24, 24), Color.White);
        }
    }
}