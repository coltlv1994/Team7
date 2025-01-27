using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour
{
    [SerializeField] private int health = 30;
    [SerializeField] private GameObject projectile;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform shootPoint;

    private bool canShoot = true;
    [SerializeField] bool isStatic;

    public enum State
    {
        Patrol,
        Chase,
        Shoot,
        Disabled
    }

    public State currentState;

    private NavMeshAgent agent;

    public Transform player;
    public List<Transform> waypoints;
    public float chaseRange = 10f;
    private int currentWaypointIndex;

    private Material enemyMat;
    private Rigidbody rb;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        if (waypoints.Count > 0)
        {
            currentWaypointIndex = Random.Range(0, waypoints.Count);
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }

        currentState = State.Patrol;
        //enemyMat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (agent != null)
        {
            switch (currentState)
            {
                case State.Disabled:
                    Disabled();
                    break;
                case State.Patrol:
                    Patrol();
                    break;
                case State.Chase:
                    Chase();
                    break;
            }
        }

        CheckTransitions();
    }

    private void Patrol()
    {
        if (agent.remainingDistance < 0.5f && !agent.pathPending && waypoints.Count > 0)
        {
            currentWaypointIndex = Random.Range(0, waypoints.Count);
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    private void Chase()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        
        if (distanceToPlayer <= chaseRange / 2 && canShoot)
        {
            StartCoroutine(ShootCoroutine());
        }
        else
        {
            agent.SetDestination(player.position);
        }
    }

    private IEnumerator ShootCoroutine()
    {
        canShoot = false;
        agent.isStopped = true;
        yield return new WaitForSeconds(1f);
        Shoot();
        yield return new WaitForSeconds(0.5f);
        agent.isStopped = false;

        //actual cd
        yield return new WaitForSeconds(2.5f);
        canShoot = true;
    }

    private void Shoot()
    {
        Debug.Log("Shooting!");

        Vector3 direction = (player.position - shootPoint.position).normalized;

        GameObject rangedProjectile = Instantiate(projectile, shootPoint.position, Quaternion.identity);
        Destroy(rangedProjectile, 3f);

        Rigidbody projectileRb = rangedProjectile.GetComponent<Rigidbody>();
        if (projectileRb != null)
        {
            projectileRb.AddForce(direction * 15f, ForceMode.Impulse);
        }
    }


    //private IEnumerator ShootCooldown()
    //{
    //    canShoot = false;
    //    yield return new WaitForSeconds(3f);
    //    canShoot = true;
    //}

    private void Disabled()
    {
        agent.enabled = false;
    }

    private void CheckTransitions()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (currentState == State.Patrol && distanceToPlayer <= chaseRange)
        {
            currentState = State.Chase;
        }
        else if (currentState == State.Chase && distanceToPlayer > chaseRange)
        {
            currentState = State.Patrol;

            if (waypoints.Count > 0)
            {
                currentWaypointIndex = Random.Range(0, waypoints.Count);
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
    }

    public void TakeDamage(int damageAmount, bool knockback)
    {
        Debug.Log($"rnemy took {damageAmount} damage");
        health -= damageAmount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (knockback)
        {
            Vector3 direction = (transform.position - player.position).normalized;
            direction.y = 0;

            currentState = State.Disabled;
            agent.enabled = false;
            rb.AddForce(direction * 30f, ForceMode.Impulse);

            StartCoroutine(KnockbackRecovery());
        }
    }

    private IEnumerator KnockbackRecovery()
    {
        yield return new WaitForSeconds(0.8f);
        rb.linearVelocity = Vector3.zero;
        agent.enabled = true;
        currentState = State.Chase;
    }
}
