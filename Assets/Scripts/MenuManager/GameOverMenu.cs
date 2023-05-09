using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenu;

    public ThirdPersonController player;

    private PauseMenu pauseMenu;

    public static bool isOver;
    void Start()
    {
        gameOverMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
