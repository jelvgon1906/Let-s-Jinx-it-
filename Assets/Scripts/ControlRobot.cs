using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class ControlRobot : MonoBehaviour
{
    [Header("EnemyData")]
    public EnemyData EnemyData;
    public int currentLife;
    public int maxLife;
    public int enemyScorePoints;

    [Header("Movement")]
    public float speed;
    public float maxAttackRange;
    public float minAttackRange;
    public float yPathOffset;
    public float followRange;
    public bool alwaysFollow;
    GameObject robot;

    [Header("Hit")]
    private float lastHitTime;
    public float hitFrequency;
    public int damageQuantity;


    private Animator animator;

    private List<Vector3> listPath;

    private ControlPlayer target;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        target = FindObjectOfType<ControlPlayer>();
        robot = GetComponent<GameObject>();

        InvokeRepeating(nameof(UpdatePaths), 0.0f, 0.25f);
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
    void Update()
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
                FindObjectOfType<ControlPlayer>().DamagePlayer(damageQuantity);
            }

        }
        else if (distance <= minAttackRange)
        {
            animator.SetTrigger("Punch");
            FindObjectOfType<ControlPlayer>().DamagePlayer(damageQuantity);
            transform.LookAt(target.transform);
            RunFromTarget();
        }

        //lookat pero con mates
        /*Vector3 direction = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.z)* Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * angle;*/
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
        animator.SetTrigger("Velocity");
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
        transform.position = Vector3.MoveTowards(transform.position, -listPath[0] + new Vector3(0, yPathOffset, 0), speed * Time.deltaTime);

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
