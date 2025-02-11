using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
//Made by Daniel
public class InteractBunny : MonoBehaviour
{
    // Whenever you feel that you need to add some more field to certain
    // atrributes that need to be save: check GameData class inside PrototypeTimer.
    // Write the attributes over there, and make sure it will be read/written from/to
    // the save file on disk.
    // Should you have any questions, contact Zhengyang

    [SerializeField] LayerMask layerMask;
    [SerializeField] private uint interactRange, foodNeeded;
    public GameObject m_saveWindow;
    // not sure if it is needed
    public GameObject m_timerParent;

    [SerializeField] public Image crossFade;
    [SerializeField] float initialDelay = 2.0f;
    [SerializeField] float displayTime = 2.0f;

    Animator bunnyAnimator;

    [SerializeField] TextMeshProUGUI foodText;

    /*[System.NonSerialized]*/ public bool increaseTimer;
    public int supaCarrotCount;

    public const int maxFoods = 3; //added by indra, this is to ensure a cap for collectables

    public PrototypeTimer pTimer;
    void Start()
    {
        //timer = GameObject.Find("Canvas").GetComponent<PrototypeTimer>();
        m_saveWindow.SetActive(false);
        pTimer = FindAnyObjectByType<PrototypeTimer>();
        // or try below line?
        //pTimer = m_timerParent.GetComponent<PrototypeTimer>();
        pTimer.gameData.foods = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            print("looking for bunny");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange, layerMask) && pTimer.gameData.foods >= foodNeeded)
            {
                // popup save window
                m_saveWindow.SetActive(true);
                UnityEngine.Cursor.lockState = CursorLockMode.Confined;
                UnityEngine.Cursor.visible = true;

                bunnyAnimator = hit.transform.gameObject.GetComponentInParent<Animator>();
            }
        }
        pTimer.UpdateFromManager(Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other) //Pauses timer when entering the Bunny's room
    {
        if (other.transform.tag == "BunnyRoom")
        {
            print("I am in here");
            pTimer.timeTicking = false;
        }
    }
    private void OnTriggerExit(Collider other) //Resumes timer when leaving the Bunny's room
    {
        if (other.transform.tag == "BunnyRoom")
        {
            print("I am out in the dungeon");
            pTimer.timeTicking = true;

            //UpdateFood();
            foodText.text = "Food: " + pTimer.gameData.foods.ToString() + "/" + foodNeeded;
        }
    }

    public void UpdateFood()
    {
        if (pTimer.gameData.foods < maxFoods)
        {
            pTimer.gameData.foods++;
            foodText.text = "Food: " + pTimer.gameData.foods.ToString() + "/" + foodNeeded;
        }
    }

    public void OnClickSaveYes()
    {
        if (increaseTimer)
        {
            pTimer.maxTime += (uint)(supaCarrotCount * 20);
            AnimationReceiver animRec = FindAnyObjectByType<AnimationReceiver>();
            animRec.hasCake = true;

        }
        m_saveWindow.SetActive(false);
        // This will save game and start a new day
        // First, set food number right
        pTimer.gameData.foods -= foodNeeded;
        foodText.text = "Food: " + pTimer.gameData.foods.ToString() + "/" + foodNeeded;

        // This function will fail/throw an exception, if save file under game directory is set to
        // "readonly". When it throws exception, it will block rest of codes in this function from executing,
        // and the save window may never disappear (but the day count will still +1 every time you click "yes".
        // To solve this problem, kindly remove the file's "readonly" attribute or delete the file.

        // then save the game
        pTimer.NewDay();
        //StartCoroutine(CrossFadeLerpInAndOut(initialDelay, displayTime));
        // resume UI status

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        //Animation stuff
        bunnyAnimator.SetTrigger("Thing");
    }

    private IEnumerator Crossfade(float initialDelay, float dt)
    {
        yield return new WaitForSeconds(initialDelay);
        crossFade.gameObject.SetActive(true);
        yield return new WaitForSeconds(dt);
        crossFade.gameObject.SetActive(false);
    }
    
    

    public void OnClickSaveNo()
    {
        m_saveWindow.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }
}