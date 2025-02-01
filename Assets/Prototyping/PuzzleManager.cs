using UnityEngine;
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private PuzzleButton[] buttonScript;
    [SerializeField] private int requiredButtons = 1;
    [SerializeField] private Transform door;
    [SerializeField] private int doorMoveHeight = 3;
    [SerializeField] private int doorOpenSpeed = 3;
    [SerializeField] private int doorCloseSpeed = 3;
    [SerializeField] private bool resetOnRelease = false;

    private bool puzzleSolved = false;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Coroutine doorCoroutine;

    private void Start()
    {
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
        if (doorCoroutine != null) StopCoroutine(doorCoroutine);
        doorCoroutine = StartCoroutine(OpenDoor());
    }

    private IEnumerator OpenDoor()
    {
        float elapsedTime = 0f;
        while (elapsedTime < doorOpenSpeed)
        {
            if (door != null)
            {
                door.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / doorOpenSpeed);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator CloseDoor()
    {
        float elapsedTime = 0f;
        while (elapsedTime < doorCloseSpeed)
        {
            if (door != null)
            {
                door.position = Vector3.Lerp(door.position, initialPosition, elapsedTime / doorCloseSpeed);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
