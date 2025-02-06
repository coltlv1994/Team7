using UnityEngine;
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private PuzzleButton[] buttonScript;
    [SerializeField] private int requiredButtons = 1;
    [SerializeField] private Transform door;
    [SerializeField] private float doorMoveHeight = 3f;
    [SerializeField] private float doorOpenTime = 3f;
    [SerializeField] private float doorCloseTime = 3f;
    [SerializeField] private bool resetOnRelease = false;
    [SerializeField] private Transform cameraPosition;

    private bool puzzleSolved = false;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Coroutine doorCoroutine;
    private CS_DoorCamera _doorCamera;

    //[SerializeField] AudioClip doorSound;
    [SerializeField] AudioManager audioManager;

    private void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        if (!TryGetComponent<CS_DoorCamera>(out _doorCamera))
            _doorCamera = gameObject.AddComponent<CS_DoorCamera>();
        
        if (door != null)
        {
            initialPosition = door.position;
            targetPosition = door.position + Vector3.up * doorMoveHeight;
        }
    }

    private void Update()
    {
        int pressedCount = 0;

        foreach (PuzzleButton button in buttonScript)
        {
            if (button.IsPressed) pressedCount++;
        }

        if (pressedCount >= requiredButtons)
        {
            if (!puzzleSolved) SolvePuzzle();
        }
        else if (resetOnRelease && puzzleSolved)
        {
            puzzleSolved = false;
            if (doorCoroutine != null) StopCoroutine(doorCoroutine);
            doorCoroutine = StartCoroutine(CloseDoor());
        }
    }

    private void SolvePuzzle()
    {
        puzzleSolved = true;
        
        if(cameraPosition != null && _doorCamera != null)
            _doorCamera.MoveCameraToPos(cameraPosition);
            
        if (doorCoroutine != null) StopCoroutine(doorCoroutine);
        doorCoroutine = StartCoroutine(OpenDoor());
    }

    private IEnumerator OpenDoor()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = door.position;

        door.GetComponent<AudioSource>().Play();
        //audioManager.PlaySFX(door.GetComponent<AudioSource>().clip);
        Debug.Log(door.GetComponent<AudioSource>().clip.name);
        while (elapsedTime < doorOpenTime)
        {
            while (GameStateManager.Instance != null && GameStateManager.Instance.CurrentGameState == GameState.Pause)
            {
                yield return null; //pause //indra
            }

            if (door != null)
            {
                door.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / doorOpenTime);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (door != null)
        {
            door.position = targetPosition;
        }
    }

    private IEnumerator CloseDoor()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = door.position;
        float remainingDistance = Vector3.Distance(startPosition, initialPosition);
        float flexibleCloseTime = doorCloseTime * (remainingDistance / doorMoveHeight);

        audioManager.PlaySFX(door.GetComponent<AudioSource>().clip);
        Debug.Log(door.GetComponent<AudioSource>().clip.name);
        while (elapsedTime < flexibleCloseTime)
        {
            while (GameStateManager.Instance != null && GameStateManager.Instance.CurrentGameState == GameState.Pause)
            {
                yield return null; // hope this works //indra
            }

            if (door != null)
            {
                door.position = Vector3.Lerp(startPosition, initialPosition, elapsedTime / flexibleCloseTime);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (door != null)
        {
            door.position = initialPosition;
        }
    }
}