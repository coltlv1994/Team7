using UnityEngine;

public class DestroyBarrier : MonoBehaviour
{
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.transform.name == "Blockades")
    //    {
    //        Destroy(other.gameObject);
    //        //play sounds
    //    }
    //}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Blockades")
        {
            Destroy(collision.gameObject);
            //play sounds
        }
    }
}
