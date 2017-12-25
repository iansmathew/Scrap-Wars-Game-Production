using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour {

    [Header("Turret Variables")]
    [SerializeField] private Text turretCountText;
    private int turretCount = 10;

    [Header("Timer Variables")]
    [SerializeField] private Text timerText;
    private int seconds = 60;
    private int minutes = 9;
    private string secondsText;
    private string minutesText;

    [Header("Reset Car Panel")]
    [SerializeField] private GameObject resetCarPanel;

    [Header("Countdown / Game Over Variables")]
    [SerializeField] Text victoryText;

    //Misc 
    private const int delayToGameOver = 3;

    
    

    public static HUDScript Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }
    private static HUDScript instance;

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (seconds > 0 || minutes > 0)
        {
            yield return new WaitForSeconds(1);

            if (seconds > 0)
            {
                --seconds;
            }
            else
            {
                --minutes;
                seconds = 59;
            }

            secondsText = (seconds < 10) ? "0" + seconds.ToString() : seconds.ToString();
            minutesText = (minutes < 10) ? "0" + minutes.ToString() : minutes.ToString();

            timerText.text = string.Format("Timer: {0}:{1}", minutesText, secondsText);
        }
        GameOver(false); //lost game due to time running out
    }

    public void GameOver(bool winCondition)
    {
        StopCoroutine(Timer());
        victoryText.text = winCondition ? "Victory!!" : "Defeat..";
        victoryText.gameObject.SetActive(true);

        StartCoroutine(SwitchToMenu());
    }

    private IEnumerator SwitchToMenu()
    {
        yield return new WaitForSeconds(delayToGameOver);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        yield break;
    }

    public void UpdateTurretText()
    {
        turretCountText.text = "Turrets Left: " + --turretCount;

        if (turretCount <= 0)
        {
            GameOver(true); //win game by destroying turrets
            return;
        }
    }

    public void SetResetCarPanel(bool set)
    {
        resetCarPanel.SetActive(set);
    }

}
