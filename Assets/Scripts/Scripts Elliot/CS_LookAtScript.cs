using UnityEngine;

public class CS_LookAtScript : MonoBehaviour
{
    public GameObject playerOBJ;
    // Update is called once per frame
    void Update()
    {
       this.transform.localPosition = new Vector3(playerOBJ.transform.localPosition.x, this.transform.localPosition.y, playerOBJ.transform.localPosition.z);
    }
}
