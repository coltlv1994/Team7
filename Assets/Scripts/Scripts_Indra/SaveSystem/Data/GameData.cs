//Indra
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int deathCount;
    //the values defined here will be the default values 
    //that the game starts with on a new game (when theres no data to load)
    public GameData() 
    {
        this.deathCount = 0;
    }
}
