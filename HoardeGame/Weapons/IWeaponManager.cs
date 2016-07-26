using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoardeGame.Weapons
{
    public interface IWeaponProvider
    {
        Weapon GetWeapon(string name);
        Dictionary<string, Weapon> GetWeapons();
    }
}
