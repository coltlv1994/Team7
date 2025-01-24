using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.GraphicsBuffer;

public class CS_RayCastScript : MonoBehaviour //Created by Elliot
{
    CS_PlayerMovement m_playerMovement;

    [SerializeField] TextMeshProUGUI m_pointsText, m_foodText, m_keyText;
    private int m_pointsAmount = 00, m_foodAmount = 00, m_keyAmount = 00;

    public void OnOBJInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CheckToInteract();
        }
        else if (context.canceled)
        {
            m_playerMovement.ReleaseFunction();
        }
    }

    private void Start()
    {
        m_playerMovement = FindAnyObjectByType<CS_PlayerMovement>();
        m_pointsText.SetText(m_pointsAmount.ToString());
        m_foodText.SetText(m_foodAmount.ToString());
        m_keyText.SetText(m_keyAmount.ToString());
    }

    public void CheckToInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, 3f))
        {
               string objectsName = hit.transform.name;
               switch(objectsName)
               {
                    case "PointOBJ":
                        m_pointsAmount++;
                        m_pointsText.SetText(m_pointsAmount.ToString());
                        Destroy(hit.transform.gameObject);
                        break;

                    case "FoodOBJ":
                        m_foodAmount++;
                        m_foodText.SetText(m_foodAmount.ToString());
                        Destroy(hit.transform.gameObject);
                    break;

                    case "KeyOBJ":
                        m_keyAmount++;
                        m_keyText.SetText(m_keyAmount.ToString());
                        Destroy(hit.transform.gameObject);
                    break;

                    default: break;
               }
            

            if (hit.transform.CompareTag("GrabableObject"))
            {
                m_playerMovement.m_pickedUpped = true;
                m_playerMovement.PickUpFunction(hit.transform);
            }
        }
    }
}
