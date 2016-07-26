using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HoardeGame.Resources;
using HoardeGame.Themes;
using HoardeGame.Weapons;

namespace HoardeGame.Weapons
{
    public class WeaponManager : Weapons.IWeaponProvider
    {
        private readonly Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();

        /// <summary>
        /// Load cards from a file
        /// </summary>
        /// <param name="filename">XML file to load from</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for asset validation</param>
        public void LoadXmlFile(string filename, IResourceProvider resourceProvider)
        {
            weapons.Clear();

            XmlSerializer ser = new XmlSerializer(typeof(List<Weapon>), new XmlRootAttribute("Weapons"));
            TextReader reader = new StreamReader(filename);

            List<Weapon> weaponList = ser.Deserialize(reader) as List<Weapon>;
            if (weaponList == null)
            {
                throw new InvalidDataException("Provided weapon file is not valid! ({" + filename + "})");
            }

            foreach (var weapon in weaponList)
            {
                weapon.Validate(resourceProvider);
                weapons.Add(weapon.id, weapon);
            }

            reader.Close();
        }

        /// <summary>
        /// Save cards to file
        /// </summary>
        /// <param name="filename">XML file to save to</param>
        public void SaveXmlFile(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Theme>), new XmlRootAttribute("Themes"));
            TextWriter writer = new StreamWriter(filename);

            ser.Serialize(writer, weapons.Values.ToList());

            writer.Close();
        }

        /// <inheritdoc/>
        public Weapon GetWeapon(string name)
        {
            return !weapons.ContainsKey(name) ? null : weapons[name];
        }

        public Dictionary<string, Weapon> GetWeapons()
        {
            return weapons;
        }
    }
}
