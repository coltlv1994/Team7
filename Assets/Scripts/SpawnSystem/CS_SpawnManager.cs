using UnityEngine;
using System.Collections.Generic;
//Bogdan Livanov
public class CS_SpawnManager : MonoBehaviour
{

    [System.Serializable]
    public class SpawnObject
    {
        public GameObject prefab;  
        public Transform[] positions;
        public bool isRandom;

    }

    [Header("Spawn Settings")]
    [SerializeField] private List<SpawnObject> spawnObjects = new List<SpawnObject>(); 

    
    void Start()
    {      
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        foreach (SpawnObject spawnObj in spawnObjects)
        {
            if (spawnObj.isRandom)
            {
                // Spawn at random position from the positions array
                int randomIndex = Random.Range(0, spawnObj.positions.Length);
                Instantiate(spawnObj.prefab, spawnObj.positions[randomIndex].position, Quaternion.identity);
            }
            else
            {
                // Spawn at all positions
                foreach (Transform pos in spawnObj.positions)
                {
                    Instantiate(spawnObj.prefab, pos.position, Quaternion.identity);
                }
            }
        }
    }
}
