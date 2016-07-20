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
    /// Manager for cards
    /// </summary>
    public class CardManager : ICardProvider
    {
        private Dictionary<string, Card> cards = new Dictionary<string, Card>();
        private Dictionary<CardRarity, Texture2D> backgrounds = new Dictionary<CardRarity, Texture2D>();

        /// <inheritdoc/>
        public Card GetCard(string key)
        {
            return !cards.ContainsKey(key) ? null : cards[key];
        }

        /// <summary>
        /// Load cards from a file
        /// </summary>
        /// <param name="filename">XML file to load from</param>
        public void LoadXmlFile(string filename)
        {
            cards.Clear();

            XmlSerializer ser = new XmlSerializer(typeof(List<Card>), new XmlRootAttribute("Cards"));
            TextReader reader = new StreamReader(filename);

            List<Card> cardList = ser.Deserialize(reader) as List<Card>;
            if (cardList == null)
            {
                throw new InvalidDataException("Provided card file is not valid! ({" + filename + "})");
            }

            foreach (var card in cardList)
            {
                card.Texture = ResourceManager.GetTexture(card.TextureName);
                cards.Add(card.ID, card);
            }

            reader.Close();
        }

        /// <summary>
        /// Save cards to file
        /// </summary>
        /// <param name="filename">XML file to save to</param>
        public void SaveXmlFile(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Card>), new XmlRootAttribute("Cards"));
            TextWriter writer = new StreamWriter(filename);

            ser.Serialize(writer, cards.Values.ToList());

            writer.Close();
        }
    }
}