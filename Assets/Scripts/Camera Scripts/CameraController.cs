using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using PlatformerCombat;

public class CameraController : MonoBehaviour
{
        [HideInInspector]
        public Transform player;
        public Tilemap tilemap;
        private Vector3 boundary1;
        private Vector3 boundary2;
        private float halfHeight;
        private float halfWidth;
        public int musicToPlay;
        private bool musicStarted;
        public float cameraYOffset;
        [HideInInspector] public CameraShakeEffect cameraShake; //camera shake effect
        public static CameraController instance;

    void Awake()
    {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
    }
     void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        //Sets the boundaries using our platformer tilemap
        if (tilemap != null)
        {
            tilemap.CompressBounds();

            boundary1 = tilemap.localBounds.min + new Vector3(halfWidth, halfHeight, 0f);
            boundary2 = tilemap.localBounds.max + new Vector3(-halfWidth, -halfHeight, 0f);

            PlayerController.instance.SetBounds(tilemap.localBounds.min, tilemap.localBounds.max);
        } 

        cameraShake = GetComponent<CameraShakeEffect>();
    }

    void FixedUpdate () 
    {
    transform.position = new Vector3(player.position.x, player.position.y + cameraYOffset, transform.position.z);

    //keep the camera inside the bounds
    transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundary1.x, boundary2.x), transform.position.y, transform.position.z);

       if (!GameManager.instance.cutSceneMusicActive && !GameManager.instance.battleActive)
   {
        if(!musicStarted)
        {
            musicStarted = true;
            AudioManager.instance.PlayBGM(musicToPlay);
        }

        if (!AudioManager.instance.bgm[musicToPlay].isPlaying)
        {
            musicStarted = false;
        }
   }

    }
}
