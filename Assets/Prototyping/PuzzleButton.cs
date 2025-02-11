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
    Collider buttonCollider;

    bool playerOnButton = false;

    private void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        buttonCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.name == "PlayerNew" && !inMotion)) //|| other.gameObject.name == "Crate") 
        {
            audioManager.PlaySFX(buttonPressOn);
            IsPressed = true;
            playerOnButton = true;
            StartCoroutine(SquishButton());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.name == "PlayerNew" && !inMotion)) //|| other.gameObject.name == "Crate") 
        {
            if (!remainPressed)
            {
                audioManager.PlaySFX(buttonPressOff);
                StartCoroutine(UnSquishButton());
                IsPressed = false;
                playerOnButton = false;
            }
        }
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapBox(buttonCollider.bounds.center, buttonCollider.bounds.extents, Quaternion.identity);
        bool crateOnButton = false;

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.name == "Crate")
            {
                crateOnButton = true;
                break;
            }
        }

        if (crateOnButton && !IsPressed && !inMotion)
        {
            audioManager.PlaySFX(buttonPressOn);
            IsPressed = true;
            StartCoroutine(SquishButton());
        }
        else if (!crateOnButton && IsPressed && !remainPressed && !inMotion && !playerOnButton)
        {
            audioManager.PlaySFX(buttonPressOff);
            IsPressed = false;
            StartCoroutine(UnSquishButton());
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