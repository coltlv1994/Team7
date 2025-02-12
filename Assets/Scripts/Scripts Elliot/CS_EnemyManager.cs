using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class CS_EnemyManager : MonoBehaviour
{
    public List<GameObject> m_enemyTotal;
    public PuzzleManager[] m_puzzleManagers;
    public List<Vector3> m_enemySpawnPoints;
     
    [Header("Move the boxcollider's position and size to set it how you want to it interact")]
    public GameObject m_enemyPrefab;
    public List<GameObject> BlockageOBJ; 
    public int m_enemyAmount;
    public TextMeshProUGUI m_enemiesLeftText;
    public bool m_killAllEnemiesSequence;
    public PrototypeTimer m_prototypeTimer;

    public delegate void EnemyDelegate();
    public static EnemyDelegate OnSpawnEnemies;
    public static EnemyDelegate OnDespawnEnemies;

    // CS_EnemyManager.OnSpawnEnemies.Invoke(); - Call this anywhere to spawn the enemies
    // CS_EnemyManager.OnDespawnEnemies.Invoke(); - Call this anywhere to despawn the spawned enemies

    private void OnEnable()
    {
        m_puzzleManagers = GetComponentsInChildren<PuzzleManager>();
        m_prototypeTimer = FindAnyObjectByType<PrototypeTimer>();
        OnSpawnEnemies += SpawnEnemies;
        OnDespawnEnemies += DespawnEnemies;
    }

    private void OnDisable()
    {
        OnSpawnEnemies -= SpawnEnemies;
        OnDespawnEnemies -= DespawnEnemies;
    }

    private void Update()
    {
        if (m_killAllEnemiesSequence) //This might be heavy for the PC, pay attention to FPS counter in unity to see if it is heavy
        {
            m_prototypeTimer.timeTicking = false;
            m_enemiesLeftText.gameObject.SetActive(true);
            m_enemiesLeftText.text = "Enemies left:" + " " + m_enemyTotal.Count;

            for (int i = 0; i < m_enemyTotal.Count; i++)
            {
                if (m_enemyTotal.Count <= 0) return;
                if (m_enemyTotal[i] == null) m_enemyTotal.Remove(m_enemyTotal[i]);
                else
                {
                    var m_theEnemyScript = m_enemyTotal[i].GetComponent<CS_EnemyScript>();
                    if (!m_theEnemyScript.m_recovering || m_theEnemyScript.m_enemyCurrentHealth! <= 0)
                    {
                        m_theEnemyScript.state = CS_EnemyScript.EnemyState.AttackState;
                    }
                }
            }
        }
        else { m_enemiesLeftText.gameObject.SetActive(false); }

        if(m_enemyTotal.Count <= 0 && m_killAllEnemiesSequence) 
        {
            m_prototypeTimer.timeTicking = true;
            m_killAllEnemiesSequence = false;
            CS_EnemyManager.OnDespawnEnemies.Invoke();
            //foreach (GameObject doorOBJ in BlockageOBJ)
            //{
            //    doorOBJ.SetActive(false);
            //    //doorOBJ.GetComponent<PuzzleButton>().IsPressed = false;
            //}
            foreach(PuzzleManager theDoor in m_puzzleManagers)
            {
                theDoor.closeDoor = true;
            }
            print("Killed All enemies");  
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_killAllEnemiesSequence = true;
            this.GetComponent<Collider>().enabled = false;
            //foreach (GameObject doorOBJ in BlockageOBJ)
            //{
            //    doorOBJ.SetActive(true);
            //    //doorOBJ.GetComponent<PuzzleButton>().IsPressed = true;
            //}
            foreach (PuzzleManager theDoor in m_puzzleManagers)
            {
                theDoor.openDoor = true;
            }
            CS_EnemyManager.OnSpawnEnemies.Invoke();
        }
    }

    private void SpawnEnemies()
    {
        print("EventStarted");
        for (int i = 0; i < m_enemyAmount; i++)
        {
           GameObject newEnemy = Instantiate(m_enemyPrefab);
           m_enemyTotal.Add(newEnemy);
           var randomSpawnSpot = UnityEngine.Random.Range(0, m_enemySpawnPoints.Count);
           newEnemy.transform.position = this.transform.position + m_enemySpawnPoints[randomSpawnSpot];
           m_enemySpawnPoints.RemoveAt(randomSpawnSpot);
        }
    }

    private void DespawnEnemies()
    {
        foreach (GameObject newEnemy in m_enemyTotal)
        {
            Destroy(newEnemy);
        }
        m_enemyTotal.Clear();
    }
}
