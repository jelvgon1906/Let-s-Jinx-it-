using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private int minX, maxX, minY, maxY, minZ, maxZ;
    [SerializeField] private float interval = 5;
    [SerializeField] private float spawnAceleration = 0.1f;
    
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 2.0f, interval);
    }
    public void SpawnEnemy()
    {
            int x, y, z;

            x = Random.Range(minX, maxX);
            y = Random.Range(minY, maxY);
            z = Random.Range(minZ, maxZ);

            Vector3 spawnPosition = new Vector3(x, y, z);

            Instantiate(enemy, spawnPosition, Quaternion.identity);

        if (interval >= 1)
        {
            interval -= spawnAceleration;
        }
    }

    








}
