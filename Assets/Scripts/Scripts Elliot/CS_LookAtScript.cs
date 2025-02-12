using Unity.VisualScripting;
using UnityEngine;

public class CS_LookAtScript : MonoBehaviour
{
    public GameObject playerOBJ;
    public float yAxis;
    // Update is called once per frame


    private void Start()
    {
        playerOBJ = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if(playerOBJ.transform.position.y < yAxis - 0.25f)
        {
            yAxis = playerOBJ.transform.position.y; 
        }
        if (playerOBJ.transform.position.y > yAxis + 0.25f)
        {
            yAxis = playerOBJ.transform.position.y;
        }
        this.transform.localPosition = new Vector3(playerOBJ.transform.localPosition.x, yAxis -1f, playerOBJ.transform.localPosition.z);
    }
}
