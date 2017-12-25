using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownScript : MonoBehaviour {
    [SerializeField] Text countDownText;
	void Start()
    {
        StartCoroutine(StartTurrets());
    }

    IEnumerator StartTurrets()
    {
        yield return new WaitForSeconds(1);
        countDownText.text = "2";
        yield return new WaitForSeconds(1);
        countDownText.text = "1";
        yield return new WaitForSeconds(1);
        countDownText.text = "SURVIVE!";
        yield return new WaitForSeconds(1);
        countDownText.gameObject.SetActive(false);

        BasicTurretScript.SetCanFire(true);
        yield break;
    }
}
