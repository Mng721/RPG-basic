using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public PauseMenu pauseMenu;
    private void OnTriggerEnter(Collider other) {
        pauseMenu.EndGame();
    }
}
