using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("iueurhdf");
            PauseGame();
        }
    }
    
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);

    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
