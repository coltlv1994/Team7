//indra
using System;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        instance = this;
    }
    public event Action onPlayerJump;
    public void PlayerJump()
    {
        if (onPlayerJump != null)
        {
            onPlayerJump();
        }
    }
}
