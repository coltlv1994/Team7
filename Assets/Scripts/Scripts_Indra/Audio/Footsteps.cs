//Indra
//using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Footsteps : MonoBehaviour
{
    public AudioSource footstepSource;
    public AudioClip defaultStep, throneStep, waterStep;
    [Range(0, 1)] public float stepVolume;

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.CurrentGameState == GameState.Pause) return;

        if (characterController.isGrounded && characterController.velocity.magnitude > 1f)
        {
            if (!footstepSource.isPlaying)
            {
                PlayFootsteps();
            }
        }
    }

    void PlayFootsteps()
    {
        string floorLayer = GetCurrentLayer();

        switch (floorLayer)
        {
            case "Default":
                footstepSource.clip = defaultStep; Debug.Log("Default");
                break;

            case "Throne":
                footstepSource.clip = throneStep; Debug.Log("Throne");
                break;

            case "Water":
                footstepSource.clip = waterStep; Debug.Log("Water");
                break;

        }

        if (footstepSource.clip != null)
        {
            footstepSource.volume = stepVolume;
            footstepSource.Play();
        }
    }

    string GetCurrentLayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            return LayerMask.LayerToName(hit.collider.gameObject.layer);
        }
        return "Unknown";

    }
}