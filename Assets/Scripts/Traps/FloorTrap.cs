//Bogdan Livanov
using UnityEngine;

public class FloorTrap : MonoBehaviour
{
    [Header("Movement Settings")]

    [Tooltip("Offset when the plate is pressed")]
    [SerializeField] private Vector3 pressedOffset = new Vector3(0, -0.1f, 0); // Offset when the plate is pressed
    [Tooltip("Speed of the plate movement")]
    [SerializeField] private float moveSpeed = 5f; // Speed of the plate movement
    [Tooltip("Distance the plate moves when pressed")]
    [SerializeField] private GameObject trapObject; // Object that will be activated when the plate is pressed
    [Tooltip("Delay before activating the trap after the plate is pressed")]
    [SerializeField] private float activationDelay = 0.5f; // Delay before activating the trap after the plate is pressed
    [Tooltip("Layers that can trigger the plate")]
    [SerializeField] private LayerMask triggerLayers; // Layers that can trigger the plate
    [Tooltip("Minimum weight of the object to trigger the plate")]
    [SerializeField] private float minimumWeight = 5f; // Minimum weight of the object to trigger the plate

    private Vector3 startPosition; // Start position of the plate
    private Vector3 targetPosition; // Target position of the plate
    private bool isMoving = false; //  Is the plate moving
    private bool isActivated = false; // Is the plate activated
    private int objectsOnPlate = 0; // Number of objects on the plate

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition;
    }

    void Update()
    {
        if (isMoving) // Move the plate towards the target position
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        }
    }

    // Check if the object is valid to trigger the plate
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectsOnPlate++;
            PressPlate();
        }
        else if (IsValidTriggerObject(other))
        {
            objectsOnPlate++;
            PressPlate();
        }
    }
    
    // Check if the object is valid to trigger the plate

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectsOnPlate--;
            if (objectsOnPlate <= 0)
            {
                objectsOnPlate = 0;
                ReleasePlate();
            }
            return;
        }

        if (IsValidTriggerObject(other))
        {
            objectsOnPlate--;
            if (objectsOnPlate <= 0)
            {
                objectsOnPlate = 0;
                ReleasePlate();
            }
        }
    }

    // Check if the object is valid to trigger the plate
    private bool IsValidTriggerObject(Collider other)
    {

        // Only check if the object's layer is in our triggerLayers mask
        return (triggerLayers.value & (1 << other.gameObject.layer)) != 0;

    }

    // Press the plate
    private void PressPlate()
    {
        isMoving = true;
        targetPosition = startPosition + pressedOffset;
        
        if (!isActivated)
        {
            isActivated = true;
            Invoke(nameof(TriggerConnectedTrap), activationDelay);
        }
    }

    // Release the plate
    private void ReleasePlate()
    {
        isMoving = true;
        targetPosition = startPosition;
    }

    // Trigger the connected trap
    private void TriggerConnectedTrap()
    {
        trapObject.SetActive(true);
    }
}
