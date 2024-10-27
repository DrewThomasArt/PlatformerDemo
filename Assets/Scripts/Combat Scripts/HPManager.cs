using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class HPManager : MonoBehaviour
    {
        public static HPManager instance;
        public GameObject hpBar;
        public Image hp;
        void Awake()
        {
            if (instance == null)
            {
            instance = this;
            } else
            {
                if (instance != this)
                {
                Destroy(gameObject);
                }
            }

        SceneManager.activeSceneChanged += OnSceneChange;
        }

        private void OnSceneChange(Scene current, Scene next)
        {
              if (SceneManager.GetActiveScene().name == "TitlePage")
            {
                hpBar.gameObject.SetActive(false);
            } else {
                hpBar.gameObject.SetActive(true);
            }
        }

        private void Start()
        {
            UpdateHP(Stats.instance.statsData.HP); //clear UI
        }

        //Player hp update method
        public void UpdateHP(float hpValue)
        {
            hp.fillAmount = hpValue / 100;
        }
    }
