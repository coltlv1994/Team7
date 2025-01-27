using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public class CS_EnemyScript : MonoBehaviour //Created by Elliot //Still being worked on...
{                                        //This used mixes between transform and rigidbody to move around, this might get changed to only of of them
    [Header("Numbers and Values")]
    public float m_rotationSpeed;
    public float m_moveSpeed;

    public float m_lungeAtPlayerMinDistance;
    public float m_lungeForce;
    private bool m_lungingAtPlayer;
    private float m_resetLungeTimer;
    int ammountOfLunge = 1;

    public float xRotation;
    public float raycastToGround;
    private float walkingBackTime;

    public int m_enemyDamage;

    [Header("Refrences")]
    public GameObject m_playerOBJ;
    CS_PlayerMovement m_playerScript;
    [SerializeField] CS_PlayerHealthbar m_healthbar;

    public GameObject m_pivotObject;
    private Vector3 startPosition;

    public Collider m_collider;
    private Rigidbody m_rb;

    [Header("WhatAIDoing")]
    public EnemyState state = 0;

    private void Start()
    {
        m_playerScript = m_playerOBJ.GetComponent<CS_PlayerMovement>();
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

    public void Update()
    {
        Vector3 down = transform.TransformDirection(Vector3.down) * 5;
        Debug.DrawRay(transform.position, down, Color.red);
        GroundCheck();
        m_resetLungeTimer += Time.deltaTime;
        if(m_resetLungeTimer > 2f && m_lungingAtPlayer)
        {
            m_lungingAtPlayer = false;
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

    private void GroundCheck()
    {
        Vector3 down = transform.TransformDirection(Vector3.back) * raycastToGround;
        Debug.DrawRay(transform.position, down, Color.red);
        RaycastHit hit;
        if(!Physics.Raycast(transform.position, down, out hit, raycastToGround))
        {
            if (!m_lungingAtPlayer)
            {
                transform.Rotate(Vector3.right * -90);
            }
        }
        else { m_lungingAtPlayer = true;}
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
        if (collision.gameObject.name == "Player") //This will be changed to be expandable
        {
            m_healthbar.TakeDamage(m_enemyDamage);
            m_playerScript.KnockBack();
        }
     }

    private void WalkBackActive()
    {
            float distance = Vector3.Distance(transform.position, startPosition);
            if (distance <= 2 && walkingBackTime >= 0.5f) //The timer is a check, elsewise the state will flicker between AttackState and WalkBackState in infinity loop
            {
                state = EnemyState.IdleState;  
                walkingBackTime = 0;
            }
            else if(distance > 1)
            {
              transform.LookAt(startPosition);
              transform.Rotate(Vector3.right * -90);
              Vector3 newPos = Vector3.MoveTowards(transform.position, startPosition, m_moveSpeed * Time.deltaTime);
              transform.position = newPos;
            }      
    }

    private void IdleIsActive()
    {
      transform.RotateAround(m_pivotObject.transform.position, new Vector3(0, -1, 0), m_rotationSpeed * Time.deltaTime);
    }

    private void AttackIsActive()
    {
        transform.LookAt(m_playerOBJ.transform.position);
        transform.Rotate(Vector3.right * -90);
        Vector3 newPos = Vector3.MoveTowards(transform.position, m_playerOBJ.transform.position, m_moveSpeed * Time.deltaTime);
        transform.position = newPos;

        float distance = Vector3.Distance(transform.position, m_playerOBJ.transform.position);
        if (distance <= m_lungeAtPlayerMinDistance && !m_lungingAtPlayer)
        {
            m_lungingAtPlayer = true;
            if (ammountOfLunge == 1)
            {
                m_rb.AddForce(transform.forward * m_lungeForce, ForceMode.Impulse);
                m_rb.AddForce(-transform.up * m_lungeForce, ForceMode.Impulse);
                m_collider.enabled = false;
                ammountOfLunge--;
            }
            m_collider.enabled = true;
        }
    }

    private void DyingIsActive()
    {
        Destroy(gameObject);
    }

}
