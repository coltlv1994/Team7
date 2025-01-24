//Indra
using UnityEngine;

public interface IData 
{
    void LoadData(GameData data); //when loading the implemented script only cares about reading
    void SaveData(ref GameData data); //when we save data we allow the implementing script to modify the data
}
