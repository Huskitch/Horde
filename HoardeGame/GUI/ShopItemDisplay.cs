// <copyright file="ShopItemDisplay.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.Gameplay.Player;
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

        /// <summary>
        /// Gets or sets the background colour of the display when the user hovers the mouse over it
        /// </summary>
        public Color MouseOverColor { get; set; } = new Color(150, 150, 150, 100);

        /// <summary>
        /// Gets or sets the background colour
        /// </summary>
        public Color BackgroundColor { get; set; } = new Color(100, 100, 100, 100);

        private readonly Texture2D background;
        private readonly Texture2D redGem;
        private readonly Texture2D greenGem;
        private readonly Texture2D blueGem;
        private readonly IPlayerProvider playerProvider;
        private readonly SpriteFont font;

        private Color color;

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
            playerProvider = serviceContainer.GetService<IPlayerProvider>();

            font = resourceProvider.GetFont("SmallFont");
            background = resourceProvider.GetTexture("OneByOneEmpty");
            redGem = resourceProvider.GetTexture("GemAnimation");
            greenGem = resourceProvider.GetTexture("EmeraldSheet");
            blueGem = resourceProvider.GetTexture("DiamondSheet");

            color = BackgroundColor;
        }

        /// <inheritdoc/>
        public override void Check(GameTime gameTime, Point point, bool click)
        {
            if (Visibility == HiddenState.Disabled || Visibility == HiddenState.Hidden)
            {
                return;
            }

            if (new Rectangle(Position.ToPoint(), new Point(200, 104)).Contains(point))
            {
                OnMouseOver?.Invoke();
                color = MouseOverColor;

                if (click)
                {
                    OnClick?.Invoke();

                    if (ShopItem?.Price?.RedGems <= playerProvider.Player.Gems.RedGems && ShopItem?.Price?.GreenGems <= playerProvider.Player.Gems.GreenGems && ShopItem?.Price?.BlueGems <= playerProvider.Player.Gems.BlueGems)
                    {
                        bool? bought = ShopItem?.OnBought?.Invoke(playerProvider.Player);
                        if (bought.HasValue && bought.Value)
                        {
                            playerProvider.Player.Gems.RedGems -= ShopItem.Price.RedGems;
                            playerProvider.Player.Gems.GreenGems -= ShopItem.Price.GreenGems;
                            playerProvider.Player.Gems.BlueGems -= ShopItem.Price.BlueGems;
                        }
                    }
                }
            }
            else
            {
                color = BackgroundColor;
            }
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
            if (ShopItem == null || Visibility == HiddenState.Hidden)
            {
                return;
            }

            spriteBatch.Draw(background, new Rectangle(Position.ToPoint(), new Point(200, 104)), color);

            if (ShopItem.Icon != null)
            {
                spriteBatch.Draw(ShopItem.Icon, new Rectangle(Position.ToPoint() + new Point(5, 20), new Point(64, 64)), Color);
            }

            spriteBatch.DrawString(font, ShopItem.Name, Position + new Vector2(74, 5), Color, 0f, Vector2.Zero, new Vector2(0.75f), SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, ShopItem.Price?.RedGems.ToString(), Position + new Vector2(74, 30), Color, 0f, Vector2.Zero, new Vector2(0.75f), SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, ShopItem.Price?.GreenGems.ToString(), Position + new Vector2(74, 55), Color, 0f, Vector2.Zero, new Vector2(0.75f), SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, ShopItem.Price?.BlueGems.ToString(), Position + new Vector2(74, 80), Color, 0f, Vector2.Zero, new Vector2(0.75f), SpriteEffects.None, 0f);

            spriteBatch.Draw(redGem, new Rectangle((int)Position.X + 90, (int)Position.Y + 27, 32, 32), new Rectangle(64, 0, 16, 16), Color.White);
            spriteBatch.Draw(greenGem, new Rectangle((int)Position.X + 90, (int)Position.Y + 52, 32, 32), new Rectangle(64, 0, 16, 16), Color.White);
            spriteBatch.Draw(blueGem, new Rectangle((int)Position.X + 85, (int)Position.Y + 67, 44, 44), new Rectangle(144, 0, 24, 24), Color.White);
        }
    }
}