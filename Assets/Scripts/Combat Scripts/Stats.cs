using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    public class Stats : MonoBehaviour
    {
        //Death event call when player or somebody hp <= 0 
        public delegate void DeathEvent();
        public DeathEvent OnDeath;
        public StatsData statsData = new StatsData();
        public static Stats instance;

        void Start()
        {
         instance = this;  
        }

        //Damage method
        public void GetDamage(float damage)
        {
            statsData.HP -= damage; //Subtract damage from hp

            if (gameObject.tag == "Player") //if player
                HPManager.instance.UpdateHP(statsData.HP); //Update UI

            if (statsData.HP <= 0) 
            {
                Death();
            }
        }

        //Death method
        public void Death()
        {
            OnDeath();
        }

    }

    [Serializable]
    public struct StatsData
    {
        public float HP;
    }
