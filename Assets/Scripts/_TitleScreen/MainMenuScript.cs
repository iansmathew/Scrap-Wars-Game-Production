using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {
    public Animator camAnim;
    public GameObject mainMenuPanel;
    public GameObject helpMenuPanel;


    public void StartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void HelpButton()
    {
        Debug.Log("CLICKED");
        //helpMenuPanel.SetActive(true);
        camAnim.SetTrigger("moveToHelp");
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void HelpBackButton()
    {
        camAnim.SetTrigger("moveToMain");
    }
}
