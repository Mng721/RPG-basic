using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    private void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void playAgainButton() {
        SceneManager.LoadScene(1);
    }

    public void goToMainMenuButton() {
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
