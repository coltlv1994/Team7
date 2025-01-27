using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Background Music Settings")]
    public AudioClip background;
    [Range(0, 1)] public float backgroundVolume = 1.0f;

    [Header("SFX Settings")]
    public AudioClip collectable;
    [Range(0, 1)] public float collectableVolume = 1.0f;


    //make as many fields as you want as shown below for different sounds
    public AudioClip something1;
    [Range(0, 1)] public float somethingVolume1 = 1.0f;


    private void Start()
    {

        musicSource.clip = background;
        musicSource.volume = backgroundVolume;
        musicSource.Play();
    }


    public void PlaySFX(AudioClip clip)
    {
        if (clip == collectable)
            sfxSource.volume = collectableVolume;
        //fill this with all the various clips
        //else if (clip == whatever)
        //sfxSource.volume = whateverVolume;


        sfxSource.PlayOneShot(clip);
    }

    //public IEnumerator Fade(bool fadein, float duration, AudioSource source) //fade in function, place variable name of the audio source you want to fade
    //{
    //    float target = fadein ? somethingVolume : 0;
    //    float from = !fadein ? somethingVolume : 0;
    //    float time = 0;
    //    while (time < 1)
    //    {
    //        time += Time.deltaTime / duration;
    //        source.volume = Mathf.SmoothStep(from, target, time);
    //        yield return null;
    //    }
    //    source.volume = target;

    //}
}

