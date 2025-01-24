using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CS_EnemyScript : MonoBehaviour //Created by Elliot
{                                        //This used mixes between transform and rigidbody to move around, this might get changed to only of of them
    public GameObject m_playerOBJ;
    public float m_rotationSpeed;
    public float m_moveSpeed;
    public GameObject m_pivotObject;
    private Vector3 startPosition;
    private float walkingBackTime;
    public float m_lungeAtPlayerMinDistance;
    public float m_lungeSpeed;
    private Rigidbody m_rb;
    private bool m_lungingAtPlayer;
    private float m_resetLungeTimer;

    [SerializeField] CS_PlayerHealthbar m_healthbar;
    [SerializeField] CS_PlayerMovement m_playerScript;
    public int m_enemyDamage;
    public Collider m_collider;


    public EnemyState state = 0;
    private void Start()
    {
        startPosition = transform.position;
        m_rb = GetComponent<Rigidbody>();
        state = EnemyState.IdleState;
    }
    public enum EnemyState
    {
        IdleState = 0,
        WalkBackState = 1,
        AttackState = 2,
        DieState = 3
    }

    private void Update()
    {
        m_resetLungeTimer += Time.deltaTime;
        if(m_resetLungeTimer > 2f)
        {
            ammountOfLunge = 1;
            m_resetLungeTimer = 0;
        }
        switch (state)
        {
            case EnemyState.IdleState:
                IdleIsActive();
                break;

            case EnemyState.WalkBackState:
                walkingBackTime += Time.deltaTime;
                WalkBackActive();
                break;

            case EnemyState.AttackState:
                AttackIsActive();
                break;

            case EnemyState.DieState:
                DyingIsActive();
                break;
            default: break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == m_playerOBJ)
        {
            state = EnemyState.AttackState;
        }
        else
        {
            state = EnemyState.WalkBackState;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == m_playerOBJ)
        {
            state = EnemyState.WalkBackState;
        }
    }

     private void OnCollisionEnter(Collision collision)
     {
        if (collision.gameObject.name == "Player") //This will be changed to be expansible
        {
            m_healthbar.TakeDamage(m_enemyDamage);
            m_playerScript.KnockBack();
        }
     }

    private void WalkBackActive()
    {
            float distance = Vector3.Distance(transform.position, startPosition);
            if (distance <= 1 && walkingBackTime >= 2f) //The timer is a check, elsewise the state will flicker between AttackState and WalkBackState in infinity loop
            {
                state = EnemyState.IdleState;  
                walkingBackTime = 0;
            }
            else
            {
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, 0);
                transform.LookAt(startPosition, transform.up);
                Vector3 newPos = Vector3.MoveTowards(transform.position, startPosition, m_moveSpeed * Time.deltaTime);
                transform.position = newPos;
            }      
    }

    private void IdleIsActive()
    {
        transform.RotateAround(m_pivotObject.transform.position, new Vector3(0, 1, 0), m_rotationSpeed * Time.deltaTime);
    }
    int ammountOfLunge = 1;
    private void AttackIsActive()
    {
        float distance = Vector3.Distance(transform.position, m_playerOBJ.transform.position);
        if(distance <= m_lungeAtPlayerMinDistance && !m_lungingAtPlayer)
        {
            if(ammountOfLunge == 1)
            {
                print("Stop1");
                m_lungingAtPlayer = true;
                m_collider.enabled = true ;
                m_rb.AddForce(transform.forward * m_lungeSpeed, ForceMode.Impulse);
                m_rb.AddForce(transform.up * m_lungeSpeed, ForceMode.Impulse);
                m_lungingAtPlayer = false;
                print("Stop2");
                transform.rotation = new Quaternion(0, transform.rotation.y, 0, 0);
                m_collider.enabled = false;
                ammountOfLunge--;
            }
            m_collider.enabled = true;
        }

        //transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, 0);
        transform.LookAt(m_playerOBJ.transform.position, transform.up);
        Vector3 newPos = Vector3.MoveTowards(transform.position, m_playerOBJ.transform.position, m_moveSpeed * Time.deltaTime);
        transform.position = newPos;
    }

    private void DyingIsActive()
    {
        Destroy(gameObject);
    }

}
