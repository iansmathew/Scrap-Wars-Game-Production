using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GunPodScript : MonoBehaviour {
    private Camera cam; 
    private int layerMaskInvert;

    [Header("GunPods")]
    [SerializeField] private Transform[] GunPodArray;

    [Header("Bullet Properties")]
    [SerializeField]
    private GameObject bulletPrefab;

    public int maxAmmo = 50;
    public float bulletForce = 6000.0f;
    public float fireRate = 0.1f;
    private float lastFire = 0.0f;

    [Header("Ammo Properties")]
    public Image energyBar;
    private float fireTimer = 100.0f;
    private float fireTimerDelta = 0.5f;
    private float fireTimerRefill = 0.7f;
    private PlayerBulletPoolScript weaponPoolerScript;

    [Header("Audio Components")]
    private AudioSource gunFireAudioSource;
    public AudioClip bulletFireClip;


    //Instance References
    private static GunPodScript instance;
    public static GunPodScript Instance
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

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
        weaponPoolerScript = new PlayerBulletPoolScript(bulletPrefab, maxAmmo);
        gunFireAudioSource = GetComponent<AudioSource>();
    }

    void Start () {
        cam = Camera.main;

        layerMaskInvert = 1 << gameObject.layer;
        layerMaskInvert = ~layerMaskInvert;  //hit ever layer but Car
	}
	
	void Update () {
        Ray vRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
       
        if (Physics.Raycast(vRay, out hit, 1000.0f, layerMaskInvert))
        {
            foreach (Transform gunPod in GunPodArray)
            {
                gunPod.LookAt(hit.point);
            }

            Fire();


            if (Input.GetMouseButton(0))
            {
                fireTimer -= fireTimerDelta;
            }
        }

        if (!Input.GetMouseButton(0))
        {
            fireTimer += fireTimerRefill;
        }
        fireTimer = Mathf.Clamp(fireTimer, 0.0f, 100.0f);
        energyBar.transform.localScale = new Vector3(fireTimer, energyBar.transform.localScale.y, 0.0f);
    }

    private void Fire()
    {
        if (Input.GetMouseButton(0) && Time.time > lastFire && fireTimer > 0)
        {
            lastFire = Time.time + fireRate;

            gunFireAudioSource.PlayOneShot(bulletFireClip, 0.3f);

            foreach (Transform gunPod in GunPodArray)
            { 
                GameObject bullet = weaponPoolerScript.GetObjectFromPool();
                bullet.transform.position = gunPod.position;
                bullet.transform.rotation = gunPod.rotation;
                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletForce);

                StartCoroutine(DestroyBullet(bullet));
            }
        }

    }

    public static IEnumerator DestroyBullet(GameObject _bullet)
    {
        yield return new WaitForSeconds(2.0f);
        PlayerBulletPoolScript.Instance.ReturnObjectToPool(_bullet);
    }
}
