using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector cutscene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cutscene.Play();
            other.GetComponent<MeshRenderer>().enabled = false;
        }
    }

}
