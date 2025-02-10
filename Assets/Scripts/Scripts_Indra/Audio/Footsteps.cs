//Indra
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Footsteps : MonoBehaviour 
{
    public AudioSource footstepSource;
    public AudioClip defaultStep, throneStep, waterStep;

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
//{
    //[Header("Footsteps")]
    //public List<AudioClip> stoneFS;
    //public List<AudioClip> throneFS;
    //public List<AudioClip> waterFS;

    //enum FSMaterial 
    //{
    //    Stone,
    //    Throne,
    //    Water,
    //    Empty
    //}

    //private AudioSource footstepSource;

    //private void Start()
    //{
    //    footstepSource = GetComponent<AudioSource>();
    //}

    //private FSMaterial SurfaceSelect() 
    //{
    //    RaycastHit hit;
    //    Ray ray = new Ray(transform.position + Vector3.up * 0.5f, -Vector3.up);
    //    Material surfaceMaterial;

    //    if (Physics.Raycast(ray, out hit, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore)) 
    //    {
    //        Renderer surfaceRenderer = hit.collider.GetComponentInChildren<Renderer>();
    //        if (surfaceRenderer) 
    //        {
    //            surfaceMaterial = surfaceRenderer ? surfaceRenderer.sharedMaterial : null;
    //            if (surfaceMaterial.name.Contains("ProBuilderDefault"))
    //            {
    //                return FSMaterial.Stone;
    //            }
    //            else 
    //            {
    //                return FSMaterial.Empty;
    //            }
    //        }
    //    }
    //    return FSMaterial.Empty;
    //}

    //void PlayFootstep() 
    //{
    //    AudioClip clip = null;

    //    FSMaterial surface = SurfaceSelect();

    //    switch (surface) 
    //    {
    //        case FSMaterial.Stone:
    //            clip = stoneFS[UnityEngine.Random.Range(0, stoneFS.Count)];
    //            break;
    //        default:
    //            break;
    //    }
    //    Debug.Log(surface);

    //    if (surface != FSMaterial.Empty) 
    //    {
    //        footstepSource.clip = clip;
    //        footstepSource.volume = UnityEngine.Random.Range(0.02f, 0.05f);
    //        footstepSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
    //        footstepSource.Play();
    //    }
    //}

//}

//public class Footsteps : MonoBehaviour
//{
//    [Range(0, 20f)]
//    public float frequency = 10.0f;

//    public UnityEvent onFootStep;

//    float Sin;

//    void Update()
//    {
//        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;    
//        if(inputMagnitude > 0) 
//        {
//            StartFootsteps();    
//        }
//    }

//    private void StartFootsteps() 
//    {
//        Sin = Mathf.Sin(Time.time * frequency);

//        if (Sin > 0.97f) 
//        {
//            Debug.Log("wow");
//            onFootStep.Invoke();
//        }
//    }
    //public AudioManager audioManager;

    //public AudioClip footstepStone;
    //[Range(0, 1)] public float footstepStoneVolume = 1.0f;

    //public AudioClip footstepWood;
    //[Range(0, 1)] public float footstepWoodVolume = 1.0f;

    //public AudioClip footstepMetal;
    //[Range(0, 1)] public float footstepMetalVolume = 1.0f;

    //public AudioClip footstepGross;
    //[Range(0, 1)] public float footstepGrossVolume = 1.0f;

    //RaycastHit hit;
    //public Transform rayStart;
    //public float range;
    //public LayerMask layerMask;

    //public void Footstep()
    //{
    //}
//}
