using Unity.VisualScripting;
using UnityEngine;
using TMPro;
//Made by Daniel
public class InteractBunny : MonoBehaviour
{
    // Whenever you feel that you need to add some more field to certain
    // atrributes that need to be save: check GameData class inside PrototypeTimer.
    // Write the attributes over there, and make sure it will be read/written from/to
    // the save file on disk.
    // Should you have any questions, contact Zhengyang

    [SerializeField] LayerMask layerMask;
    [SerializeField] private int interactRange, foodNeeded;
    [SerializeField] private PrototypeTimer timer;
    public GameObject m_saveWindow;

    Animator bunnyAnimator;

    [SerializeField] TextMeshProUGUI foodText;

    /*[System.NonSerialized]*/ public bool increaseTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //timer = GameObject.Find("Canvas").GetComponent<PrototypeTimer>();
        m_saveWindow.SetActive(false);
        timer.gameData.foods = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            print("looking for bunny");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange, layerMask) && timer.gameData.foods == foodNeeded)
            {
                // popup save window
                m_saveWindow.SetActive(true);
                UnityEngine.Cursor.lockState = CursorLockMode.Confined;
                UnityEngine.Cursor.visible = true;

                bunnyAnimator = hit.transform.gameObject.GetComponentInParent<Animator>();
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

            //UpdateFood();
            foodText.text = "Food: " + timer.gameData.foods.ToString() + "/" + foodNeeded;
        }
    }

    public void UpdateFood()
    {
        if (timer.gameData.foods < foodNeeded)
        {
            timer.gameData.foods++;
            foodText.text = "Food: " + timer.gameData.foods.ToString() + "/" + foodNeeded;
        }
    }

    public void OnClickSaveYes()
    {
        if (increaseTimer) timer.maxTime += 20;
        m_saveWindow.SetActive(false);
        // This will save game and start a new day
        // First, set food number right
        timer.gameData.foods = 0;
        foodText.text = "Food: " + timer.gameData.foods.ToString() + "/" + foodNeeded;

        // This function will fail/throw an exception, if save file under game directory is set to
        // "readonly". When it throws exception, it will block rest of codes in this function from executing,
        // and the save window may never disappear (but the day count will still +1 every time you click "yes".
        // To solve this problem, kindly remove the file's "readonly" attribute or delete the file.

        // then save the game
        //timer.NewDay();

        // resume UI status

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        //Animation stuff
        bunnyAnimator.SetTrigger("Thing");
    }

    public void OnClickSaveNo()
    {
        m_saveWindow.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }
}