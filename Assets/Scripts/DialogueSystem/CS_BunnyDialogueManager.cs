//Created by Linus Jernström
using System.Collections;
using TMPro;
using UnityEngine;

namespace DialogueSystem
{
    public class CS_BunnyDialogueManager : MonoBehaviour
    {
        [SerializeField] private SO_Dialogue[] _dialogues;
        [SerializeField][Range(0, 1)] private float _readSpeed = .1f;
        
        private SO_Dialogue _currentDialogue;
        
        private TextMeshProUGUI _nameText;
        private TextMeshProUGUI _dialogueText;
        private GameObject _uiCanvas;

        private string[] _words;
        private string _text;
        private float _counter;

        private void OnEnable()
        {
            _uiCanvas = GameObject.FindWithTag("DialogueCanvas");

            TextMeshProUGUI[] textElements = _uiCanvas.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var text in textElements)
            {
                if (text.gameObject.name == "NameText")
                    _nameText = text;
                else if (text.gameObject.name == "DialogueText")
                    _dialogueText = text;
            }
            _uiCanvas.gameObject.SetActive(false);
        }

        void Start()
        {
            RunDialogue(1);
        }

        public void RunDialogue(int day)
        {
            StartDialogue(_dialogues[day - 1]);
        }

        private void StartDialogue(SO_Dialogue dialogue)
        {
            _uiCanvas.gameObject.SetActive(true);
            _currentDialogue = dialogue;
            _nameText.text = dialogue.speakerName;

            _words = dialogue.text.Split(" ");

            _text = _words[0] + " ";
            StartCoroutine(ShowDialogue());
        }

        private IEnumerator ShowDialogue()
        {
            yield return new WaitForSeconds(_readSpeed);
            _counter++;
            if (Mathf.CeilToInt(_counter) < _words.Length)
            {
                _text += _words[Mathf.CeilToInt(_counter)] + " ";
                _dialogueText.text = _text;
                StartCoroutine(ShowDialogue());
                yield return null;
            }
            else
            {
                _counter = 0f;
                StartCoroutine(CloseAfterSeconds());
            }
        }

        private IEnumerator CloseAfterSeconds()
        {
            yield return new WaitForSeconds(_currentDialogue.displayTime);
            
            EndDialogue();
        }

        private void EndDialogue()
        {
            _uiCanvas.gameObject.SetActive(false);
        }
    }
}
