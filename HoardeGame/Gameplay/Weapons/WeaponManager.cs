// <copyright file="WeaponManager.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using HoardeGame.Resources;

namespace HoardeGame.Gameplay.Weapons
{
    /// <summary>
    /// Manager for weapons
    /// </summary>
    public class WeaponManager : IWeaponProvider
    {
        /// <summary>
        /// Gets the weapon dictionary
        /// </summary>
        public Dictionary<string, WeaponInfo> Weapons { get; private set; } = new Dictionary<string, WeaponInfo>();

        /// <summary>
        /// Load weapons from a file
        /// </summary>
        /// <param name="filename">XML file to load from</param>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for asset validation</param>
        public void LoadXmlFile(string filename, IResourceProvider resourceProvider)
        {
            Debug.WriteLine("Loading weapons...");
            Weapons.Clear();

            XmlSerializer ser = new XmlSerializer(typeof(List<WeaponInfo>), new XmlRootAttribute("Weapons"));
            TextReader reader = new StreamReader(filename);

            List<WeaponInfo> weaponList = ser.Deserialize(reader) as List<WeaponInfo>;
            if (weaponList == null)
            {
                throw new InvalidDataException("Provided weapon file is not valid! ({" + filename + "})");
            }

            foreach (var weapon in weaponList)
            {
                weapon.Validate(resourceProvider);
                Weapons.Add(weapon.Id, weapon);

                Debug.WriteLine($"Loaded weapon: {weapon.Name} ({weapon.Id})");
            }

            reader.Close();
            Debug.WriteLine("Done loading weapons...");
        }

        /// <summary>
        /// Save weapons to file
        /// </summary>
        /// <param name="filename">XML file to save to</param>
        public void SaveXmlFile(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<WeaponInfo>), new XmlRootAttribute("Weapons"));
            TextWriter writer = new StreamWriter(filename);

            ser.Serialize(writer, Weapons.Values.ToList());

            writer.Close();
        }

        /// <inheritdoc/>
        public WeaponInfo GetWeapon(string name)
        {
            return !Weapons.ContainsKey(name) ? null : Weapons[name];
        }
    }
}