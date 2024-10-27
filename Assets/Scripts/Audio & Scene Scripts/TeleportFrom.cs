using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportFrom : MonoBehaviour {

    [HideInInspector]
    public string teleportName;
    public UnityEvent onSpawn;

    void Start () {
        //Checks which teleporter the player just came and if it matches this one
        if(teleportName == PlayerController.instance.areaTransitionName)
        {
            Debug.Log("teleported");
            // Puts the player in front of the teleporter after they load to scene
            PlayerController.instance.transform.position = transform.position;
        }

        ScreenFade.instance.FadeFromBlack();
        GameManager.instance.fadingBetweenAreas = false;

        onSpawn?.Invoke();
    }
}
