using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleManager : MonoBehaviour
{
    //Make instance of this script to be able reference from other scripts!
    public static BattleManager instance;
        void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
