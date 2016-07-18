using System;
using System.Diagnostics;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Gameplay
{
    public enum CardRarity
    {
        Common,
        Rare,
        Epic
    }

    public enum CardType
    {
        IncreaseDamage,
        IncreaseSpeed
    }

    public class Card
    {
        public static Vector2 CardTextureOffset = new Vector2(72, 76);
        public static Vector2 CardNameOffset = new Vector2(135, 65);
        public static Vector2 CardDescriptionOffset = new Vector2(98, 390);
        public static Vector2 Mana1Offset = new Vector2(128, 485);
        public static Vector2 Mana2Offset = new Vector2(198, 485);
        public static Vector2 Mana3Offset = new Vector2(268, 485);

        [XmlIgnore]
        public Texture2D Texture;

        [XmlAttribute]
        public string ID;

        public string Name;
        public string TextureName;
        public CardRarity Rarity;
        public CardType Type;
        public string Description;
        public int[] Price = new int[3];

        public void Draw(Vector2 position, float scale, GameTime gameTime, SpriteBatch spriteBatch, float interpolation)
        {
            Vector2 scaleVector = new Vector2(scale, scale);

            spriteBatch.Draw(CardManager.GetBackground(Rarity), position, null, null, null, 0f, scaleVector, Color.White);
            spriteBatch.Draw(Texture, position + CardTextureOffset * scale, null, null, null, 0f, scaleVector, Color.White);

            spriteBatch.DrawString(ResourceManager.Font("BasicFont"), Name, CardNameOffset + position, Color.White, 0f, Vector2.Zero, scaleVector * 1.25f, SpriteEffects.None, 0);

            spriteBatch.DrawString(ResourceManager.Font("BasicFont"), Description, CardDescriptionOffset + position - Vector2.One, Color.Gray, 0f, Vector2.Zero, scaleVector * 1.25f, SpriteEffects.None, 0);
            spriteBatch.DrawString(ResourceManager.Font("BasicFont"), Description, CardDescriptionOffset + position, Color.White, 0f, Vector2.Zero, scaleVector * 1.25f, SpriteEffects.None, 0);

            spriteBatch.DrawString(ResourceManager.Font("BasicFont"), Price[0].ToString(), Mana1Offset + position, Color.White, 0f, Vector2.Zero, scaleVector * 1.25f, SpriteEffects.None, 0);
            spriteBatch.DrawString(ResourceManager.Font("BasicFont"), Price[1].ToString(), Mana2Offset + position, Color.White, 0f, Vector2.Zero, scaleVector * 1.25f, SpriteEffects.None, 0);
            spriteBatch.DrawString(ResourceManager.Font("BasicFont"), Price[2].ToString(), Mana3Offset + position, Color.White, 0f, Vector2.Zero, scaleVector * 1.25f, SpriteEffects.None, 0);
        }

        public override string ToString()
        {
            return $"{Name} ({ID}): {Description} ({Price[0]}|{Price[1]}|{Price[2]})";
        }
    }
}
