using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    public int damageQuantity = 1;
    public float activeTime;
    public float shootTime;
    
    [HideInInspector] public bool isPlayer;
    [SerializeField] private GameObject explosionParticle;
    /*[SerializeField] private Transform objectToLook, objectThatLooks, yPos;
    private Vector3 objectToLookPosition;*/
    private Transform target;
    [SerializeField] float speed = 10f;
    [SerializeField] private bool bulletFollow = false;


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

        lookAtObject();
        /* AddForce(Vector3.up * shotSpeed, ForceMode.Impulse);*/
    }
    private void lookAtObject()
    {

        /*objectToLook = GameObject.FindGameObjectWithTag("Robot").transform;
        objectToLookPosition = objectToLook.transform.position;

        objectThatLooks.transform.LookAt(objectToLookPosition);*/

         }
    

    void Update()
    {
        if (bulletFollow)
        {

            target = GameObject.FindGameObjectWithTag("Robot").transform;
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        
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
        if (other.CompareTag("Robot"))
        {

            other.GetComponent<ControlRobot>().DamageEnemy(damageQuantity);
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
