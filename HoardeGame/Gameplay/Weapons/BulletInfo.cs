using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoardeGame.Resources;
using Microsoft.Xna.Framework.Graphics;

namespace HoardeGame.Gameplay.Weapons
{
    public class BulletInfo
    {
        public string Texture { get; set; }

        public int Damage { get; set; }

        public int Delay { get; set; }

        public float Speed { get; set; }

        public float Offset { get; set; }

        public int Count { get; set; }

        public void Validate(IResourceProvider resourceProvider)
        {
            if (resourceProvider.GetTexture(Texture) == null)
            {
                throw new FileNotFoundException($"Texture {Texture} does not exist!");
            }
        }
    }
}
