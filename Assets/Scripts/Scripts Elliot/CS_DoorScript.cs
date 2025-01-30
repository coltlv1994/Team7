using Interactables;
using UnityEngine;
using static CS_EnemyScript;
using static UnityEngine.Rendering.DebugUI;

public class CS_DoorScript : CS_InteractableObject //Created by Elliot
{
    private float m_changeStateTimer = 0;
    public float m_doorInteractTime;
    private Quaternion m_rotateRight;
    private Quaternion m_rotateLeft;
    private bool interactWithDoor = true;

    [SerializeField] GameObject m_doorHolder;
    private CS_InteractableObject interactableObject;

    [Header("DoorCurrentState")]
    public DoorState state = 0;

    public enum DoorState
    {
        ClosedState = 0,
        OpenState = 1,
        Idle = 2
    }

    private void Awake()
    {
        m_rotateRight = Quaternion.AngleAxis(90, transform.up);
        m_rotateLeft = Quaternion.AngleAxis(0, transform.up);
        state = DoorState.Idle;
        interactableObject = transform.GetChild(0).GetComponent<CS_InteractableObject>();
    }
    public override void OnInteract()
    {
        //if (interactWithDoor) state = DoorState.OpenState;
        Debug.Log("INTERACT!");
    }
    public override void OnActivate()
    {
        if (interactWithDoor) state = DoorState.OpenState;
        Debug.Log("ACTIVATE");
    }
    public override void OnDeactivate()
    {
        if (interactWithDoor) state = DoorState.ClosedState;
        Debug.Log("DEACTIVATE");
    }

    public void Update()
    {      
            if (state == DoorState.ClosedState)
            {
                interactWithDoor = false;
                m_changeStateTimer += Time.deltaTime;
                m_doorHolder.transform.rotation = Quaternion.Lerp(m_doorHolder.transform.rotation, m_rotateLeft, m_changeStateTimer);
                if (m_changeStateTimer >= m_doorInteractTime)
                {
                    state = DoorState.Idle;
                    m_changeStateTimer = 0;
                    interactWithDoor = true;
                }
            }
            else if (state == DoorState.OpenState)
            {
                interactWithDoor = false;
                m_changeStateTimer += Time.deltaTime;
                m_doorHolder.transform.rotation = Quaternion.Lerp(m_doorHolder.transform.rotation, m_rotateRight, m_changeStateTimer);
                if (m_changeStateTimer >= m_doorInteractTime)
                {
                    state = DoorState.Idle;
                    m_changeStateTimer = 0;
                    interactWithDoor = true;
                }
            }                
    }
}
