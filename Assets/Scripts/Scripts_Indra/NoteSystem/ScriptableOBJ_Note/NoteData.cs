using UnityEngine;

[CreateAssetMenu(fileName = "NoteData", menuName = "ScriptableObjects/Notes", order = 1)]
public class NoteData : ScriptableObject 
{
    [TextArea(3, 10)]
    public string noteText;

    public int numberOfPrefabsToCreate;
    public Vector3[] spawnPoints;
}
