// <copyright file="CardManager.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using HoardeGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Gameplay.Cards
{
    /// <summary>
    /// Manager for cards
    /// </summary>
    public class CardManager : ICardProvider
    {
        private readonly Dictionary<string, Card> cards = new Dictionary<string, Card>();
        private readonly Dictionary<CardRarity, Texture2D> backgrounds = new Dictionary<CardRarity, Texture2D>();
        private readonly IResourceProvider resourceProvider;
        private readonly GameServiceContainer serviceContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardManager"/> class.
        /// </summary>
        /// <param name="serviceContainer"><see cref="GameServiceContainer"/> for resolving DI</param>
        public CardManager(GameServiceContainer serviceContainer)
        {
            this.serviceContainer = serviceContainer;
            resourceProvider = serviceContainer.GetService<IResourceProvider>();
        }

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
            Debug.WriteLine("Loading cards...");
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
                card.Validate(serviceContainer);
                card.Initialize(serviceContainer);
                cards.Add(card.ID, card);

                Debug.WriteLine($"Loaded card: {card.Name} ({card.ID})");
            }

            reader.Close();
            Debug.WriteLine("Done loading cards...");
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