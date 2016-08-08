// <copyright file="ConsumeableDisplay.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using HoardeGame.Gameplay.Consumeables;
using HoardeGame.Resources;
using HoardeGame.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.GUI
{
    /// <summary>
    /// Control displaying a consumeable an it's name
    /// </summary>
    public class ConsumeableDisplay : GuiBase
    {
        /// <summary>
        /// Gets or sets the weapon to be displayed
        /// </summary>
        public Consumeable DisplayedItem { get; set; }

        /// <summary>
        /// Gets or sets extra text in the bottom right of this control
        /// </summary>
        public string ExtraText { get; set; }

        /// <summary>
        /// Gets or sets the rectangle where this display should be drawn
        /// </summary>
        public Rectangle TargetRectangle { get; set; }

        private readonly IResourceProvider resourceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumeableDisplay"/> class.
        /// </summary>
        /// <param name="state"><see cref="GameState"/> to which this button belongs</param>
        /// <param name="id">ID of this control for debugging</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        /// <param name="noSub">Whether to subscribe to events in gamestate</param>
        public ConsumeableDisplay(GameState state, string id, IResourceProvider resourceProvider, bool noSub = false) : base(state, id, noSub)
        {
            this.resourceProvider = resourceProvider;
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
            var texture = resourceProvider.GetTexture(DisplayedItem?.Texture ?? string.Empty);
            Rectangle targetWeaponRectangle = TargetRectangle;

            if (texture?.Width == texture?.Height)
            {
                targetWeaponRectangle = new Rectangle(TargetRectangle.X + (TargetRectangle.Width - TargetRectangle.Height) / 2, TargetRectangle.Y, TargetRectangle.Height, TargetRectangle.Height);
            }

            spriteBatch.Draw(resourceProvider.GetTexture("OneByOneEmpty"), TargetRectangle, Color);

            if (DisplayedItem != null)
            {
                spriteBatch.Draw(texture, targetWeaponRectangle, Color.White);
            }

            spriteBatch.DrawString(resourceProvider.GetFont("SmallFont"), DisplayedItem?.Name ?? "Empty", TargetRectangle.Location.ToVector2() + new Vector2(2, TargetRectangle.Height - 23), Color.White, 0f, Vector2.Zero, new Vector2(0.75f, 0.75f), SpriteEffects.None, 0f);
            spriteBatch.DrawString(resourceProvider.GetFont("SmallFont"), ExtraText, (TargetRectangle.Location + TargetRectangle.Size).ToVector2() - new Vector2(15, 20), Color.White, 0f, Vector2.Zero, new Vector2(0.75f, 0.75f), SpriteEffects.None, 0f);
        }
    }
}