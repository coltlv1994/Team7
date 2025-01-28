using Unity.VisualScripting;
using UnityEngine;
//Made by Daniel
public class InteractBunny : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] private int interactRange;
    private PrototypeTimer timer;
    public GameObject m_saveWindow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = GameObject.Find("Canvas").GetComponent<PrototypeTimer>();
        m_saveWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            print("looking for bunny");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange, layerMask))
            {
                // popup save window
                m_saveWindow.SetActive(true);
                UnityEngine.Cursor.lockState = CursorLockMode.Confined;
                UnityEngine.Cursor.visible = true;
            }
        }
        timer.UpdateFromManager(Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other) //Pauses timer when entering the Bunny's room
    {
        if (other.transform.tag == "BunnyRoom")
        {
            print("I am in here");
            timer.timeTicking = false;
        }
    }
    private void OnTriggerExit(Collider other) //Resumes timer when leaving the Bunny's room
    {
        if (other.transform.tag == "BunnyRoom")
        {
            print("I am out in the dungeon");
            timer.timeTicking = true;
        }
    }

    public void OnClickSaveYes()
    {
        // This will save game and start a new day
        timer.NewDay();
        m_saveWindow.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    public void OnClickSaveNo()
    {
        m_saveWindow.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }
}
