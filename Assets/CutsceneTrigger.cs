using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections;

public class CutsceneTrigger : MonoBehaviour
{
    PrototypeTimer timer;
    [SerializeField] private PlayableDirector cutscene;

    [SerializeField] GameObject[] stuffToDisable;


    End end;
    private void Start()
    {
        timer = FindAnyObjectByType<PrototypeTimer>();
        end = FindAnyObjectByType<End>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cutscene.Play();
            other.GetComponent<MeshRenderer>().enabled = false;
            other.GetComponent<Footsteps>().enabled = false;
            timer.PauseTimer(true);

            foreach (GameObject obj in stuffToDisable)
            {
                obj.SetActive(false);
            }
            StartCoroutine(End());
        }

    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(23.5f);
        end.StartCoroutine(end.CrossFadeLerpWhite(3.5f));
    }
}
