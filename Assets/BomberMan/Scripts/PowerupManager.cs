using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BomberMan.Scripts
{
    [CreateAssetMenu(fileName ="Powerup",menuName = "Powerup",order = 10)]
    public class PowerupManager : ScriptableObject
    {
        public PowerUpAbilities AbilityType;
        public string PrefabName;
    }
}
