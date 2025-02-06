//Indra
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Interactables;
using System.Threading;
public class NoteController : CS_InteractableObject
{
    [Header("Input")]
    [SerializeField] private KeyCode closeKey;

    [Space(10)]
    [SerializeField] private FPSController player;

    [Header("UI Text")]
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private TMP_Text noteTextUI;
    [SerializeField] private NoteData noteData;

    //[Space(10)]
    //[SerializeField][TextArea] private string noteText;
    [SerializeField] private PrototypeTimer protoTimer;

    [Space(10)]
    [SerializeField] private UnityEvent openEvent;
    //private bool isOpen = false; // redundant

    public override void OnActivate()
    {
        if (noteData == null)
        {
            Debug.LogWarning("NoteData is not assigned on " + gameObject.name);
            return;
        }

        if (noteTextUI == null || noteCanvas == null)
        {
            Debug.LogWarning("UI components are not assigned on " + gameObject.name);
            return;
        }

        noteTextUI.text = noteData.noteText;
        noteCanvas.SetActive(true);
        openEvent.Invoke();
        DisablePlayer(true);
        //isOpen = true; // redundant

        if (protoTimer != null) 
        {
            protoTimer.PauseTimer(true); //stops timer
        }
    }

    public override void OnDeactivate()
    {
        noteCanvas.SetActive(false);
        DisablePlayer(false);
        // isOpen = false; // redundant
        if (protoTimer != null)
        {
            protoTimer.PauseTimer(false); // resumes timer
        }

    }

    public override void OnInteract()
    {

    }

    public void ShowNote()
    {

    }


    void DisableNote() 
    {

    }

    void DisablePlayer(bool disable) 
    {
        player.enabled = !disable;
    }

    private void Update()
    {
        //if (isOpen) 
        //{
        //    if (Input.GetKeyDown(closeKey)) 
        //    {
        //        DisableNote();
        //    }
        //}
    }
}
