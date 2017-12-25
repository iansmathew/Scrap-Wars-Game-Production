using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticScripts : MonoBehaviour {

    private static ResetStaticScripts instance;
    public static ResetStaticScripts Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ResetStatic()
    {
        PlayerBulletPoolScript.Instance = null;
        WeaponPoolerScript.Instance = null;
        GunPodScript.Instance = null;
        PlayerHealthScript.Instance = null;
        HUDScript.Instance = null;
        BasicTurretScript.SetCanFire(false);
    }
}
