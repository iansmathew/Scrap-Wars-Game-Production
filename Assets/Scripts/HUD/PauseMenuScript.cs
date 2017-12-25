using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseMenuScript : MonoBehaviour {
    public Canvas pauseMenuCanvas;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (!pauseMenuCanvas.gameObject.activeInHierarchy)
        {
            Time.timeScale = 0;
            pauseMenuCanvas.gameObject.SetActive(true);
        }

        else if (pauseMenuCanvas.gameObject.activeInHierarchy)
        {
            pauseMenuCanvas.gameObject.SetActive(false);
            Time.timeScale = 1;

        }
    }

    public void QuitButton()
    {
        ResetStaticScripts.Instance.ResetStatic();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    
}
