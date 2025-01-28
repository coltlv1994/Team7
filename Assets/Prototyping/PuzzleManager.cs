using TMPro;
using UnityEngine;
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private PuzzleButton[] buttonScript;
    [SerializeField] private int requiredButtons = 1;
    [SerializeField] private Transform door;
    [SerializeField] int doorMoveHeight = 3;

    private bool puzzleSolved = false;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

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
        if (puzzleSolved) return;

        int pressedCount = 0;

        foreach (PuzzleButton button in buttonScript)
        {
            if (button.IsPressed) pressedCount++;
        }

        if (pressedCount >= requiredButtons)
        {
            SolvePuzzle();
        }
    }

    private void SolvePuzzle()
    {
        puzzleSolved = true;
        StartCoroutine(OpenDoor());
    }

    private IEnumerator OpenDoor()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 3)
        {
            if (door != null)
            {
                door.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / 3);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

       
        //door.position = targetPosition;
        
    }
}