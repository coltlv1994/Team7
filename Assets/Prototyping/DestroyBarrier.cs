using UnityEngine;

public class DestroyBarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "BlockadesChild")
        {
            Destroy(other.transform.parent.gameObject);
            //play sounds
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.name == "Blockades")
    //    {
    //        //collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    //        Destroy(collision.gameObject);
    //        //play sounds
    //    }
    //}
}
