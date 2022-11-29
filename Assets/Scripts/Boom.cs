using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Boom : MonoBehaviour
{
    [SerializeField] int explosionDamage = 15;
    [SerializeField] float radius = 5f;
    [SerializeField] float power = 10f;
    

    private void Start()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }
        Destroy(gameObject, 0.8f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<ControlEnemy>().DamageEnemy(explosionDamage);
        }
        else if (other.CompareTag("Player"))
        {
            other.GetComponent<ControlPlayer>().DamagePlayer(explosionDamage);
        }
    }
}
