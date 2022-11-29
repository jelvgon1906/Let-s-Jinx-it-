using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class ControlEnemy : MonoBehaviour
{
    [Header("EnemyData")]
    public EnemyData EnemyData;
    public int currentLife;
    public int maxLife;
    public int enemyScorePoints;
    [SerializeField] private Animator animator;
    Rigidbody rb;

    [Header("Movement")]
    public float speed;
    public float maxAttackRange;
    public float minAttackRange;
    public float yPathOffset;
    public float followRange;
    public bool alwaysFollow;
    GameObject enemy;
    float movement;

    private List<Vector3> listPath;
    Vector3 lastPosition;

    private WeaponController weaponController;
    private ControlPlayer target;
    public static ControlEnemy instance;
    public bool stunned;

    [Header("Hit")]
    private float lastHitTime;
    public float hitFrequency;
    [SerializeField] private int damageQuantity;



    [Header("SelectAI")]
    [SerializeField] private bool Range;
    [SerializeField] private bool Mele;
    private float startSpeed;

    private void Start()
    {
        instance = this;
        weaponController = GetComponent<WeaponController>();
        target = FindObjectOfType<ControlPlayer>();
        enemy = GetComponent<GameObject>();
        rb = GetComponent<Rigidbody>();
        InvokeRepeating(nameof(UpdatePaths), 0.0f, 0.25f);
        startSpeed = speed;
    }
    /// <summary>
    /// update every 0.5 seconds the path to target
    /// </summary>

    private void UpdatePaths()
    {
        //Create a meshpath object
        NavMeshPath navMeshPath = new NavMeshPath();
        // calculate all points of the path to player
        NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, navMeshPath);
        //Convert all the points to the List
        listPath = navMeshPath.corners.ToList();

    }

    private void Update()
    {
        if (!stunned)
        {
            if (Range) RangedEnemy();
            if (Mele) MeleEnemy();
        }
        movement = Vector3.Distance(transform.position, lastPosition);
        //lookat pero con mates
        /*Vector3 direction = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.z)* Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * angle;*/
    }
    private void LateUpdate()
    {
        lastPosition = transform.position;
    }

    
    private void RangedEnemy()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        
            if (distance <= followRange && distance > maxAttackRange || alwaysFollow)
            {
                transform.LookAt(target.transform);
                ReachTarget();

            }
            else if (distance <= maxAttackRange && distance > minAttackRange)
            {
                transform.LookAt(target.transform);
                if (weaponController.canShoot())
                {
                    weaponController.Shoot();
                }
            }
            else if (distance <= minAttackRange)
            {
                transform.LookAt(target.transform);
                RunFromTarget();
                if (weaponController.canShoot())
                {
                    weaponController.Shoot();
                }
            }
        
    }

    private void MeleEnemy()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        
        if (distance <= followRange && distance > maxAttackRange || alwaysFollow)
        {
            transform.LookAt(target.transform);
            ReachTarget();

        }
        else if (distance <= maxAttackRange && distance > minAttackRange)
        {
            transform.LookAt(target.transform);
            if (canHit() == true)
            {
                lastHitTime = Time.time;
                animator.SetTrigger("Punch");
                StartCoroutine(meledmg());
            }
            else ReachTarget();

        }
        else if (distance <= minAttackRange)
        {
            if (canHit() == true)
            {
                lastHitTime = Time.time;
                animator.SetTrigger("Punch");
                StartCoroutine(meledmg());
            }
            else
            {
                transform.LookAt(target.transform);
            }
        }

        
    }
    IEnumerator meledmg()
    {
        yield return new WaitForSeconds(0.3f);
        target.DamagePlayer(damageQuantity);
    }
    public void Stun()
    {
        stunned = true;
        if(animator != null)
        animator.SetTrigger("stunned");
        rb.velocity = Vector3.zero;
        Invoke("Reset", 3f);
    }
    private void Reset()
    {
        stunned = false;   
    }


    public bool canHit()
    {
        if ((Time.time - lastHitTime >= hitFrequency) && !GameManager.instance.gamePaused)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// move the enemy to reac the target following the path calculated
    /// </summary>
    private void ReachTarget()
    {
        speed = startSpeed;
        if (animator != null)
            animator.SetFloat("Moving", movement);
        
        //if is not a path dont do anything
        if (listPath.Count == 0) return;

        //calculate new pos at listpath
        transform.position = Vector3.MoveTowards(transform.position, listPath[0] + new Vector3(0, yPathOffset, 0), speed * Time.deltaTime);

        //everytime reach position remove it after
        if (transform.position == listPath[0] + new Vector3(0, yPathOffset, 0))
        {
            listPath.RemoveAt(0);
        }
    }

    private void RunFromTarget()
    {
        //if is not a path dont do anything
        if (listPath.Count == 0) return;

        //calculate new pos at listpath
        transform.position = Vector3.MoveTowards(transform.position, -listPath[0] + new Vector3(0, yPathOffset, 0) , speed * Time.deltaTime);

        //everytime reach position remove it after
        if (transform.position == listPath[0] + new Vector3(0, yPathOffset, 0))
        {
            listPath.RemoveAt(0);
        }
    }

    public void DamageEnemy(int quantity)
    {
        GameManager.instance.UpdateScore(enemyScorePoints);

        currentLife -= quantity;
        if (currentLife <= 0)
            Destroy(gameObject);
    }
}

