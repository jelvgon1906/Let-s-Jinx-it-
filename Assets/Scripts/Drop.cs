using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    float rotation = 0;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private bool missile;
    [SerializeField] private bool bullets;
    [SerializeField] private bool healthPotion;
    public int ammoQuantity;
    public int healthQuantity;
    [SerializeField] private float activeDropTime;
    private float dropTime;


    private void Start()
    {
        dropTime = Time.time;
    }


    void FixedUpdate()
    {
        Rotation();
        if (Time.time - dropTime >= activeDropTime)
        {
            Destroy(gameObject);
        }
    }
    
    private void Rotation()
    {

        rotation += RotationSpeed;

        if (rotation > 360.0f)
        {
            rotation = 0.0f;
        }
        transform.localRotation = Quaternion.Euler(0, rotation, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (missile)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponentInChildren<FishBonesController>().AddAmmo(ammoQuantity);
                Destroy(gameObject);
            }
        }

        if (bullets)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponentInChildren<PowPowController>().AddAmmo(ammoQuantity);
                Destroy(gameObject);
            }
        }
        if (healthPotion)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<ControlPlayer>().DamagePlayer(-healthQuantity);
                Destroy(gameObject);
            }
        }
    }
}
