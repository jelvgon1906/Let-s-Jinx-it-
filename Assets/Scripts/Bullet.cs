using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damageQuantity = 1;
    public float activeTime;
    public float shootTime;
    [HideInInspector]public bool isPlayer;
    [SerializeField]private GameObject explosionParticle;


    //event change from active false to true
    private void OnEnable()
    {
        /*if (GetComponentsInParent<Bullet>())
        {
            isPlayer = true;
        }
        else isShootByPlayer = false;*/

        shootTime = Time.time;
    }
    void FixedUpdate()
    {
        if (Time.time - shootTime >= activeTime)
        {
            gameObject.SetActive(false);
        }
       /* AddForce(Vector3.up * shotSpeed, ForceMode.Impulse);*/
    }
    private void OnTriggerEnter(Collider other)
    {

        /*gameObject.SetActive(false);*/
        GameObject particles = Instantiate(explosionParticle, transform.position, Quaternion.identity);
        Destroy(particles, 1f);

        if (other.CompareTag("Enemy"))
        {

            other.GetComponent<ControlEnemy>().DamageEnemy(damageQuantity);
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Player") /*&& !isPlayer*/)
        {
            if (!isPlayer)
            {
                other.GetComponent<ControlPlayer>().DamagePlayer(damageQuantity);
                gameObject.SetActive(false);
        }

    }
        else if (other.CompareTag("Projectile") )
        {
            gameObject.SetActive(true);
        }
        else {
            gameObject.SetActive(false); ;
        }
         

    }


}
