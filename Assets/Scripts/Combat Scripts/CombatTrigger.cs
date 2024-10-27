using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Namespaces can be used to avoid conflicts between classes of the same name. Useful for working collaberatively or on big projects
namespace PlatformerCombat
{
    public class CombatTrigger : MonoBehaviour
    {
        public Combat combat;

        //Hit method
        public delegate void HitDetect();
        public HitDetect OnHit;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            combat.ColliderDetected = collision;
            OnHit(); //Call event
        }

    }
}
