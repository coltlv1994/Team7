//Indra
using System;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class GameData
{
    public int deathCount;

    public uint day = 1;
    public uint foods = 0;
    //the values defined here will be the default values 
    //that the game starts with on a new game (when theres no data to load)

    public void ParseFromDict(Dictionary<string, string> p_dict)
    {
        // Need extra checking in case dictionary has invalid entries due to corrupted save file
        string outValue;

        if (p_dict.TryGetValue("day", out outValue))
        {
            day = Convert.ToUInt32(outValue);
        }

        if (p_dict.TryGetValue("foods", out outValue))
        {
            foods = Convert.ToUInt32(outValue);
        }
    }

    public Dictionary<string, string> WriteToDict()
    {
        Dictionary<string, string> p_dict = new Dictionary<string, string>();

        p_dict["day"] = day.ToString();
        p_dict["foods"] = foods.ToString();

        return p_dict;
    }
}
