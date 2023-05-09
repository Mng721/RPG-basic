using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject endMenu;
    public GameObject gameOverMenu;

    public ThirdPersonController player;

    public static bool isPaused;
    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if (!isPaused) {
                PauseGame();
            } else {
                ResumeGame();
            }
        }
        GameOver();
    }

    public void PauseGame() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MenuScene");
        isPaused = false;
        player.CurrentHealth = player.MaxHealth;
    }

    public void EndGame() {
        endMenu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void PlayAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isPaused = false;
        player.CurrentHealth = player.MaxHealth;
        Time.timeScale = 1f;
    }

    public void GameOver() {
        if (player.CurrentHealth <= 0) {
            gameOverMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }
}
