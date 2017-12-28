using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineHazardScript : MonoBehaviour {

    public float explosionForce = 5.0f;
    public int damageAmt = 15;

    [SerializeField] GameObject explosionPrefab;

    private void Start()
    {
        HazardManagerScript.mineCount++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthScript.Instance.TakeDamage(damageAmt);
            other.GetComponent<Rigidbody>().AddExplosionForce(explosionForce * 100, transform.position, 0.7f);
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject); 
        }
    }

    private void OnDestroy()
    {
        HazardManagerScript.mineCount--;
    }



}
