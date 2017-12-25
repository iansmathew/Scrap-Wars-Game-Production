using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthScript : MonoBehaviour {
    [Header("Health Bar Values")]
    public Image healthBar;
    public bool isDead = false;
    public float healthBarChangeSpeed = 10.0f;
    [SerializeField] GameObject carMesh;
    [SerializeField] GameObject brokenPrefab;

    private float playerMaxHealth = 100.0f;
    private float playerCurrHealth = 0.0f;
    private bool updateSlider = false;

    //PowerLevel
    private float powerLevel = -1.0f;
    public float PowerLevel
    {
        get
        {
            return powerLevel;
        }

        set
        {
            if (value < 0 || value > 3)
            {
                Debug.LogError("Setting invalid power level");
                return;
            }

            if (value != 0.5)
            {
                powerLevel = Mathf.RoundToInt(value);
            }
            else
            {
                powerLevel = value;
            }
        }
    }

    [Header("Audio Components")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip bulletImpactClip;


    //Script Reference Variables
    private HoverCarControl playerControllerScript;

    //Public Instance Variables
    private static PlayerHealthScript instance;
    public static PlayerHealthScript Instance
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
            return;

        instance = this;

        playerControllerScript = GetComponent<HoverCarControl>();
        playerCurrHealth = playerMaxHealth;
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //HUDScript.Instance.UpdateTurretText();
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            IncreaseHealth(10);
        }

        UpdateSlider();
    }

    public void TakeDamage(float dmgAmt)
    {
        audioSource.PlayOneShot(bulletImpactClip);
        updateSlider = true;
        if (!isDead)
            playerCurrHealth -= dmgAmt;
        if (playerCurrHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void IncreaseHealth(int healthAmt)
    {
        updateSlider = true;
        if (playerCurrHealth + healthAmt > 100.0f)
            playerCurrHealth = 100.0f;
        else
            playerCurrHealth += healthAmt;
    }

    private void Death()
    {
        if (playerCurrHealth > 0)
        {
            Debug.Log("Health is not empty. Death func called regardless??");
            return;
        }

        //TODO: Implement death sound

        playerCurrHealth = 0;
        isDead = true;

        playerControllerScript.enabled = false;
        GunPodScript.Instance.enabled = false;

        HUDScript.Instance.GameOver(false);
        //carMesh.SetActive(false);
        //Instantiate(brokenPrefab, transform.position, transform.rotation);
        
    }

    private void UpdateSlider()
    {
        if (updateSlider)
        {
            float scaleX = Mathf.MoveTowards(healthBar.transform.localScale.x, playerCurrHealth, Time.deltaTime * healthBarChangeSpeed);
            healthBar.transform.localScale = new Vector3(scaleX, healthBar.transform.localScale.y, 0);

            if (scaleX == playerCurrHealth)
                updateSlider = false;
        }
    }
}
