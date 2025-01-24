using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGuy : MonoBehaviour
{
    [SerializeField] private int health = 30;
    public enum State
    {
        Patrol,
        Chase
    }

    public State currentState;

    private NavMeshAgent agent;

    public Transform player;
    public List<Transform> waypoints;
    public float chaseRange = 10f;
    private int currentWaypointIndex;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentWaypointIndex = Random.Range(0, waypoints.Count);
        currentState = State.Patrol;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
        }

        CheckTransitions();
    }

    private void Patrol()
    {
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            currentWaypointIndex = Random.Range(0, waypoints.Count);
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
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
            currentWaypointIndex = Random.Range(0, waypoints.Count);
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
    void UpdateHealth()
    {
        //health -= 
    }
    public void TakeDamage(int damageAmount)
    {

        Debug.Log("Enemy took " +  damageAmount + " damage");
        health -= damageAmount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}