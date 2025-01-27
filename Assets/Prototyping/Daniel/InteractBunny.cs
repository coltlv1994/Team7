using Unity.VisualScripting;
using UnityEngine;
//Made by Daniel
public class InteractBunny : MonoBehaviour
{
    [SerializeField] LayerMask layerMask = LayerMask.GetMask("Bunny");
    [SerializeField] private int interactRange;
    private BunnyReceive BunnyReceive;
    private PrototypeTimer timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BunnyReceive = GameObject.Find("Bunny").GetComponent<BunnyReceive>();
        timer = GameObject.Find("Canvas").GetComponent<PrototypeTimer>();
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
                timer.NewDay();
            }
        }
    }
    private void OnTriggerEnter(Collider other) //Pauses timer when entering the Bunny's room
    {
        if (other.transform.tag == "BunnyRoom")
        {
            print("I am in here");
            timer.timeTicking = false;
        }
    }
    private void OnTriggerExit(Collider other) //Resumes timer when leaving the Bunny´s room
    {
        if (other.transform.tag == "BunnyRoom")
        {
            print("I am out in the dungeon");
            timer.timeTicking = true;
        }
    }
}
