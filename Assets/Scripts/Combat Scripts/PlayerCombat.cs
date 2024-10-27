using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerCombat
{
    public class PlayerCombat : Combat, ICombat
    {
        //Components
        Rigidbody2D rb;
        Animator anim;
        PlayerController playerController;

        bool canCombo;

        public override void Start()
        {
            base.Start();

            playerController = GetComponentInParent<PlayerController>();
            rb = GetComponentInParent<Rigidbody2D>();
            anim = GetComponent<Animator>();

            //Add hit event
            hitInfo += HitDetected;
        }
        //Hit method
        public override void HitDetected()
        {
            if (ColliderDetected.gameObject.tag == "Enemy")//if is enemy
            {
                Stats enemyStats = ColliderDetected.GetComponent<Stats>(); //Get data component from object
                                                                          
                //Make visual hit effect
                HitEffect hitEffect = ColliderDetected.GetComponent<HitEffect>();
                hitEffect.PlayEffect();
                Debug.Log("played effect on enemy");

                float damage = damageRange.Random(); //get 1 random value damage of 2 (min,max)
                Debug.Log("figured out damage for enemy");

                MeleeAttack(enemyStats, damage);
                Debug.Log("dealt damage to enemy");

                CameraController.instance.cameraShake.Shake();
            }
        }

        //Method for animator, when player melee attack begin
        public void OnMeleeAttackBegin(float timeToCombo)
        {
            StartCoroutine(ICombo(timeToCombo)); //Start combo system 
        }

        public void AttackForce(float forcePower)
        {
            rb.AddForce(Vector2.right * transform.localScale.x * forcePower, ForceMode2D.Impulse); //Makes jerk when attacking
        }

        //Method for animator, when player melee attack end
        public void OnMeleeAttackEnd()
        {
            StopCoroutine(ICombo(0)); //Stop combo 

            //Animator update
            anim.ResetTrigger("AttackCombo");
            anim.SetBool("MeleeAttack", false);

            canCombo = false; //block combo
            playerController.isAttacking = false;
        }

        public void OnRangedAttackEnd()
        {
            anim.SetBool("RangedAttack", false);
            playerController.isAttacking = false;
        }

        //Combo system
        IEnumerator ICombo(float comboTimer)
        {
            canCombo = false;
            yield return new WaitForSeconds(comboTimer);
            canCombo = true;

            while (canCombo)
            {
                if (Input.GetButtonDown("Attack"))
                {
                    Debug.Log("Registered attack combo button press");
                    canCombo = false;
                    anim.SetTrigger("AttackCombo");
                    StopCoroutine(ICombo(0));
                }
                yield return null;
            }
        }
    }
}
