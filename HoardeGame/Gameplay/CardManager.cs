
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Gameplay
{
    class CardManager
    {
        private static Dictionary<string, Card> _cards;

        [XmlIgnore]
        private static Dictionary<CardRarity, Texture2D> _backgrounds;

        public static void Init()
        {
            _cards = new Dictionary<string, Card>();
            _backgrounds = new Dictionary<CardRarity, Texture2D>();
        }

        public static void LoadXmlFile(string filename)
        {
            _cards.Clear();

            XmlSerializer ser = new XmlSerializer(typeof(List<Card>), new XmlRootAttribute("Cards"));
            TextReader reader = new StreamReader(filename);

            List<Card> cards = ser.Deserialize(reader) as List<Card>;
            if (cards == null) throw new InvalidDataException("Provided card file is not valid! ({" + filename + "})");

            foreach (var card in cards)
            {
                card.Texture = ResourceManager.Texture(card.TextureName);
                _cards.Add(card.ID, card);
            }

            reader.Close();
        }

        public static void SaveXmlFile(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Card>), new XmlRootAttribute("Cards"));
            TextWriter writer = new StreamWriter(filename);

            List<Card> cards = _cards.Values.ToList();

            ser.Serialize(writer, cards);

            writer.Close();
        }

        public static void LoadBackgrounds()
        {
            _backgrounds.Add(CardRarity.Common, ResourceManager.Texture("CommonCard"));
            //_backgrounds.Add(CardRarity.Rare, ResourceManager.Texture("RareCard"));
            //_backgrounds.Add(CardRarity.Epic, ResourceManager.Texture("EpicCard"));
        }

        public static Card GetCard(string key)
        {
            return !_cards.ContainsKey(key) ? null : _cards[key];
        }

        public static Texture2D GetBackground(CardRarity rarity)
        {
            return !_backgrounds.ContainsKey(rarity) ? null : _backgrounds[rarity];
        }
    }
}
