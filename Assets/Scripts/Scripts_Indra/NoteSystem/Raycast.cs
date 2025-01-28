using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    [Header("Raycast Feature")]
    [SerializeField] private float rayLength = 5;
    private Camera _camera;

    private NoteController noteController;

    [Header("Crosshair")]
    [SerializeField] private Image crosshair;

    [Header("Crosshair")]
    [SerializeField] private KeyCode interactKey;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }
    private void Update()
    {
        if (Physics.Raycast(_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f)), transform.forward, out RaycastHit hit, rayLength))
        {
            var readableItem = hit.collider.GetComponent<NoteController>();
            if (readableItem != null)
            {
                noteController = readableItem;
                HighlightCrosshair(true);
            }
            else
            {
                ClearNote();
            }
        }
        else 
        {
            ClearNote();
        }

        if (noteController != null) 
        {
            if (Input.GetKeyDown(interactKey)) 
            {
                noteController.ShowNote();
            }
        }
    }

    void ClearNote() 
    {
        if (noteController != null) 
        {
            HighlightCrosshair(false);
            noteController = null;
        }
    }

    void HighlightCrosshair(bool on) 
    {
        if (on)
        {
            crosshair.color = Color.blue;
        }
        else 
        {
            crosshair.color = Color.white;
        }
    }
}
