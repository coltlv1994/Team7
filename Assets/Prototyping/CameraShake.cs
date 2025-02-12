using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    PrototypeTimer m_timer;

    [Header("Shake Settings")]
    [SerializeField] private float shakeAmount = 0.1f;
    [SerializeField] private float shakeDuration = 1.0f;

    private Vector3 initialPosition;
    private bool isShaking = false;
    private bool hasShaken1 = false;
    private bool hasShaken2 = false;

    [SerializeField] AudioSource bunnySource;
    AudioManager audioManager;
    [SerializeField] AudioClip bunnyClip;
    [SerializeField] AudioClip tickTock;

    private void Awake()
    {
        initialPosition = transform.localPosition;
        m_timer = FindAnyObjectByType<PrototypeTimer>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void Update()
    {
        if (isShaking)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeAmount;
        }

        if (m_timer != null && m_timer.time < (m_timer.maxTime / 2))
        {
            if (!hasShaken1)
            {
                StartShake(shakeAmount);
                hasShaken1 = true;

                audioManager.PlaySFX(tickTock);
            }

            if (m_timer != null && m_timer.time < ((m_timer.maxTime / 2) / 2))
            {
                if (!hasShaken2)
                {
                    StartShake(shakeAmount * 3);
                    hasShaken2 = true;

                    audioManager.PlaySFX(tickTock);
                }
            }
        }
    }

    public void StartShake(float shakeAmount)
    {
        if (!isShaking)
        {
            StartCoroutine(Shake(shakeAmount));
            bunnySource.PlayOneShot(bunnyClip);

            
        }
    }

    private IEnumerator Shake(float shakeAmount)
    {
        isShaking = true;
        yield return new WaitForSeconds(shakeDuration);
        isShaking = false;
        transform.localPosition = initialPosition;
    }
}