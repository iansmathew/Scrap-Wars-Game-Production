using UnityEngine;
using UnityEngine.UI;
public class MenuColorShiftScript : MonoBehaviour {

    [SerializeField] private Camera cam;
    [SerializeField] AudioClip bgMusic;
    private GameObject[] buttons;
    private GameObject[] texts;
    bool initialize = false;

    AudioSource audioManager;

    private void Awake()
    {
        buttons = GameObject.FindGameObjectsWithTag("Button");
        texts = GameObject.FindGameObjectsWithTag("Text");
        audioManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>();
    }

    private void Start()
    {
        CancelInvoke();
        InvokeRepeating("ChangeColor", 0.5f, 0.7f);
    }

    

    void ChangeColor()
    {

        if (!initialize)
        {
            audioManager.clip = bgMusic;
            audioManager.Play();
            initialize = true;

           
        }

        return; //I PUT THIS HERE SO THAT I DONT GIVE DAVID A SEZIURE

        /* Color buttonColor = Random.ColorHSV();
        Color bgColor = Random.ColorHSV();
        Color txtColor = Random.ColorHSV();

        cam.backgroundColor = bgColor;

        foreach (GameObject button in buttons)
        {
            button.GetComponent<Image>().color = buttonColor;
        }

        foreach (GameObject text in texts)
        {
            text.GetComponent<Text>().color = txtColor;
        }
        */
    }
}
