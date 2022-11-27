using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool1 : MonoBehaviour
{
    public GameObject prefabObject;
    public int objectNumberOnStart;

    private List<GameObject> poolObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < objectNumberOnStart; i++) ;
        {
            createNewObject();
        }
    }

    private GameObject createNewObject()
    {
        GameObject gameObject = Instantiate(prefabObject);
        gameObject.SetActive(false);
        poolObjects.Add(gameObject);

        return gameObject;
    }

    public GameObject GetGameObject()
    {
        //find in the poolObject an object that is inactive in the 
        //game herarchy 
        GameObject gameObject = poolObjects.Find(x => x.activeInHierarchy == false);

        //if non exist create one
        if (gameObject == null)
        {
            gameObject = createNewObject();
        }

        gameObject.SetActive(true);

        return gameObject;
    }


}
