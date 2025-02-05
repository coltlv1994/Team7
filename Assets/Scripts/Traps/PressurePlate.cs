//Bogdan Livanov
using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Offset when the plate is pressed")]
    [SerializeField] private Vector3 pressedOffset = new Vector3(0, -0.1f, 0); // Offset when the plate is pressed
    [Tooltip("Speed of the plate movement")]
    [SerializeField] private float moveSpeed = 5f; // Speed of the plate movement
    [Tooltip("Distance the plate moves when pressed")]
    [SerializeField] private GameObject plateObject; // Object that will be activated when the plate is pressed
    [Tooltip("Delay before activating the trap after the plate is pressed")]
    [SerializeField] private float activationDelay = 0.5f; // Delay before activating the trap after the plate is pressed
    [Tooltip("Layers that can trigger the plate")]
    [SerializeField] private LayerMask triggerLayers; // Layers that can trigger the plate
    [Tooltip("Minimum weight of the object to trigger the plate")]
    [SerializeField] private float minimumWeight = 5f; // Minimum weight of the object to trigger the plate
    
    [System.Serializable]
    public class MovableObject
    {
        public GameObject objectToMove;
        public GameObject[] targetObjects;
        public float moveSpeed = 5f;
        public float stayDuration = 5f;
        public Vector3 originalPosition;
        public bool isLocal = true;
        [SerializeField] public MonoBehaviour[] scriptsToToggle;
        [HideInInspector] public int currentTargetIndex = 0;
        [HideInInspector] public bool isReturning = false;
        [HideInInspector] public float stayTimer = 0f;
    }

    [Space(10)]

    [Header("Focus Settings")] 
    [SerializeField] private MovableObject[] movableObjects;
    private bool hasActivated = false;

    // Movement settings
    private Vector3 startPosition; // Start position of the plate
    private Vector3 targetPosition; // Target position of the plate
    private bool isMoving = false; //  Is the plate moving
    private bool isActivated = false; // Is the plate activated
    private int objectsOnPlate = 0; // Number of objects on the plate

    // Focus settings
    private bool isFocusing = false;

    void Start()
    {
            startPosition = transform.position;
            targetPosition = startPosition;
    }

private void Update()
{
    // Move the plate to the target position
    if (isMoving)
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    // Move the objects to the target positions
    if (isFocusing)
    {
        foreach (var movable in movableObjects)
        {
            if (movable.objectToMove == null || movable.targetObjects == null || movable.targetObjects.Length == 0) 
                continue;

            if (!movable.isReturning)
            {
                Vector3 currentTarget = movable.targetObjects[movable.currentTargetIndex].transform.position;
                
                if (Vector3.Distance(movable.objectToMove.transform.position, currentTarget) > 0.01f)
                {
                    movable.objectToMove.transform.position = Vector3.MoveTowards(
                        movable.objectToMove.transform.position,
                        currentTarget,
                        movable.moveSpeed * Time.deltaTime);
                }
                else
                {
                    movable.stayTimer += Time.deltaTime;
                    if (movable.stayTimer >= movable.stayDuration)
                    {
                        movable.currentTargetIndex++;
                        movable.stayTimer = 0f;
                        if (movable.currentTargetIndex >= movable.targetObjects.Length)
                        {
                            movable.isReturning = true;
                        }
                    }
                }
            }
            else
            {
                movable.objectToMove.transform.position = movable.originalPosition;
                ResetMovableObjects(movable);
            }
            if (movable.isReturning)
            {
                if (movable.isLocal)
                {
                    movable.objectToMove.transform.localPosition = movable.originalPosition;
                }
                else
                {
                    movable.objectToMove.transform.position = movable.originalPosition;
                }
                ResetMovableObjects(movable);
            }           
        }
    }    
}

    // Reset the movable object
    private void ResetMovableObjects(MovableObject movable)
    {
        movable.currentTargetIndex = 0;
        movable.isReturning = false;
        isFocusing = false;

        // Re-enable scripts when object returns
        foreach (var script in movable.scriptsToToggle)
        {
            script.enabled = true;
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

        if (other.CompareTag("Player") && !isFocusing && !hasActivated)
        {
            // Disable scripts when activated
            foreach (var movable in movableObjects)
            {
                foreach (var script in movable.scriptsToToggle)
                {
                    script.enabled = false;
                }
            }
            StartCoroutine(ActivateWithDelay());
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
            Invoke(nameof(TriggerConnectedPlate), activationDelay);
        }
    }

    // Release the plate
    private void ReleasePlate()
    {
        isMoving = true;
        targetPosition = startPosition;
    }
    // Trigger the connected trap
    private void TriggerConnectedPlate()
    {
        plateObject.SetActive(true);
    }

    // Activate the trap with a delay
    private IEnumerator ActivateWithDelay()
    {
        yield return new WaitForSeconds(activationDelay);
        isFocusing = true;
        hasActivated = true;
    }
}