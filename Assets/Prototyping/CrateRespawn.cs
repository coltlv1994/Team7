using UnityEngine;

public class CrateRespawn : MonoBehaviour
{
    Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Respawn"))
        {
            transform.position = initialPosition;
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }
}
