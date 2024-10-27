using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerCombat
{
    public class Combat : MonoBehaviour
    {
        [HideInInspector] public CombatTrigger combatTrigger;
        [HideInInspector] public Stats stats; //Data component
        public GameObject rangedWeaponPrefab;
        public Transform rangedWeaponStartPos; //Weapon spawn position
        public Collider2D ColliderDetected { get; set; } //Here we write the collider which was detected

        //Hit event
        public delegate void HitInfo();
        public HitInfo hitInfo;
        public DoubleFloat damageRange; //min and max damage

        public bool isPlayer; //Check player or not, set in inspector

        public virtual void Start()
        {
            stats = GetComponent<Stats>();

            combatTrigger = GetComponentInChildren<CombatTrigger>();

            combatTrigger.combat = this;
            combatTrigger.OnHit += HitDetected; //Add event
        }

        //Method to call Hit event
        public virtual void HitDetected()
        {
            hitInfo();
        }

        //Melee attack method
        public virtual void MeleeAttack(Stats targetStats, float damage)
        {
            targetStats.GetDamage(damage);
        }

        //Range attack method
        public virtual void RangedAttack()
        {
            GameObject rangedWeaponGO = Instantiate(rangedWeaponPrefab, rangedWeaponStartPos.position, Quaternion.identity); //spawn projectile

            float rotAngle = Mathf.Acos(transform.localScale.x) * Mathf.Rad2Deg; //calculate rotation angle
            rangedWeaponGO.transform.Rotate(new Vector3(0, rotAngle, 0)); //rotate

            RangedWeapon rangedWeapon = rangedWeaponGO.GetComponent<RangedWeapon>(); 

            rangedWeapon.damage = damageRange.Random(); //Get random damage
            rangedWeapon.fromPlayer = isPlayer;
        }

    }
}