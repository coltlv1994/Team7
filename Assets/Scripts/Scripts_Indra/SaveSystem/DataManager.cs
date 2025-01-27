//Indra & Zhengyang
/*
 * Example of how youd call these methods from another script
 * 
 * public void OnNewGameClick(){DataManager.instance.NewGame();}
 * public void OnLoadGameClick(){DataManager.instance.LoadGame();}
 * public void OnSaveGameClick(){DataManager.instance.SaveGame();}
 * 
 */

using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private GameData gameData;
    public static DataManager instance { get; private set; }

    private readonly float m_dayTime = 15.0f; // day time by second

    private float m_timer = 0;

    // Note: program will automatically try to find the file under Team7 directory.
    //       I have not worked out how to use absolute path; but maybe it is good enough.
    string savePath = "savefile_team7_gp2.txt";

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("More than one DataManager in scene, get rid of the duplicate!!");
        }
        instance = this;
        
        // create game data
        gameData = new GameData();

        if (File.Exists(savePath))
        {
            // load from save
            LoadGame();
        }
        else
        {
            NewGame();
        }
    }

    private void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= m_dayTime)
        {
            m_timer = 0;
            Debug.Log("A new day has passed.");
            gameData.day += 1;
            // try autosave
            SaveGame();
        }
    }

    public void NewGame() 
    {
        // start loading scene and set door status according to gameData
    }

    public void LoadGame()
    {
        ReadFromSave();

        NewGame(); // now gameData has been changed.
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
}
