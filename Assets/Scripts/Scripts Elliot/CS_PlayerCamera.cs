using UnityEngine;

public class CS_PlayerCamera : MonoBehaviour //Created by Elliot
{
    public Transform player;
    private float cameraVerticalRotation;
    public float mouseSensitivty = 100f;


    [HideInInspector] public bool lookingAtPlaceable;
    [HideInInspector] public Vector3 rayHitPoint;
    [HideInInspector] public Vector3 rayHitNormal;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable()
    {
        Camera.main.transform.localPosition = new Vector3(0, 0.5f, 0);
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivty * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivty * Time.deltaTime;

        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90, 90);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        player.Rotate(Vector3.up * mouseX);

        Ray ray = new(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward);

        lookingAtPlaceable = Physics.Raycast(ray, out RaycastHit rayHit, 25, 7 | 8);
        rayHitPoint = rayHit.point;
        rayHitNormal = rayHit.normal;
    }
}
