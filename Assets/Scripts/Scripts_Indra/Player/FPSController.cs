//Indra
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
//using static Unity.Cinemachine.InputAxisControllerBase<T>;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{

    [Header("Movement")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float crouchSpeed = 3f;
    float speed = 6f;

    [Header("Jumping")]
    public float jumpPower = 7f;
    public float gravity = 10f;

    [Header("Crouching")]
    public GameObject crouchingMoveObj;
    public float defaultPlayerHeight = 1;
    public float crouchingHeight = .5f;
    public float crouchingSpeed = 1;

    [Header("Dashing and Knockback")]
    //added bt Elliot
    public float m_worldBottomBoundry = -100;
    public Vector3 m_respawnLocation;
    public float m_knockbackForce;
    public float m_dashDistance;
    float m_dashTimer;
    bool dashing;
    Rigidbody m_rb;
    CapsuleCollider capsuleCollider;

    [Header("Camera")]
    public Camera playerCamera;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;


    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    float crouchLerpTime;
    float heightOffset;


    CharacterController characterController;

    #region Handles Pause
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged; 
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    private void OnGameStateChanged(GameState newState)
    {
        enabled = newState == GameState.Play;
    }
    #endregion
    void Start()
    {
        m_respawnLocation = transform.position;
        capsuleCollider = GetComponent<CapsuleCollider>();
        m_rb = GetComponent<Rigidbody>();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        //crouchingMoveObj.transform.localPosition = new Vector3(crouchingMoveObj.transform.localPosition.x, defaultPlayerHeight, crouchingMoveObj.transform.localPosition.z);
    }

    public void KnockBack()
    {
        if(m_rb != null)
        {
            print("KnocingBack");
            m_rb.AddForce(transform.forward * m_knockbackForce, ForceMode.Impulse);
        }
    }
    private void CheckBounds()
    {
        if (transform.position.y < m_worldBottomBoundry)
        {
            characterController.enabled = false;
            transform.position = m_respawnLocation;
            characterController.enabled = true;
        }
    }

    void Update()
    {
        CheckBounds();
        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (crouchLerpTime <= 0 ? (isRunning ? runSpeed : walkSpeed) : crouchSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (crouchLerpTime <= 0 ? (isRunning ? runSpeed : walkSpeed) : crouchSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        #endregion

        #region Handles Crouch

        void ChangeCrouchingHeight()
        {
            crouchLerpTime = Mathf.Clamp01(crouchLerpTime);
            float currentPlayerHeight = Mathf.Lerp(defaultPlayerHeight, crouchingHeight, crouchLerpTime);
            crouchingMoveObj.transform.localPosition = new Vector3(crouchingMoveObj.transform.localPosition.x, currentPlayerHeight, crouchingMoveObj.transform.localPosition.z);
        }

        //if (Input.GetKey(KeyCode.LeftControl))
        //{
        //    crouchLerpTime += Time.deltaTime * crouchingSpeed;
        //    ChangeCrouchingHeight();
        //    speed = crouchSpeed;
        //}
        //else if (crouchLerpTime > 0)
        //{
        //    crouchLerpTime -= Time.deltaTime * crouchingSpeed;
        //    ChangeCrouchingHeight();
        //    speed = walkSpeed;
        //}

        #endregion

        #region Handles Dashing

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            dashing = true;
        }

        if(dashing)
        {
            capsuleCollider.enabled = false;
            m_dashTimer += Time.deltaTime;
            if (m_dashTimer >= 0.5) { dashing = false; m_dashTimer = 0; capsuleCollider.enabled = true; }
            Vector3 dashDirection = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
            characterController.Move(dashDirection * m_dashDistance);
        }
        #endregion
    }
}

