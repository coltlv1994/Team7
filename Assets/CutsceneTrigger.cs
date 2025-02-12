using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneTrigger : MonoBehaviour
{
    PrototypeTimer timer;
    [SerializeField] private PlayableDirector cutscene;

    [SerializeField] GameObject[] stuffToDisable;

    private void Start()
    {
        timer = FindAnyObjectByType<PrototypeTimer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cutscene.Play();
            other.GetComponent<MeshRenderer>().enabled = false;
            timer.PauseTimer(true);

            foreach (GameObject obj in stuffToDisable)
            {
                obj.SetActive(false);
            }
        }
    }

}
