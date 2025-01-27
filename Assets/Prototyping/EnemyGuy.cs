using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGuy : MonoBehaviour
{
    [SerializeField] private int health = 30;
    public enum State
    {
        Patrol,
        Chase,
        Disabled
    }

    public State currentState;

    private NavMeshAgent agent;

    public Transform player;
    public List<Transform> waypoints;
    public float chaseRange = 10f;
    private int currentWaypointIndex;

    Color originalColor;
    Renderer renderer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentWaypointIndex = Random.Range(0, waypoints.Count);
        currentState = State.Patrol;
        renderer = transform.GetChild(0).GetComponent<Renderer>();

        originalColor = renderer.material.color;
    }

    private void Update()
    {
        if(agent != null)
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
        if (agent != null)
        {
            CheckTransitions();
        }
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
    private void Disabled()
    {
        //agent.isStopped = true;
        agent.enabled = false;
        StartCoroutine(Delay());
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
    public void TakeDamage(int damageAmount, bool knockback)
    {
        //originalColor = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));
        //renderer.material.color = originalColor;
        //StartCoroutine(HitEffect());
        Debug.Log("Enemy took " +  damageAmount + " damage");
        health -= damageAmount;

        //if (health <= 0)
        //{
        //    Destroy(gameObject);
        //}

        if (knockback)
        {
            Vector3 direction = (transform.position - player.transform.position);
            direction.y = 0;
            direction.Normalize();

            currentState = State.Disabled;
            
            GetComponent<Rigidbody>().AddForce(30 * direction);
        }
    }
    //private IEnumerator HitEffect()
    //{
        
    //    float duration = 0.2f;
    //    float elapsedTime = 0f;
    //    while (elapsedTime < duration)
    //    {
    //        renderer.material.color = Color.Lerp(originalColor, Color.red, elapsedTime / duration);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    elapsedTime = 0f;
    //    while (elapsedTime < duration)
    //    {
    //        renderer.material.color = Color.Lerp(Color.red, originalColor, elapsedTime / duration);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    renderer.material.color = originalColor;
    //}
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.8f);
        agent.enabled = true;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        currentState = State.Chase;
    }
}