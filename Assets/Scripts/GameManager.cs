using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Make instance of this script to be able reference from other scripts!
    public static GameManager instance;

    [Header("Currently active menus")]
    //Bools for checking if one of these menus is currently active
    public bool cutSceneActive;
    public bool cutSceneMusicActive;
    public bool gameMenuOpen;
    public bool dialogueActive;
    public bool battleActive;
    public bool fadingBetweenAreas;
    public bool eventLockActive;

    [Header("Character Bools")]
    //For checking if the player can move
    public bool confirmCanMove;
    void Start()
    {
      instance = this;  
      DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
         //Check if any menu is currently open and prevent the player from moving
        if (gameMenuOpen || dialogueActive || fadingBetweenAreas || eventLockActive || battleActive)
        {
            PlayerController.instance.canMove = false;
            confirmCanMove = PlayerController.instance.canMove;
        } else
        {
            PlayerController.instance.canMove = true;
            confirmCanMove = PlayerController.instance.canMove;
        }
    }
}



