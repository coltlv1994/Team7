using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ProBuilder.MeshOperations;
using static CS_RespawnCheck;
using static UnityEngine.GraphicsBuffer;

public class CS_EnemyScript : MonoBehaviour //Created by Elliot //Still being worked on...
{                                        //This used mixes between transform and rigidbody to move around, this might get changed to only of of them
    [Header("Speed for movement")]
    public float m_rotationSpeed;
    public float m_moveSpeed;

    [Header("Lunge Settings")]
    public float m_lungeAtPlayerMinDistance;
    public float m_lungeForce;
    public bool m_lungingAtPlayer;
    private float m_resetLungeTimer;
    int ammountOfLunge = 1;
     bool m_takingtheDamage;
    public float m_stunStopTimer;

    [Header("Ground Settings")]
    public float xRotation;
    public float raycastToGround;
    private float walkingBackTime;
    bool m_recovering;

    [Header("Combat")]
    public int m_enemyDamage;
    public int m_enemyCurrentHealth;
    public int m_enemyMaxHealth;
    bool m_died;
    bool m_canGiveDamage = true;
    float m_resetStateTimer;
    bool StunStopping;
    public bool firstHit;
    public bool m_canJuggleEnemy;
    private bool m_jugglingEnemy;

    [Header("Refrences")]
    public GameObject m_playerOBJ;
    FPSController m_playerScript;
    [SerializeField] CS_PlayerHealthbar m_healthbar;
    MeshRenderer[] m_meshrenders;

    public GameObject m_pivotObject;
    private Vector3 startPosition;

    public Collider m_collider;
    private SphereCollider m_sphereCollider;
    private Rigidbody m_rb;
    private CS_RespawnCheck m_respawnCheck;

    [Header("WhatAIDoing")]
    public EnemyState state = 0;

    private void Start()
    {
        m_playerOBJ = GameObject.FindGameObjectWithTag("Player");
        m_healthbar = GameObject.Find("PlayerHealthBar").GetComponent<CS_PlayerHealthbar>();
        m_enemyCurrentHealth = m_enemyMaxHealth;
        m_playerScript = m_playerOBJ.GetComponent<FPSController>();
        m_meshrenders = GetComponentsInChildren<MeshRenderer>();
        startPosition = transform.position;
        m_rb = GetComponent<Rigidbody>();
        m_sphereCollider = GetComponent<SphereCollider>();
        m_respawnCheck = m_playerOBJ.GetComponent<CS_RespawnCheck>();  
        state = EnemyState.IdleState;
    }
    public enum EnemyState
    {
        IdleState = 0,
        WalkBackState = 1,
        AttackState = 2,
        DieState = 3,
        StunStopSate = 4
    }

    public void Update()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.CurrentGameState == GameState.Pause) return;

        Vector3 down = transform.TransformDirection(Vector3.back) * raycastToGround;
        Debug.DrawRay(transform.position, down, Color.red);
        if(m_recovering)RecoveryCoroutine();
        if(m_canJuggleEnemy) m_jugglingEnemy = false;

        if (StunStopping && m_stunStopTimer != 0) state = EnemyState.StunStopSate;

        if(m_stunStopTimer >= 0.5f) state = EnemyState.AttackState;
        if (m_enemyCurrentHealth <= 0) { state = EnemyState.DieState; m_died = true; }


        m_resetStateTimer += Time.deltaTime;
        if (m_resetStateTimer >= 1 && state != EnemyState.AttackState && state != EnemyState.StunStopSate)
        {
            m_sphereCollider.enabled = false;
            m_sphereCollider.enabled = true;
            m_resetStateTimer = 0;
        }
        if (m_lungingAtPlayer) m_resetLungeTimer += Time.deltaTime;
        if (m_resetLungeTimer > 5f)
        {
            m_canGiveDamage = true;
            m_lungingAtPlayer = false; 
            ammountOfLunge = 1;
            m_resetLungeTimer = 0;
        }
        switch (state)
        {
            case EnemyState.IdleState:
                m_rb.isKinematic = false;
                m_respawnCheck.state = PlayerCombatState.OutsideCombatState;
                IdleIsActive();
                break;

            case EnemyState.WalkBackState:
                m_rb.isKinematic = false;
                m_respawnCheck.state = PlayerCombatState.OutsideCombatState;
                walkingBackTime += Time.deltaTime;
                WalkBackActive();
                break;

            case EnemyState.AttackState:
                m_rb.isKinematic = false;
                m_respawnCheck.state = PlayerCombatState.CombatState;
                AttackIsActive();
                break;

            case EnemyState.DieState:
                DyingIsActive();
                break;

            case EnemyState.StunStopSate:
                m_stunStopTimer += Time.deltaTime;
                StunStopFunction();
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == m_playerOBJ && !m_takingtheDamage)
        {
            state = EnemyState.WalkBackState;
        }
    }
     private void OnCollisionEnter(Collision collision)
     {
        if (collision.gameObject.CompareTag("Player") && m_canGiveDamage) //This will be changed to be expandable
        {
            StartCoroutine(GivingDamage());
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
    public void TakingDamage(int takenDamage)
    {
        if (!firstHit && !m_jugglingEnemy)
        {
            firstHit = true;
            m_enemyCurrentHealth -= takenDamage;
            StartCoroutine(ChangeColorCoroutine());
            m_recovering = true;
        }
    }
    public IEnumerator ChangeColorCoroutine()
    {
        foreach (MeshRenderer currentMesh in m_meshrenders) // Turns enemy red
        {
            currentMesh.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(1, 0.27f, 0.35f, 0));
        }

        yield return new WaitForSeconds(0.5f);

        if (!m_died)
        {
           foreach (MeshRenderer currentMesh in m_meshrenders) // Turns enemy to normal color
           {
              currentMesh.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(1f, 1f, 1f, 1));
           }
        }
        firstHit = false;
    }

    IEnumerator GivingDamage()
    {
        m_canGiveDamage = false;
        m_healthbar.TakeDamage(m_enemyDamage, this.transform.position, true);
        m_playerScript.KnockBack();
        yield return new WaitForSeconds(5f);
    }

    private void RecoveryCoroutine()
    {
        print("Recovering");
        Vector3 down = transform.TransformDirection(Vector3.back) * raycastToGround;
        Debug.DrawRay(transform.position, down, Color.red);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, down, out hit, raycastToGround));
        {
            if (hit.transform == null) { m_jugglingEnemy = true; return; }
            if (hit.transform.gameObject.CompareTag("TheGround"))
            {
                m_jugglingEnemy = false;
                m_recovering = false;
            }
            else m_jugglingEnemy = true;
        }
    }

    private void StunStopFunction()
    {
        StunStopping = true;
        m_rb.isKinematic = true;
        if (m_stunStopTimer >= 0.25f && m_lungingAtPlayer)
        {
            m_lungingAtPlayer = false;
            m_rb.isKinematic = false;

            m_rb.AddForce(transform.forward * m_lungeForce, ForceMode.Impulse);
            m_rb.AddForce(transform.up * m_lungeForce, ForceMode.Impulse);
            StunStopping = false;
            m_stunStopTimer = 0;
            state = EnemyState.AttackState;
        }
    }
}
