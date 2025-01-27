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
    public uint coins = 0;
    public uint foods = 0;
    public uint keys = 0;
    public bool door_1 = false;
    public bool door_2 = false;
    public bool door_3 = false;
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

        if (p_dict.TryGetValue("coins", out outValue))
        {
            coins = Convert.ToUInt32(outValue);
        }

        if (p_dict.TryGetValue("foods", out outValue))
        {
            foods = Convert.ToUInt32(outValue);
        }

        if (p_dict.TryGetValue("keys", out outValue))
        {
            keys = Convert.ToUInt32(outValue);
        }

        if (p_dict.TryGetValue("door_1", out outValue))
        {
            door_1 = Convert.ToBoolean(outValue);
        }

        if (p_dict.TryGetValue("door_2", out outValue))
        {
            door_2 = Convert.ToBoolean(outValue);
        }

        if (p_dict.TryGetValue("door_3", out outValue))
        {
            door_3 = Convert.ToBoolean(outValue);
        }
    }

    public Dictionary<string, string> WriteToDict()
    {
        Dictionary<string, string> p_dict = new Dictionary<string, string>();

        p_dict["day"] = day.ToString();
        p_dict["coins"] = coins.ToString();
        p_dict["foods"] = foods.ToString();
        p_dict["keys"] = keys.ToString();
        p_dict["door_1"] = door_1.ToString();
        p_dict["door_2"] = door_2.ToString();
        p_dict["door_3"] = door_3.ToString();

        return p_dict;
    }
}
