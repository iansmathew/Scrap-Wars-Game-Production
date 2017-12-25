using System.Collections;
using UnityEngine;

public class WallHazardScript : MonoBehaviour {
    [Header("Hazard Vairables")]
    public float health = 100.0f;
    private int damageAmt = 10;
    private float risingSpeed = 0.5f;

    [Header("Destruction Variables")]
    [SerializeField] GameObject brokenObject;

	void Start () {
        HazardManagerScript.wallCount++;
        StartCoroutine(RiseUp());
	}
	
	void Update () {
        if (health <= 0)
        {
            Instantiate(brokenObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "PlayerBullet" || other.gameObject.name == "Bullet")
        {
            health -= 7.5f;
        }
        else if (other.gameObject.tag == "Player")
        {
            PlayerHealthScript.Instance.TakeDamage(damageAmt);
            health = 0.0f;
        }
    }

    IEnumerator RiseUp()
    {
        while (true)
        {
            if (transform.position.y >= 0.0f)
                yield break;

            transform.position += Vector3.up * risingSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private void OnDestroy()
    {
        HazardManagerScript.wallCount--;
    }
}
