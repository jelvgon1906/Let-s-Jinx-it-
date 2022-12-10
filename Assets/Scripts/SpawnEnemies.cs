using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private int minX, maxX, minY, maxY, minZ, maxZ;
    [SerializeField] private float interval = 5;
    [SerializeField] private float spawnAceleration = 0.1f;
    [SerializeField] private int enemiesLeft;
    private Vector3 freePos;

    void Start()
    {
        StartCoroutine(SpawnObjects());
        /*InvokeRepeating("SpawnEnemy", 2.0f, interval);*/
    }
    /*public void SpawnEnemy()
    {
        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemiesLeft < 50)
        {
        int x, y, z;

            x = Random.Range(minX, maxX);
            y = Random.Range(minY, maxY);
            z = Random.Range(minZ, maxZ);

            Vector3 spawnPosition = new Vector3(x, y, z);

            Instantiate(enemy, spawnPosition, Quaternion.identity);

        if (interval > 1)
        {
            interval -= spawnAceleration;
        }
        }
    }*/
    IEnumerator SpawnObjects()
    {
        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;

        while (enemiesLeft < 50)
        {
            freePos =  GetFreePosition();
            Instantiate(enemy, freePos, Quaternion.identity);
            if (interval > 1)
            {
                interval -= spawnAceleration;
            }
            yield return new WaitForSeconds(interval);
        }
    }

    

    Vector3 GetFreePosition()
    {
        Vector3 position;
        Collider[] collisions = new Collider[1];
        do
        {
            position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
        }
        while (Physics.OverlapSphereNonAlloc(position, 1f, collisions) > 0);

        return position;
    }
}
