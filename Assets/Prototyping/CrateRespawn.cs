using UnityEngine;

public class CrateRespawn : MonoBehaviour
{
    Vector3 initialPosition;
    [SerializeField] private GameObject respawnObject;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == respawnObject)
        {
            transform.position = initialPosition;
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        GameObject.Find("Fab_CHONK_Bunny").GetComponent<Animator>().SetTrigger("Thing");
    //    }
    //}
}
