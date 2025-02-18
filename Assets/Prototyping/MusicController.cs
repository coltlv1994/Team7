using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioSource musicSource;
    [SerializeField] AudioClip musicClip;

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
        //musicSource.Play();
    }
}
