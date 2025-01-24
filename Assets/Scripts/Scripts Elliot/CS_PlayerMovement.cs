using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class CS_PlayerMovement : MonoBehaviour //Created by Elliot
{

    CS_RayCastScript m_rayCastScript;
    private Rigidbody m_rb;
    public float m_moveSpeed;
    bool m_isGrounded;
    public float m_jumpHeight;
    bool m_crouching;
    bool m_sprinting;
    public float m_sprintSpeed;
    public float m_crouchSpeed;
    public float m_walkingSpeed;
    [SerializeField] float m_worldBottomBoundry;

    public Transform m_respawnTransform;
    public Transform m_pickUpTransform;

    GameObject m_pickedUpObject;
    public bool m_pickedUpped;
    public float m_knockbackSpeed;


    public bool m_enableMovement = true;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (m_isGrounded)
        {
            m_rb.AddForce(transform.up * m_jumpHeight, ForceMode.Impulse);
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                m_crouching = true;
                break;
            case InputActionPhase.Canceled:
                m_crouching = false;
                break;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                m_sprinting = true;
                break;
            case InputActionPhase.Canceled:
                m_sprinting = false;
                break;
        }
    }


    [SerializeField] private Vector2 m_moveInput;
    Vector3 _moveDirection;
    public void OnMove(InputAction.CallbackContext context)
    {
        m_moveInput = context.ReadValue<Vector2>();
        _moveDirection = new Vector3(m_moveInput.x, 0, m_moveInput.y);
    }

    void Start()
    {
        m_rayCastScript = FindAnyObjectByType<CS_RayCastScript>();
        m_rb = GetComponent<Rigidbody>();
        m_rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(transform.position, fwd, 1.1f)) { m_isGrounded = true; }
        else { m_isGrounded = false; }

        m_moveSpeed = Mathf.Clamp(m_moveSpeed, 0f, m_sprintSpeed);

        if (m_enableMovement)
        {
            Movement();
            Crouching();
            Sprinting();
            CheckBounds();
        }

        //Needs work, but below works, just janky
        if (m_pickedUpped) { (m_pickedUpObject.transform.position, m_pickedUpObject.gameObject.transform.rotation) = (m_pickUpTransform.transform.position, m_pickUpTransform.transform.rotation); }
    }

    private void Movement()
    {
        if (m_rb.linearVelocity.magnitude > m_moveSpeed)
        {
            m_rb.linearVelocity = Vector3.ClampMagnitude(m_rb.linearVelocity, m_moveSpeed);
        }
        else if (m_rb.linearVelocity.magnitude < 0)
        {
            m_rb.linearVelocity = Vector3.ClampMagnitude(m_rb.linearVelocity, 0);
        }

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 CameraRelativeMovement = (_moveDirection.z * forward + _moveDirection.x * right);

        if (m_moveInput.magnitude == 0)
        {
            m_rb.linearVelocity -= new Vector3((CameraRelativeMovement.x * m_moveSpeed), 0, (CameraRelativeMovement.z * m_moveSpeed));
        }
        else
        {
            m_rb.linearVelocity += new Vector3((CameraRelativeMovement.x * m_moveSpeed), m_rb.linearVelocity.y, (CameraRelativeMovement.z * m_moveSpeed));
        }
    }
    private void Crouching()
    {
        if (m_crouching && !m_sprinting)
        {
            transform.localScale = new Vector3(1, 0.5f, 1);
            m_moveSpeed = m_crouchSpeed;
        }
        else
        {
            m_moveSpeed = m_walkingSpeed;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void Sprinting()
    {
        if (m_sprinting && !m_crouching)
        {
            m_moveSpeed = m_sprintSpeed;
        }
    }

    private void CheckBounds()
    {
        if (transform.position.y < m_worldBottomBoundry)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = m_respawnTransform.position;
    }

    public void PickUpFunction(Transform targ)
    {
        var rb = targ.GetComponent<Rigidbody>();
        m_pickedUpObject = targ.gameObject;
        if (rb != null) rb.useGravity = false; rb.freezeRotation = true;
        //targ.parent = Camera.main.transform;
        //targ.parent = transform;               
    }

    public void ReleaseFunction()
    {
        m_pickedUpped = false;
        if (m_pickedUpObject == null) { return; }
        else
        {
            var rb = m_pickedUpObject.GetComponent<Rigidbody>();
            if (rb != null) rb.useGravity = true; rb.freezeRotation = false;
            //pickedUpObject.transform.parent = null;
            (m_pickedUpObject.transform.position, m_pickedUpObject.transform.rotation) = (m_pickedUpObject.transform.position, m_pickedUpObject.transform.rotation);
            m_pickedUpObject = null;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "InstaDeath")
        {
            Respawn();
        }
    }

    public void KnockBack()
    {
        m_rb.AddForce(transform.forward * m_knockbackSpeed, ForceMode.Impulse);
    }
}
