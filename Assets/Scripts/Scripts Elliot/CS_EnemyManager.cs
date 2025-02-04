using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public class CS_EnemyManager : MonoBehaviour
{
    public List<GameObject> m_enemyTotal;
    public List<Vector3> m_enemySpawnPoints;
    public GameObject m_enemyPrefab;
    public int m_enemyAmount;

    public delegate void EnemyDelegate();
    public static EnemyDelegate OnSpawnEnemies;
    public static EnemyDelegate OnDespawnEnemies;

    // CS_EnemyManager.OnSpawnEnemies.Invoke(); - Call this anywhere to spawns the enemies
    // CS_EnemyManager.OnDespawnEnemies.Invoke(); - Call this anywhere to despawned the spawned enemies

    private void OnEnable()
    {
        OnSpawnEnemies += SpawnEnemies;
        OnDespawnEnemies += DespawnEnemies;
    }

    private void OnDisable()
    {
        OnSpawnEnemies -= SpawnEnemies;
        OnDespawnEnemies -= DespawnEnemies;
    }

    public void SpawnEnemies()
    {
        print("EventStarted");
        for (int i = 0; i < m_enemyAmount; i++)
        {
           GameObject newEnemy = Instantiate(m_enemyPrefab);
           m_enemyTotal.Add(newEnemy);
           newEnemy.transform.position = m_enemySpawnPoints[UnityEngine.Random.Range(0, m_enemySpawnPoints.Count)];
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
