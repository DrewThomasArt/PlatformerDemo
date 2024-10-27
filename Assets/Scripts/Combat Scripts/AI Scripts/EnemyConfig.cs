using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerCombat
{
    //This script for creating configurations of enemies

    [CreateAssetMenu(fileName = "Enemy Config", menuName = "Enemy/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        //Your settings
        public float HP; 
        public DoubleFloat damageRange;
    }
}

