using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HoardeGame.Entities;
using HoardeGame.Resources;
using HoardeGame.Themes;

namespace HoardeGame.Weapons
{
    public class Weapon
    {
        /// <summary>
        /// Gets or sets the readable name of the theme
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ID of the theme
        /// </summary>
        [XmlAttribute]
        public string id { get; set; }

        /// <summary>
        /// Gets or sets the name of the floor texture
        /// </summary>
        public string Texture { get; set; }

        /// <summary>
        /// Gets or sets the list of enemies that can spawn in this theme
        /// </summary>
        public List<EntityBullet> Bullets { get; set; }

        /// <summary>
        /// Validates the theme
        /// </summary>
        /// <param name="resourceProvider"><see cref="IResourceProvider"/> for checking assets</param>
        public void Validate(IResourceProvider resourceProvider)
        {

        }
    }
}
