using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    Dictionary<string,Queue<GameObject>> _ObjectPool = new Dictionary<string, Queue<GameObject>>();

    public GameObject GetObject(GameObject prefab)
    {
        if(_ObjectPool.TryGetValue(prefab.name, out Queue<GameObject> objectList))
        {
            if(objectList.Count == 0)
            {
                return CreateNewGameObject(prefab);
            }
            else
            {
                GameObject obj = objectList.Dequeue();
                obj.SetActive(true);
                return obj;
            }
        }
        else
        {
            return CreateNewGameObject(prefab);
        }
    }

    GameObject CreateNewGameObject(GameObject prefab)
    {
        GameObject newGameObject = Instantiate(prefab);
        newGameObject.name = prefab.name;
        return newGameObject;
    }

    public void ReturnGameObject(GameObject gameObject)
    {
        if(_ObjectPool.TryGetValue(gameObject.name, out Queue<GameObject> objectList))
        {
            objectList.Enqueue(gameObject);
        }
        else
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            queue.Enqueue(gameObject);
            _ObjectPool.Add(gameObject.name, queue);
        }

        gameObject.SetActive(false);
    }
}
