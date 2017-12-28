using UnityEngine;
using UnityEngine.UI;
public class MenuColorShiftScript : MonoBehaviour {

    [SerializeField] private Camera cam;
    [SerializeField] AudioClip bgMusic;
    bool initialize = false;

    AudioSource audioManager;

    private void Awake()
    {
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
        return;
    }
}
