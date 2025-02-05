// Indra (& Linus)
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject UI;
    public Dialogue baseDialogue;
    public float secondsToShow = 5f;

    private string[] _messages => baseDialogue.sentences;

    private PrototypeTimer _timer;
    void Start()
    {
        _timer = GameObject.FindObjectsByType<PrototypeTimer>(FindObjectsSortMode.None)[0];
        StartDialogue(1);
    }

    public void StartDialogue(int day) 
    {
        UI.SetActive(true);
        //Debug.Log("Chatting with " + dialogue.npcName);
        
        nameText.text = baseDialogue.npcName;

        dialogueText.text = _messages[day - 1];

        StartCoroutine(CloseAfterSeconds());
    }

    public void StartDialogue(Dialogue dialogue)
    {
        
    }
    
    public IEnumerator CloseAfterSeconds()
    {
        yield return new WaitForSeconds(secondsToShow);
        
        EndDialogue();
    }

    public void EndDialogue() 
    {
        UI.SetActive(false);
        Debug.Log("Chat end");
    }
}
