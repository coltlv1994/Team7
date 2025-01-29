using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Text;

//Made by Daniel
public class PrototypeTimer : MonoBehaviour
{
    [SerializeField] uint maxTime = 15;
    [SerializeField] float time;
    public bool timeTicking;
    [SerializeField] private TMP_Text timerText, dayText;

    // Imported from Indra/Zhengyang's work
    public GameData gameData;
    string savePath = "savefile_team7_gp2.txt";

    void Awake()
    {
        // create game data
        gameData = new GameData();
    }

    public void SaveGame()
    {

        Dictionary<string, string> m_dataToSave = gameData.WriteToDict();

        // false means the file will be overwritten.
        // public StreamWriter(string path, bool append, Encoding encoding);
        StreamWriter sw = new StreamWriter(savePath, false, Encoding.ASCII);

        foreach (KeyValuePair<string, string> kvp in m_dataToSave)
        {
            sw.WriteLine(kvp.Key + "=" + kvp.Value);
        }

        sw.Close();

        Debug.Log("Game Saved.");
    }

    public void ReadFromSave()
    {
        // TODO: add execption handlers in this function
        Dictionary<string, string> m_inputStatus = new Dictionary<string, string>();

        foreach (string line in File.ReadLines(savePath))
        {
            if (line[0] == ';')
            {
                // allow comments starting with semicolon.
                // note: comments won't be written back to save files
                continue;
            }

            string[] fields = line.Split("=");
            m_inputStatus[fields[0]] = fields[1]; // read it and cover default value
        }

        gameData.ParseFromDict(m_inputStatus);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (File.Exists(savePath))
        {
            // load from save
            ReadFromSave();
        }

        time = maxTime;
        timerText = GameObject.Find("TimerUI").GetComponent<TMP_Text>();
        dayText = GameObject.Find("DayUI").GetComponent<TMP_Text>();
    }

    public void UpdateFromManager(float p_deltaTime)
    {
        if (time > 0)
        {
            TimeTick(p_deltaTime);
        }
        else
        {
            TimeOver();
        }
        timerText.text = "Time remaining: " + time.ToString("F2");
        dayText.text = "Day " + gameData.day;
    }

    private void TimeTick(float p_deltaTime)
    {
        if (timeTicking == true) //Defined by the player gameobject through the InteractBunny script
        {
            time -= p_deltaTime;
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
        gameData.day += 1;
        time = maxTime;
        SaveGame(); // autosave
    }
}
