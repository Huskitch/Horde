// <copyright file="Card.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Xml.Serialization;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Gameplay
{
    /// <summary>
    /// Rarity of a card
    /// </summary>
    public enum CardRarity
    {
        /// <summary>
        /// Common rarity
        /// </summary>
        Common,

        /// <summary>
        /// Rare rarity
        /// </summary>
        Rare,

        /// <summary>
        /// Epic rarity
        /// </summary>
        Epic
    }

    /// <summary>
    /// Type of a card
    /// </summary>
    public enum CardType
    {
        /// <summary>
        /// Card increases damage
        /// </summary>
        IncreaseDamage,

        /// <summary>
        /// Card increases speed
        /// </summary>
        IncreaseSpeed
    }

    /// <summary>
    /// Card
    /// </summary>
    public class Card
    {
        private static readonly Vector2 CardTextureOffset = new Vector2(72, 76);
        private static readonly Vector2 CardNameOffset = new Vector2(135, 65);
        private static readonly Vector2 CardDescriptionOffset = new Vector2(98, 390);
        private static readonly Vector2 Mana1Offset = new Vector2(128, 485);
        private static readonly Vector2 Mana2Offset = new Vector2(198, 485);
        private static readonly Vector2 Mana3Offset = new Vector2(268, 485);

        /// <summary>
        /// Gets or sets texture of the card
        /// </summary>
        [XmlIgnore]
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Gets or sets unique ID of the card
        /// </summary>
        [XmlAttribute]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the card
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the texture name of the card
        /// </summary>
        public string TextureName { get; set; }

        /// <summary>
        /// Gets or sets the rarity of the card
        /// </summary>
        public CardRarity Rarity { get; set; }

        /// <summary>
        /// Gets or sets the type of the card
        /// </summary>
        public CardType Type { get; set; }

        /// <summary>
        /// Gets or sets the description of the card
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the prices of the card
        /// </summary>
        public int[] Price { get; set; } = new int[3];

        private IResourceProvider resourceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for loading resources</param>
        public void Initialize(IResourceProvider resourceProvider)
        {
            this.resourceProvider = resourceProvider;
        }

        /// <summary>
        ///     Draw the card
        /// </summary>
        /// <param name="position">Position to draw at</param>
        /// <param name="scale">Scale to draw at</param>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> to draw with</param>
        /// <param name="interpolation">Interpolation</param>
        public void Draw(Vector2 position, float scale, GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
            Vector2 scaleVector = new Vector2(scale, scale);
            Texture2D cardTexture = null;

            switch (Rarity)
            {
                case CardRarity.Common:
                    cardTexture = resourceProvider.GetTexture("CommonCard");
                    break;
                case CardRarity.Rare:
                    cardTexture = resourceProvider.GetTexture("RareCard");
                    break;
                case CardRarity.Epic:
                    cardTexture = resourceProvider.GetTexture("EpicCard");
                    break;
            }

            spriteBatch.Draw(cardTexture, position, null, null, null, 0f, scaleVector, Color.White);
            spriteBatch.Draw(Texture, position + CardTextureOffset * scale, null, null, null, 0f, scaleVector, Color.White);

            spriteBatch.DrawString(resourceProvider.GetFont("BasicFont"), Name, CardNameOffset * scale + position, Color.White, 0f, Vector2.Zero, scaleVector * 1.25f, SpriteEffects.None, 0);

            spriteBatch.DrawString(resourceProvider.GetFont("BasicFont"), Description, CardDescriptionOffset * scale + position - Vector2.One, Color.Gray, 0f, Vector2.Zero, scaleVector * 1.25f, SpriteEffects.None, 0);
            spriteBatch.DrawString(resourceProvider.GetFont("BasicFont"), Description, CardDescriptionOffset * scale + position, Color.White, 0f, Vector2.Zero, scaleVector * 1.25f, SpriteEffects.None, 0);

            spriteBatch.DrawString(resourceProvider.GetFont("BasicFont"), Price[0].ToString(), Mana1Offset * scale + position, Color.White, 0f, Vector2.Zero, scaleVector * 1.25f, SpriteEffects.None, 0);
            spriteBatch.DrawString(resourceProvider.GetFont("BasicFont"), Price[1].ToString(), Mana2Offset * scale + position, Color.White, 0f, Vector2.Zero, scaleVector * 1.25f, SpriteEffects.None, 0);
            spriteBatch.DrawString(resourceProvider.GetFont("BasicFont"), Price[2].ToString(), Mana3Offset * scale + position, Color.White, 0f, Vector2.Zero, scaleVector * 1.25f, SpriteEffects.None, 0);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Name} ({ID}): {Description} ({Price[0]}|{Price[1]}|{Price[2]})";
        }
    }
}