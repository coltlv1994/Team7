//Created by Linus Jernström
using UnityEngine;

public class CS_PuzzleLockTrigger : MonoBehaviour
{
    [HideInInspector] public PuzzleManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        
        if(manager.PuzzleOpen())
            manager.Lock();
    }
}
