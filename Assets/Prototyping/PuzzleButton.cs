using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    public bool IsPressed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerNew" || other.gameObject.name == "Crate")
        {
            IsPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PlayerNew" || other.gameObject.name == "Crate")
        {
            IsPressed = false;
        }
    }
}