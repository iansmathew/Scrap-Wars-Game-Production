using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {

    List<GameObject> listOfPlayerChildren;
    GameObject player;

    [SerializeField] Material powerUpMat;
    [SerializeField] AudioClip powerUpSound;
    private AudioSource audioSource;

    private void Awake()
    {
        this.transform.GetChild(0).GetComponent<Renderer>().material = powerUpMat;
        player = GameObject.FindGameObjectWithTag("Player");
        listOfPlayerChildren = new List<GameObject>();

        GetListOfPlayerChildren(player);

        audioSource = GetComponent<AudioSource>();
    }


    void GetListOfPlayerChildren(GameObject parent)
    {
        foreach(Transform child in parent.transform)
        { 
            if (child == null)
                continue;

            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                listOfPlayerChildren.Add(child.gameObject);
            }

            GetListOfPlayerChildren(child.gameObject);
        }

        listOfPlayerChildren.Reverse();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(SetColor());
            audioSource.PlayOneShot(powerUpSound); //TODO: Change powerup sound to more subtle sound

        }
    }

    private IEnumerator SetColor()
    {

        if (listOfPlayerChildren[0].GetComponent<Renderer>().material.color == powerUpMat.color)
        {
            //do not run function if same material
            yield break;
        }

        foreach (GameObject obj in listOfPlayerChildren)
        {
            obj.GetComponent<Renderer>().material = powerUpMat;
            yield return null;
        }

        float pLevel = -1;
        switch (powerUpMat.name)
        {
            case "Turret_.5Mat":
                pLevel = 0.5f;
                break;
            case "Turret1Mat":
                pLevel = 1.0f;
                break;
            case "Turret2Mat":
                pLevel = 2.0f;
                break;
            case "Turret3Mat":
                pLevel = 3.0f;
                break;
            default:
                Debug.LogError("Invalid power level set");
                break;
        }

        PlayerHealthScript.Instance.PowerLevel = pLevel;
        yield break;
    }

}
