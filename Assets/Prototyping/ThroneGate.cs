using UnityEngine;
using System.Collections;

public class ThroneGate : MonoBehaviour
{
    [SerializeField] float duration = 3;
    [SerializeField] float moveHeight = 3;
    Vector3 ogPos;

    void Start()
    {
        ogPos = transform.position;
    }

    public IEnumerator MoveGateDown()
    {
        float elapsedTime = 0;
        Vector3 startPos = ogPos;
        Vector3 endPos = new Vector3(transform.position.x, transform.position.y - moveHeight, transform.position.z);

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    }
    public IEnumerator MoveGateUp()
    {
        float elapsedTime = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = ogPos;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    }
}
