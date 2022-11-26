using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public int enemyNumber;
    public GameObject enemyObject;
    public GameObject ground;
    public int enemyCount;

    private void Start()
    {
        StartCoroutine(GenerateEnemies());
    }
    IEnumerator GenerateEnemies()
    {

        while (enemyCount < enemyNumber)
        {
            enemyNumber++;
            //instanciar en posicion de los meshes (puedo añadir lo de apuntes de en una area random en esfera)
            Vector3 randomPos = new Vector3(Random.Range(ground.GetComponent<MeshCollider>().bounds.min.x, ground.GetComponent<MeshCollider>().bounds.max.x),
                                            1,
                                            Random.Range(ground.GetComponent<MeshCollider>().bounds.min.z, ground.GetComponent<MeshCollider>().bounds.max.z));
            Instantiate(enemyObject);
            yield return new WaitForSeconds(1.5f);
        }
        yield return null;


    }
}
