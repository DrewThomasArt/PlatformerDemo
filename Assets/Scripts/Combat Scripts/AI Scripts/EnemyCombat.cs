using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerCombat
{
    public class EnemyCombat : Combat, ICombat
    {
        [Header("Config")]
        public EnemyConfig enemyConfig; //Config, drap and drop in inspector your config
        
        //Components
        Rigidbody2D rb;
        Animator anim;

        [Header("Variables")]
        public float attackRateSpeed; //Rate of enemy attack
        float attackRateTimer; //Rate local timer

        [HideInInspector] public bool isAttacking; //check status of attack

        public override void Start()
        {
            base.Start(); //parent start method 

            //Get components
            rb = GetComponentInParent<Rigidbody2D>();
            anim = GetComponent<Animator>();

            //Set enemy data from config
            damageRange = new DoubleFloat(enemyConfig.damageRange.min, enemyConfig.damageRange.max);
            stats.statsData.HP = enemyConfig.HP;

            //add method hitInfo to enent, need for setup logic when enemy get hit
            hitInfo += HitDetected;
        }

        //Hit method
        public override void HitDetected()
        {
            if (ColliderDetected.gameObject.tag == "Player") //if is player
            {
                Stats playerStats = ColliderDetected.GetComponent<Stats>(); //Get data component from object

                //Make visual hit effect
                HitEffect hitEffect = ColliderDetected.GetComponent<HitEffect>();
                hitEffect.PlayEffect();

                float damage = damageRange.Random(); //get 1 random value damage of 2 (min, max)

                MeleeAttack(playerStats, damage); //Damage

                CameraController.instance.cameraShake.Shake();
            }
        }

        //Method for animator, when enemy melee attack ends
        public void OnMeleeAttackEnd()
        {
            isAttacking = false; //Attack status
            anim.SetBool("MeleeAttack", false); //Animator bool MeleeAttack = false
        }

        public void AttackForce(float forcePower)
        {
            // Here you can add any logic of the moment of impact, similar to that of the player.
        }

        public override void RangedAttack()
        {
            base.RangedAttack();
        }
        //Method for animator, when enemy range attack end
        public void OnRangeAttackEnd()
        {
            anim.SetBool("RangeAttack", false);
        }

        void Update()
        {
            if (isAttacking) 
            {
                if (attackRateTimer > 0) //attack rate timer
                {
                    attackRateTimer -= Time.deltaTime; //attack rate timer - 1 every frame(you can change Update to FixedUpdate for everysecond)
                }
                else
                {
                    attackRateTimer = attackRateSpeed; //set attack rate timer to attack rate time
                    anim.SetBool("MeleeAttack", true);
                }
            }
        }
    }
}