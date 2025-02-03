using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    [SerializeField] bool remainPressed;

    public bool IsPressed = false;
    [SerializeField] AudioManager audioManager;
    [SerializeField] AudioClip buttonPressOn;
    [SerializeField] AudioClip buttonPressOff;

    private void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerNew" || other.gameObject.name == "Crate")
        {
            audioManager.PlaySFX(buttonPressOn);
            IsPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PlayerNew" || other.gameObject.name == "Crate")
        {
            if (!remainPressed)
            {
                audioManager.PlaySFX(buttonPressOff);            
            }
            IsPressed = false;
        }
    }
}