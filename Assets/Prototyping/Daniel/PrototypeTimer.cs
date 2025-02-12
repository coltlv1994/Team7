using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DialogueSystem;
using Unity.VisualScripting;
using NUnit.Framework;
using System;
using System.Linq;

//Made by Daniel
public class PrototypeTimer : MonoBehaviour
{
    [SerializeField] public uint maxTime = 15;
    [SerializeField] public float time;
    public bool timeTicking;
    public GameSettingsPersistent settings;
    [SerializeField] private TMP_Text timerText, dayText;
    [SerializeField] private CS_BunnyDialogueManager _dialogueManager;

    // Imported from Indra/Zhengyang's work
    public GameData gameData;
    string savePath = "savefile_team7_gp2.txt";

    public List<GameObject> m_buttonOBJS;
    public List<GameObject> m_doorsOpened;

    public List<GameObject> m_crateOBJS;
    public List<Vector3> m_crateLocations;

    void Awake()
    {
        // create game data
        gameData = new GameData();
    }

    public void PauseTimer(bool pause) //added this bool for pausing timer
    {
        timeTicking = !pause;
    }
    private void OnEnable()
    {
        m_buttonOBJS.Clear();
        m_crateOBJS.Clear();

        foreach (GameObject button in GameObject.FindGameObjectsWithTag("Button"))
        {
            m_buttonOBJS.Add(button);
        }
        foreach (GameObject checkCrate in GameObject.FindGameObjectsWithTag("Crate"))
        {
            m_crateOBJS.Add(checkCrate);
        }
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

        foreach (GameObject button in m_buttonOBJS)
        {
            bool isButtonPressed = button.GetComponent<PuzzleButton>().IsPressed;
            if (isButtonPressed)
            {
                //m_doorsOpened.Add(button);
                // only write pressed button's position
                sw.WriteLine("buttonPressed=" + button.transform.position.x + "," + button.transform.position.y + "," + button.transform.position.z);
            }
        }

        foreach (GameObject checkCrate in m_crateOBJS)
        {
            // write to file
            // note: it offsets crate's location to make sure it won't stuck at ground
            // could be removed if this is deemed unnecessary.
            sw.WriteLine("crate=" + checkCrate.transform.position.x + "," + (checkCrate.transform.position.y + 2.0f) + "," + checkCrate.transform.position.z);
        }

        sw.Close();
        Debug.Log("Game Saved.");
    }

    public void ReadFromSave()
    {
        // TODO: add execption handlers in this function
        Dictionary<string, string> m_inputStatus = new Dictionary<string, string>();
        List<Vector3> m_crateRespawnLocation = new List<Vector3>();
        List<float> m_buttonDistance = new List<float>();

        foreach (string line in File.ReadLines(savePath))
        {
            if (line[0] == ';')
            {
                // allow comments starting with semicolon.
                // note: comments won't be written back to save files
                continue;
            }

            string[] fields = line.Split("=");
            if (fields[0] == "crate")
            {
                // read crate location
                string[] position = fields[1].Split(",");
                Vector3 crateLocation = new Vector3(float.Parse(position[0]), float.Parse(position[1]), float.Parse(position[2]));
                m_crateRespawnLocation.Add(crateLocation);
            }
            else
            {
                if (fields[0] == "buttonPressed")
                {
                    // restore button
                    string[] position = fields[1].Split(",");
                    Vector3 buttonLocation = new Vector3(float.Parse(position[0]), float.Parse(position[1]), float.Parse(position[2]));
                    m_buttonDistance.Clear();
                    foreach (GameObject button in m_buttonOBJS)
                    {
                        m_buttonDistance.Add((button.transform.position - buttonLocation).magnitude);
                    }

                    int minIndex = m_buttonDistance.IndexOf(m_buttonDistance.Min());
                    PuzzleButton p_button = m_buttonOBJS[minIndex].GetComponent<PuzzleButton>();
                    p_button.IsPressed = true;
                    StartCoroutine(p_button.SquishButton());
                }
                else
                {
                    m_inputStatus[fields[0]] = fields[1]; // read it and cover default value
                }
            }
        }

        // reset crate location
        // make sure no illegal access
        int noOfCrates = Mathf.Min(m_crateRespawnLocation.Count, m_crateOBJS.Count);
        for (int i = 0; i < noOfCrates; i++)
        {
            m_crateOBJS[i].transform.position = m_crateRespawnLocation[i];
        }

        gameData.ParseFromDict(m_inputStatus);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settings = GameObject.FindGameObjectWithTag("GlobalSettings")?.GetComponent<GameSettingsPersistent>();
        if (settings == null)
        {
            settings = gameObject.AddComponent<GameSettingsPersistent>();
            // NOTE: this line is for debugging purpose and should be removed before final build.
            settings.isLoadingSave = true;
        }


        if (File.Exists(savePath) && settings.isLoadingSave == true)
        {
            // load from save
            ReadFromSave();
            settings.isLoadingSave = false;
        }

        if (File.Exists(savePath) && settings.isLoadingSave == false)
        {
            // we should delete the file
            File.Delete(savePath);
        }

        time = maxTime;
        timerText = GameObject.Find("TimerUI").GetComponent<TMP_Text>();
        dayText = GameObject.Find("DayUI").GetComponent<TMP_Text>();
    }

    public void UpdateFromManager(float p_deltaTime)
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.CurrentGameState == GameState.Pause) return;
        if (time > 0)
        {
            TimeTick(p_deltaTime);
        }
        else
        {
            TimeOver();
        }
        timerText.text = "Bunny Patience: " + time.ToString("F2");
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
        settings.isLoadingSave = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NewDay()
    {
        print("Mmm! Such food you have brought me, now get back in there or I will eat you!");
        gameData.day += 1;
        time = maxTime;
        SaveGame(); // autosave

        _dialogueManager.RunDialogue(Mathf.CeilToInt(gameData.day));
    }
}
