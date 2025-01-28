using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
//Made by Daniel
public class PrototypeTimer : MonoBehaviour
{
    [SerializeField] int maxTime = 100, day = 1;
    [SerializeField] float time;
    public bool timeTicking;
    [SerializeField] private TMP_Text timerText, dayText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = maxTime;
        timerText = GameObject.Find("TimerUI").GetComponent<TMP_Text>();
        dayText = GameObject.Find("DayUI").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            TimeTick();
        }
        else
        {
            TimeOver();
        }
        timerText.text = "Time remaining: " + time.ToString("F2");
        dayText.text = "Day " + day;
    }

    private void TimeTick()
    {
        if (timeTicking == true) //Defined by the player gameobject through the InteractBunny script
        {
            time -= Time.deltaTime;
        }
    }

    private void TimeOver()
    {
        print("Time Out");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NewDay()
    {
        print("Mmm! Such food you have brought me, now get back in there or I will eat you!");
        day++;
        time = maxTime;
    }
}
