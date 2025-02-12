using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class AnimationReceiver : MonoBehaviour
{
    [SerializeField] Image crossFade;
    [SerializeField] PrototypeTimer timer;
    [SerializeField] TextMeshProUGUI dayText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Image cakeImage;
    [SerializeField] Transform timerTransform;

    public bool hasCake = false;
    int cakeNumber = 0;
    Vector2 cakeOGPosition;

    private int currentMaxTime;

    ThroneGate gate;

    private void Start()
    {
        cakeOGPosition = cakeImage.transform.position;
        cakeImage.transform.gameObject.SetActive(false);
        currentMaxTime = (int)timer.maxTime;

        gate = FindAnyObjectByType<ThroneGate>();
    }

    public void CakeMethod(int cakeAmount)
    {
        cakeNumber = cakeAmount;
        hasCake = true;
    }

    public void SwitchDay()
    {
        StartCoroutine(CrossFadeLerp(1.0f)); 
    }

    private IEnumerator CrossFadeLerp(float duration)
    {
        float elapsedTime = 0;
        Color startColor = crossFade.color;
        Color endColor = new Color(0, 0, 0, 1);

        while (elapsedTime < duration)
        {
            crossFade.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        crossFade.color = endColor;

        timerText.text = "Max Time: " + currentMaxTime;
        dayText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);

        //timer.gameData.day += 1;
        //dayText.enabled = true;
        dayText.text = "Day: " + (timer.gameData.day + (uint)1);

        if (hasCake)
        {
            cakeImage.transform.gameObject.SetActive(true);
            Vector3 startPosition = cakeImage.transform.position;
            Vector3 endPosition = timerTransform.position;
            elapsedTime = 0;
            float moveDuration = 1.0f;

            while (elapsedTime < moveDuration)
            {
                cakeImage.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            //cakeImage.transform.gameObject.SetActive(true);
            cakeImage.transform.position = cakeOGPosition;
            //dayText.enabled = false;
            cakeImage.transform.gameObject.SetActive(false);

            StartCoroutine(AnimateTimerSize());

            yield return new WaitForSeconds(2.0f);
            dayText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);

            timer.NewDay();

            gate.StartCoroutine(gate.MoveGateUp());

            elapsedTime = 0;
            while (elapsedTime < duration)
            {
                crossFade.color = Color.Lerp(endColor, startColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            crossFade.color = startColor;

            hasCake = false;
        }
        else
        {
            yield return new WaitForSeconds(2.0f);
            dayText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
            timer.NewDay();

            gate.StartCoroutine(gate.MoveGateUp());

            elapsedTime = 0;
            while (elapsedTime < duration)
            {
                crossFade.color = Color.Lerp(endColor, startColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            crossFade.color = startColor;

            
        }
               
    }

    private IEnumerator AnimateTimerSize()
    {
        Vector3 originalScale = timerTransform.localScale;
        Vector3 targetScale = originalScale * 1.2f;
        float duration = 0.3f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            timerTransform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        timerTransform.localScale = targetScale;

        timerText.text = "Max Time: " + ((int)timer.maxTime + (cakeNumber * 20));
        cakeNumber = 0;
        currentMaxTime = (int)timer.maxTime;

        elapsedTime = 0;
        while (elapsedTime < duration)
        {
            timerTransform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        timerTransform.localScale = originalScale;

       
    }

    public IEnumerator scaleUp()
    {
        Vector3 originalScale = gameObject.transform.localScale;
        Vector3 targetScale = originalScale + new Vector3(0.2f, 0.2f, 0.2f);
        float duration = 0.3f;
        float currentTime = 0.0f;
        //do
        //{
        //    gameObject.transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / duration);
        //    currentTime += Time.deltaTime;
        //    yield return null;
        //} while (currentTime <= duration);

        while (currentTime < duration)
        {
            gameObject.transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}