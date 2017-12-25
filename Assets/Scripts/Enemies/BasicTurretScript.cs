using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretScript : MonoBehaviour {

    public float health = 100.0f;

    [SerializeField] float seekRange = 5.0f;
    [SerializeField] float fireRate = 1.5f;
    [SerializeField] float powerLevel = -1.0f;
    [SerializeField] Transform baseCircle;
    [SerializeField] Transform turretHead;
    [SerializeField] Transform spawnBarrel;
    [SerializeField] GameObject brokenObject;

    private bool playerDetected = false;

    static Transform player;

    public LayerMask carLayer;
    public GameObject bulletPrefab;

    WeaponPoolerScript weaponPool;
    static bool canFire = false;
    float lastFire;
    float bulletForce = 1000.0f;

	void Awake ()
    {
        weaponPool = new WeaponPoolerScript(bulletPrefab, 350);
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (powerLevel < 0 || powerLevel > 3)
            Debug.LogError("Invalid Power Level set", gameObject);
	}
	
	void Update ()
    {
        AimAtPlayer();
        if (Vector3.Distance(transform.position, player.position) <= seekRange)
            playerDetected = true;
        else
            playerDetected = false;
     

       if (canFire && playerDetected && Time.time > lastFire)
        {
            lastFire = Time.time + fireRate;
            GameObject bullet = WeaponPoolerScript.Instance.GetObjectFromPool();
            bullet.transform.position = spawnBarrel.position;
            bullet.transform.rotation = spawnBarrel.rotation;
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletForce);
            StartCoroutine(DestroyBullet(bullet));
        }

       if (health <= 0)
        {
            //TODO: Implement Turret Destroyed sound
            HUDScript.Instance.UpdateTurretText();
            Instantiate(brokenObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
       

	}

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            health -= (PlayerHealthScript.Instance.PowerLevel != powerLevel) ? 1.0f : 2.0f;
        }
    }

    private void AimAtPlayer()
    {
        Vector3 destVec = player.position - baseCircle.position;
        destVec.y = baseCircle.position.y;

        Vector3 tDestVec = player.position - turretHead.position;

        baseCircle.rotation = Quaternion.LookRotation(destVec);
        turretHead.rotation = Quaternion.LookRotation(tDestVec);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, seekRadius);
        //Gizmos.DrawWireCube(transform.position, Vector3.one * seekRadius);
        Gizmos.color = (playerDetected) ? Color.red : Color.white;
        if (player != null)
            Gizmos.DrawLine(transform.position, player.position);
    }

    IEnumerator DestroyBullet(GameObject _bullet)
    {
        yield return new WaitForSeconds(3.0f);
        WeaponPoolerScript.Instance.ReturnObjectToPool(_bullet);
    }

    public static void SetCanFire(bool _canFire)
    {
        canFire = _canFire;
    }

}
