using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] public DialogueManager dialogueManager;

    public Dialogue dialogue;

    public void TriggerDialogue() 
    {
        dialogueManager.StartDialogue(dialogue);
    }
}
