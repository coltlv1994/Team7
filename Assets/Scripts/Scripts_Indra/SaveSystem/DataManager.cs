//Indra
/*
 * Example of how youd call these methods from another script
 * 
 * public void OnNewGameClick(){DataManager.instance.NewGame();}
 * public void OnLoadGameClick(){DataManager.instance.LoadGame();}
 * public void OnSaveGameClick(){DataManager.instance.SaveGame();}
 * 
 */

using UnityEngine;

public class DataManager : MonoBehaviour
{
    private GameData gameData;
    public static DataManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("More than one DataManager in scene, get rid of the duplicate!!");
        }
        instance = this;    
    }



    public void NewGame() 
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //to do - load any save data from file using data handler
        //if no data to load initialize new game

        if (this.gameData == null) 
        {
            Debug.Log("No data found. Initializing default data.");
            NewGame();
        }
        //to do - push loaded data to all other relevant scripts
    }

    public void SaveGame()
    {
        //to do - pass data to other scripts so they can update it
        //to do - save that data to a file using data handler
    }
}
