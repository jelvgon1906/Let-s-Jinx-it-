using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;
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

    [SerializeField] private GameObject bullet;
    private GameObject target;

    [SerializeField] float speed;
    [SerializeField] private bool bulletFollow = false;
    [SerializeField] private bool rebound = false;
    float curDistance;
    [SerializeField] float rangeBounceDistance = 5;
    private bool hit;
    Rigidbody rb;
    private int bound;
    [SerializeField] private bool stun;
    [SerializeField] private TrailRenderer trailRenderer;
    private bool stunned;




    //event change from active false to true
    private void OnEnable()
    {
        
        /*if (GetComponentsInParent<Bullet>())
        {
            isPlayer = true;
        }
        else isShootByPlayer = false;*/
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
        if(GetComponent<TrailRenderer>() != null)
        {
        trailRenderer.enabled = true;
        }
        shootTime = Time.time;
    }
    

    void FixedUpdate()
    {
        if (Time.time - shootTime >= activeTime)
        {
            gameObject.SetActive(false);
        }
        if (bulletFollow) BulletFollow();
       /* lookAtObject();*/
        /* AddForce(Vector3.up * shotSpeed, ForceMode.Impulse);*/
    }
    /*private void lookAtObject()
    {

        objectToLook = GameObject.FindGameObjectWithTag("Robot").transform;
        objectToLookPosition = objectToLook.transform.position;

        objectThatLooks.transform.LookAt(objectToLookPosition);

    }*/
    

    void Update()
    {
        if (rebound) Rebound();
    }


    private void BulletFollow()
    { 
        target = FindClosestEnemy();
        if (target != null)
        {
            rb.velocity = Vector3.zero;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            transform.LookAt(target.transform);
        }
    }
        
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
    private void Rebound()
    {
        if ((curDistance <= rangeBounceDistance) && hit && (bound <= 3))
        { 
            target = FindClosestEnemy();
            
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            hit = false;
            bound++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        /*gameObject.SetActive(false);*/
        GameObject particles = Instantiate(explosionParticle, transform.position, Quaternion.identity);
        Destroy(particles, 1f);

        if (other.CompareTag("Enemy"))
        {
            hit = true;
            other.GetComponent<ControlEnemy>().DamageEnemy(damageQuantity);
            if (GetComponent<TrailRenderer>() != null)
                trailRenderer.enabled = false;
            gameObject.SetActive(false);

            if (stun)
            {
                other.GetComponent<ControlEnemy>().Stun();
            }
        }
        else if (other.CompareTag("Player") /*&& !isPlayer*/)
        {
            if (!isPlayer)
            {
                other.GetComponent<ControlPlayer>().DamagePlayer(damageQuantity);
                if (GetComponent<TrailRenderer>() != null)
                    trailRenderer.enabled = false;
                gameObject.SetActive(false);
        }

    }
        else if (other.CompareTag("Projectile") )
        {
            if (GetComponent<TrailRenderer>() != null)
                trailRenderer.enabled = true;
            gameObject.SetActive(true);
        }
        else {
            if (GetComponent<TrailRenderer>() != null)
                trailRenderer.enabled = false;
            gameObject.SetActive(false); ;
        }
         

    }


}
