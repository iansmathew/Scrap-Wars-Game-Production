using UnityEngine;

public class StartLevelScript : MonoBehaviour {

    [SerializeField] AudioClip gameMusic;

    AudioSource audioManager;
    

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>();
        if (audioManager.isPlaying)
        {
            audioManager.Stop();
        }

        audioManager.clip = gameMusic;
        audioManager.Play();
    }
}
