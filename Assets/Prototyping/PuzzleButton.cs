using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using System.Collections;
using System;
using Unity.VisualScripting;

public class PuzzleButton : MonoBehaviour
{
    [SerializeField] bool remainPressed;

    public bool IsPressed = false;
    [SerializeField] AudioManager audioManager;
    [SerializeField] AudioClip buttonPressOn;
    [SerializeField] AudioClip buttonPressOff;

    bool inMotion;

    private void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.name == "PlayerNew" || other.gameObject.name == "Crate") && !inMotion)
        {
            audioManager.PlaySFX(buttonPressOn);
            IsPressed = true;

            StartCoroutine(SquishButton());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.name == "PlayerNew" || other.gameObject.name == "Crate") && !inMotion)
        {
            if (!remainPressed)
            {
                audioManager.PlaySFX(buttonPressOff);
                StartCoroutine(UnSquishButton());
                IsPressed = false;
            }
            else
            {

            }   
            
            
        }
    }

    private IEnumerator SquishButton()
    {
        inMotion = true;
        Transform scaleObject = transform.GetChild(0);
        float elapsedTime = 0f;
        float duration = 0.2f;
        Vector3 startScale = scaleObject.localScale;
        Vector3 endScale = new Vector3(startScale.x, 0.2f, startScale.z);

        while (elapsedTime < duration)
        {
            scaleObject.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        scaleObject.localScale = endScale;
        inMotion = false;
    }

    private IEnumerator UnSquishButton()
    {
        inMotion = true;
        Transform scaleObject = transform.GetChild(0);
        float elapsedTime = 0f;
        float duration = 0.2f;
        Vector3 startScale = scaleObject.localScale;
        Vector3 endScale = new Vector3(startScale.x, 1f, startScale.z);

        while (elapsedTime < duration)
        {
            scaleObject.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        scaleObject.localScale = endScale;
        inMotion = false;
    }
}