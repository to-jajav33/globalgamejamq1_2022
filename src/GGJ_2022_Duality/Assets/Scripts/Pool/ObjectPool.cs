using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private PoolObject prefab;

    public string PrefabID { get { return prefab.name; } }

    private Queue<PoolObject> objects = new Queue<PoolObject>();

    public PoolObject Get()
    {
        if (objects.Count == 0)
            AddObjects(1);
        PoolObject p = objects.Dequeue();
        p.gameObject.SetActive(true);
        return p;
    }

    public void ReturnToPool(PoolObject objectToReturn)
    {
        objectToReturn.transform.parent = transform;
        objectToReturn.gameObject.SetActive(false);
        objects.Enqueue(objectToReturn);
    }

    private void AddObjects(int count)
    {
        var newObject = Instantiate(prefab);
        newObject.gameObject.SetActive(false);
        objects.Enqueue(newObject);
        newObject.GetComponent<PoolObject>().Pool = this;
    }
}
