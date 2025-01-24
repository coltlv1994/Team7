//Indra
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

    // for testing
    string savePath = "%USERPROFILE%\\savefile_team7_gp2.txt";

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("More than one DataManager in scene, get rid of the duplicate!!");
        }
        instance = this;
        
        // create game data
        gameData = new GameData();
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

        // For testing purpose
        // this can be further changed like command line parameters or other method

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
