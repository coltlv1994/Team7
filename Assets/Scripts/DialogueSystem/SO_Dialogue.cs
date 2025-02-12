//Created by Linus Jernström
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dialogue")]
    public class SO_Dialogue : ScriptableObject
    {
        public string speakerName;
        public string text;
        public float displayTime;
    }
}
