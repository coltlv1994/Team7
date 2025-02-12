using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    //MAKE ENDING HERE
    [SerializeField] Image crossFadeWhite;
    [SerializeField] Image crossFadeBlack;
    [SerializeField] TextMeshProUGUI dayText;

    PrototypeTimer timer;


    private void Start()
    {
        timer = FindAnyObjectByType<PrototypeTimer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CrossFadeLerpWhite(3.5f));
        }
    }

    private IEnumerator CrossFadeLerpWhite(float duration)
    {
        float elapsedTime = 0;
        Color startColor = crossFadeWhite.color;
        Color endColor = new Color(1, 1, 1, 1);
        Debug.Log("CrossfadeLerpWhite");
        while (elapsedTime < duration)
        {
            crossFadeWhite.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        crossFadeWhite.color = endColor;


        dayText.gameObject.SetActive(true);

        dayText.text = "You Escaped the Bungeon on Day " + (timer.gameData.day); ;

        yield return new WaitForSeconds(6);

        StartCoroutine(CrossFadeLerpBlack(2.5f));
    }

    private IEnumerator CrossFadeLerpBlack(float duration)
    {
        float elapsedTime = 0;
        Color startColor = new Color(1, 1, 1, 1);
        Color endColor = new Color(0, 0, 0, 1);
        Debug.Log("CrossfadeLerpBLACK");
        while (elapsedTime < duration)
        {
            crossFadeWhite.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        crossFadeWhite.color = endColor;


        dayText.gameObject.SetActive(true);

        dayText.text = "You Escaped the Bungeon on Day" + (timer.gameData.day); ;

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);

    }
}
