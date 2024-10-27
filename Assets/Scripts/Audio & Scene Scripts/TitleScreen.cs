using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TitleScreen : MonoBehaviour {

    public string newGameScene;
    public GameObject continueButton;
    public GameObject mainMenu;
    public TextMeshProUGUI pressStartText;
    public GameObject pressStart;
    public Button newGameBtn;
    public Button continueBtn;
    public GameObject player;
    public EventSystem es;
    public string loadGameScene;

    void Start () {
        Screen.SetResolution(1280, 720, true);
        StopCoroutine(PressStartCo());
        StartCoroutine(PressStartCo());
        ScreenFade.instance.fadeScreenObject.SetActive(false);
        //GameManager.instance.cutSceneActive = true;
    }


    void Update () {
        if (Input.GetButtonDown("Interact") || Input.GetButtonDown("Submit"))  
        {
            if (!mainMenu.activeInHierarchy)
            {
                StopCoroutine(PressStartCo());
                //PlayButtonSound(0);
                ShowMainMenu();
            }            
        }
    }

    public void PlayButtonSound (int buttonSound)
    {
        AudioManager.instance.PlaySFX(buttonSound);
    }

    public void ShowMainMenu()
    {    
        {
            if (PlayerPrefs.HasKey("Current_Scene"))
            {
                continueButton.SetActive(true);
                continueBtn.interactable = false;
                    es.SetSelectedGameObject(continueBtn.gameObject);
                    // Select the button
                    continueBtn.Select();
                    // Highlight the button
                    continueBtn.OnSelect(null);
                    StartCoroutine(WaitForButton());

            }
            else
            {
                continueButton.SetActive(true);
                continueBtn.interactable = false;
                newGameBtn.interactable = false;

                    es.SetSelectedGameObject(newGameBtn.gameObject);
                    // Select the button
                    newGameBtn.Select();
                    // Highlight the button
                    newGameBtn.OnSelect(null);
                    StartCoroutine(WaitForButton());
            }
        }    
    }
    public void Continue()
    {
        ScreenFade.instance.fadeScreenObject.SetActive(true);
        GameManager.instance.cutSceneActive = false;
        SceneManager.LoadScene(loadGameScene);
    }


    public void NewGame()
    {
        ScreenFade.instance.fadeScreenObject.SetActive(true);
        PlayerController.instance.canMove = true;
        SceneManager.LoadScene(newGameScene);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public IEnumerator PressStartCo()
    {
        while (true)
        {
            switch (pressStartText.color.a.ToString())
            {
                case "0":
                    pressStartText.color = new Color(pressStartText.color.r, pressStartText.color.g, pressStartText.color.b, 1);
                    yield return new WaitForSeconds(0.5f);
                    break;
                case "1":
                    pressStartText.color = new Color(pressStartText.color.r, pressStartText.color.g, pressStartText.color.b, 0);
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
        }
    }

    //Wait before activating the continue/new game button.
    //If this isn't called after "Press Start", the game will assume you have pressed the either the new game or the continue button!
    public IEnumerator WaitForButton()
    {
        yield return new WaitForSeconds(0.1f);

        newGameBtn.interactable = true;

        if (PlayerPrefs.HasKey("Current_Scene"))
        {
            continueBtn.interactable = true;
        }

        yield return new WaitForSeconds(0.1f);

        pressStart.SetActive(false);
        mainMenu.SetActive(true);

    }

    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
