// <copyright file="CardManager.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Gameplay
{
    /// <summary>
    /// CardManager
    /// </summary>
    public class CardManager
    {
        private static Dictionary<string, Card> cards;

        [XmlIgnore]
        private static Dictionary<CardRarity, Texture2D> backgrounds;

        /// <summary>
        /// Init singleton
        /// </summary>
        public static void Init()
        {
            cards = new Dictionary<string, Card>();
            backgrounds = new Dictionary<CardRarity, Texture2D>();
        }

        /// <summary>
        /// Load cards from file
        /// </summary>
        /// <param name="filename">XML file to load from</param>
        public static void LoadXmlFile(string filename)
        {
            CardManager.cards.Clear();

            XmlSerializer ser = new XmlSerializer(typeof(List<Card>), new XmlRootAttribute("Cards"));
            TextReader reader = new StreamReader(filename);

            List<Card> cards = ser.Deserialize(reader) as List<Card>;
            if (cards == null)
            {
                throw new InvalidDataException("Provided card file is not valid! ({" + filename + "})");
            }

            foreach (var card in cards)
            {
                card.Texture = ResourceManager.Texture(card.TextureName);
                CardManager.cards.Add(card.ID, card);
            }

            reader.Close();
        }

        /// <summary>
        /// Save cards to file
        /// </summary>
        /// <param name="filename">XML file to save to</param>
        public static void SaveXmlFile(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Card>), new XmlRootAttribute("Cards"));
            TextWriter writer = new StreamWriter(filename);

            ser.Serialize(writer, cards.Values.ToList());

            writer.Close();
        }

        /// <summary>
        /// Load background textures
        /// </summary>
        public static void LoadBackgrounds()
        {
            backgrounds.Add(CardRarity.Common, ResourceManager.Texture("CommonCard"));

            // _backgrounds.Add(CardRarity.Rare, ResourceManager.Texture("RareCard"));
            // _backgrounds.Add(CardRarity.Epic, ResourceManager.Texture("EpicCard"));
        }

        /// <summary>
        ///     Get a card singleton
        /// </summary>
        /// <param name="key">Name of card</param>
        /// <returns>Found card or <c>null</c></returns>
        public static Card GetCard(string key)
        {
            return !cards.ContainsKey(key) ? null : cards[key];
        }

        /// <summary>
        ///     Get a background texture
        /// </summary>
        /// <param name="rarity"><see cref="CardRarity"/></param>
        /// <returns><see cref="Texture2D"/></returns>
        public static Texture2D GetBackground(CardRarity rarity)
        {
            return !backgrounds.ContainsKey(rarity) ? null : backgrounds[rarity];
        }
    }
}