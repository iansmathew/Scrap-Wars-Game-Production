using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponPoolerScript {

    private static WeaponPoolerScript instance;
    private List<GameObject> objectPool;
    private GameObject prefab;
    private int size;
    public static WeaponPoolerScript Instance
    {
        get
        {
            return instance;
        }
        set
        {
            if (instance != null)
            {
                instance = value;
            }
        }
    }

    public WeaponPoolerScript(GameObject _prefab, int _size)
    {
        if (instance != null)
            return;

        instance = this;
        objectPool = new List<GameObject>();
        prefab = _prefab;
        size = _size;

        GrowPool();
    }

    private void GrowPool()
    {
        for (int i = 0; i < size; i++)
        {
            GameObject temp = GameObject.Instantiate(prefab);
            temp.SetActive(false);
            objectPool.Add(temp);
        }
    }

    public GameObject GetObjectFromPool()
    {
        for (int i =0; i < size; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            { 
                objectPool[i].SetActive(true);
                return objectPool[i];
            }
        }
        //GrowPool();

        return null;
        //return GetObjectFromPool();
    }

    public void ReturnObjectToPool(GameObject _activeObj)
    {
        _activeObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _activeObj.SetActive(false);
        
    }

    

	
}
