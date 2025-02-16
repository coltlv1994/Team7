using System;
using System.Collections;
using System.Linq;
using System.Net.NetworkInformation;
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
    float timerDragBody;

    [Header("Lunge Settings")]
    public float m_lungeAtPlayerMinDistance;
    public float m_lungeForce;
    [NonSerialized] public bool m_lungingAtPlayer;
    [NonSerialized] public float m_resetLungeTimer;
    [NonSerialized] public int ammountOfLunge = 1;
    bool m_takingtheDamage;

    [Header("Ground Settings")]
    public float raycastToGround;
    float walkingBackTime;
    [NonSerialized] public bool m_recovering;

    [Header("Combat")]
    public int m_enemyDamage;
    public int m_enemyCurrentHealth;
    public int m_enemyMaxHealth;
    bool m_died;
    bool m_canGiveDamage = true;
    float m_resetStateTimer;
    [NonSerialized] public bool StunStopping;
    [NonSerialized] public bool firstHit;
    public bool m_canJuggleEnemy;
    [NonSerialized] public bool m_jugglingEnemy;
    bool takenHitLand;
    public float m_enemyKnockbackForce;
    [NonSerialized] public bool stopTime;
    [NonSerialized] public bool startTime;
    private float timeScaler;

    [Header("Refrences")]
    [NonSerialized] GameObject m_playerOBJ;
    FPSController m_playerScript;
    [NonSerialized] CS_PlayerHealthbar m_healthbar;
    SkinnedMeshRenderer[] m_meshrenders;

    [NonSerialized] public GameObject m_pivotObject;
    Vector3 startPosition; 

    SphereCollider m_sphereCollider;
    Rigidbody m_rb;
    CS_RespawnCheck m_respawnCheck;
    Animator enemyAnimtor;
    GameObject m_lookAtPlayerPivotPrefab;
    public GameObject ItemEnemyDrops;

    [Header("WhatAIDoing")]
    public EnemyState state = 0;

    private void Start()
    {
        m_playerOBJ = GameObject.FindGameObjectWithTag("Player");
        m_healthbar = GameObject.Find("PlayerHealthBar").GetComponent<CS_PlayerHealthbar>();
        m_enemyCurrentHealth = m_enemyMaxHealth;
        m_playerScript = m_playerOBJ.GetComponent<FPSController>();
        m_meshrenders = GetComponentsInChildren<SkinnedMeshRenderer>();
        startPosition = transform.position;
        m_rb = GetComponent<Rigidbody>();
        m_sphereCollider = GetComponent<SphereCollider>();
        m_respawnCheck = m_playerOBJ.GetComponent<CS_RespawnCheck>();
        enemyAnimtor = GetComponent<Animator>();
        m_lookAtPlayerPivotPrefab = GameObject.Find("LookingAtPlayerPivot");
        m_pivotObject = this.gameObject.transform.GetChild(0).gameObject;
        state = EnemyState.SpawningState;
        timeScaler = 1f;
        enemyAnimtor.SetTrigger("Spawn");
    }
    public enum EnemyState
    {
        IdleState = 0,
        WalkBackState = 1,
        AttackState = 2,
        DieState = 3,
        SpawningState = 4
    }

    public void Update()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.CurrentGameState == GameState.Play)
        {
            Time.timeScale = timeScaler;
            Vector3 down = transform.TransformDirection(Vector3.down) * raycastToGround;
            Debug.DrawRay(transform.position, down, Color.red);

            if (m_enemyCurrentHealth <= 0) { state = EnemyState.DieState; }
            if (m_recovering) Recovery();
            StunStopFunction();
            if (m_canJuggleEnemy) m_jugglingEnemy = false;

            m_resetStateTimer += Time.deltaTime;
            if (m_resetStateTimer >= 1 && state != EnemyState.AttackState)
            {
                transform.localRotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);
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
                case EnemyState.SpawningState:
                    SpawningActive();
                    break;

                case EnemyState.IdleState:
                    m_respawnCheck.state = PlayerCombatState.OutsideCombatState;
                    IdleIsActive();
                    break;

                case EnemyState.WalkBackState:
                    m_respawnCheck.state = PlayerCombatState.OutsideCombatState;
                    walkingBackTime += Time.deltaTime;
                    WalkBackActive();
                    break;

                case EnemyState.AttackState:
                    m_respawnCheck.state = PlayerCombatState.CombatState;
                    AttackIsActive();
                    break;

                case EnemyState.DieState:
                    DyingIsActive();
                    break;
                default: break;
            }
        }
        else
        {
            Time.timeScale = 0;
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
        enemyAnimtor.SetBool("Walk", true);
        float distance = Vector3.Distance(transform.position, startPosition);
            if (distance <= 2 && walkingBackTime >= 0.5f) //The timer is a check, elsewise the state will flicker between AttackState and WalkBackState in infinity loop
            {
                state = EnemyState.IdleState;  
                walkingBackTime = 0;
            }
            else if(distance > 1)
            {
                //Walking animation
                transform.LookAt(startPosition);
                if(enemyAnimtor.GetBool("Walk") == true)
                {
                  timerDragBody += Time.deltaTime;
                  if(timerDragBody > 0.5)
                  {
                    Vector3 newPos = Vector3.MoveTowards(transform.position, startPosition, m_moveSpeed * Time.deltaTime);
                    transform.position = newPos;
                    if (timerDragBody >= 1f) timerDragBody = 0;
                  }
                }
            }      
    }
    private void IdleIsActive()
    {
        //Walking animation
        enemyAnimtor.SetBool("Walk", true);
        if (enemyAnimtor.GetBool("Walk") == true)
        {
            timerDragBody += Time.deltaTime;
            if (timerDragBody > 0.5)
            {
                transform.RotateAround(m_pivotObject.transform.position, new Vector3(0, 1, 0), m_rotationSpeed * Time.deltaTime);
                if (timerDragBody >= 1f) timerDragBody = 0;
            }
        }
    }

    private void AttackIsActive()
    {
        enemyAnimtor.SetBool("Walk", true);
        transform.LookAt(m_lookAtPlayerPivotPrefab.transform.position);
        if (enemyAnimtor.GetBool("Walk") == true)
        {
            timerDragBody += Time.deltaTime;
            if (timerDragBody > 0.5)
            {
                Vector3 newPos = Vector3.MoveTowards(transform.position, m_playerOBJ.transform.position, m_moveSpeed * Time.deltaTime);
                transform.position = newPos;
                if (timerDragBody >= 1f) timerDragBody = 0;
            }
        }

        float distance = Vector3.Distance(transform.position, m_playerOBJ.transform.position);
        if (distance <= m_lungeAtPlayerMinDistance && !m_lungingAtPlayer)
        {
            m_lungingAtPlayer = true;
            if (ammountOfLunge == 1)
            {
                //Do Lunge/Attack Animation
                enemyAnimtor.SetBool("Walk", false);
                enemyAnimtor.SetTrigger("Attack_Jump");
                enemyAnimtor.Play("Attack_Jump");

                ammountOfLunge--;
                enemyAnimtor.SetTrigger("Attack_Bite");
            }
        }
        enemyAnimtor.SetTrigger("Attack_Land");
    }

    public void JumpFunction()
    {
        m_rb.AddForce(transform.forward * m_lungeForce, ForceMode.Impulse);
        m_rb.AddForce(transform.up * m_lungeForce, ForceMode.Impulse);
    }

    private void SpawningActive()
    {
        //Do Spawning Animation
        state = EnemyState.IdleState;
    }

    private void DyingIsActive()
    {
        ////Do Dying Animation
        m_canGiveDamage = false;
        m_canGiveDamage = false;
        enemyAnimtor.Play("Hit Die");
        firstHit = true;
    }
    public void DestroyOBJ()
    {
        GameObject droppedItem = Instantiate(ItemEnemyDrops);
        droppedItem.transform.Rotate(Vector3.right * 90);
        droppedItem.transform.position = this.transform.position;
        Destroy(gameObject);
    }
    public void TakingDamage(int takenDamage)
    {
        if(StunStopping) return;
        if (!firstHit && !m_jugglingEnemy)
        {
            firstHit = true;
            takenHitLand = true;
            m_enemyCurrentHealth -= takenDamage;
            m_rb.AddForce(-transform.forward * m_enemyKnockbackForce, ForceMode.Impulse);
            //Do Take Damage Animaton
            enemyAnimtor.SetTrigger("Hit");
            StartCoroutine(ChangeColorCoroutine());
            m_recovering = true;
        }
    }
    public IEnumerator ChangeColorCoroutine()
    {
        foreach (SkinnedMeshRenderer currentMesh in m_meshrenders) // Turns enemy red
        {
            currentMesh.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(1, 0.27f, 0.35f, 0));
        }

        yield return new WaitForSeconds(0.5f);

        if (!m_died)
        {
           foreach (SkinnedMeshRenderer currentMesh in m_meshrenders) // Turns enemy to normal color
           {
              currentMesh.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(1f, 1f, 1f, 1));
           }
        }
        firstHit = false;
    }
    IEnumerator GivingDamage()
    {
        enemyAnimtor.SetTrigger("Attack_Bite");
        m_canGiveDamage = false;
        m_healthbar.TakeDamage(m_enemyDamage, this.transform.position, true);
        m_playerScript.KnockBack();
        yield return new WaitForSeconds(5f);
    }

    private void Recovery()
    {
        Vector3 down = transform.TransformDirection(Vector3.down) * raycastToGround;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, down, out hit, raycastToGround))
        {
            if (hit.collider.transform != null) 
            {
                m_jugglingEnemy = false;
                m_recovering = false;
                if(takenHitLand){ enemyAnimtor.SetTrigger("Hit_Land"); takenHitLand = false; }
                else enemyAnimtor.SetTrigger("Attack_Land");
                int randomAnimNumber = UnityEngine.Random.Range(1,3);
                if(randomAnimNumber == 1) enemyAnimtor.SetBool("Hit random", true);
                else enemyAnimtor.SetBool("Hit random", false);

                if (m_enemyCurrentHealth <= 0)
                {
                    state = EnemyState.DieState;
                    m_died = true;
                    enemyAnimtor.SetBool("Hit_Survive", false);
                }
            }
        }
        else
        {
            //Do Wiggle In Air Animation, might be combined with the Getting Hit Animation
            m_canGiveDamage = false;
            m_jugglingEnemy = true;
        }
    }
    private void StunStopFunction()
    {
        if (stopTime)
        {
            timeScaler = 0f;
            startTime = true;
            stopTime = false;
        }
        if (startTime) { timeScaler += Time.unscaledDeltaTime; }

        if (timeScaler >= 1f)
        {
            startTime = false;
            timeScaler = 1f;
            StunStopping = false;
        }
    }
}
