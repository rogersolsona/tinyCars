using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public Vehicle.VehicleType tag;
    public int amountToPool;
    public bool shouldExpand;
}

public class PooledObject
{
    public GameObject pooledGameObject;
    public Vehicle.VehicleType tag;

    public PooledObject(GameObject go, Vehicle.VehicleType tag)
    {
        this.pooledGameObject = go;
        this.tag = tag;
    }
}

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler SharedInstance;
    public List<ObjectPoolItem> itemsToPool;
    public List<PooledObject> pooledObjects;

    void Awake()
    {
        SharedInstance = this;
    }

    // Use this for initialization
    void Start()
    {
        pooledObjects = new List<PooledObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.SetActive(false);
                obj.transform.parent = gameObject.transform;

                PooledObject pooledObject = new PooledObject(obj, item.tag);
                pooledObjects.Add(pooledObject);
            }
        }
    }

    public GameObject GetPooledObject(Vehicle.VehicleType tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].pooledGameObject.activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i].pooledGameObject;
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    obj.transform.parent = gameObject.transform;

                    PooledObject pooledObject = new PooledObject(obj, tag);
                    pooledObjects.Add(pooledObject);

                    return obj;
                }
            }
        }
        return null;
    }
}
