using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Player moves in relation to the camera's rotation.
// Uses Unity's preview input system.
public class PlayerMovement : MonoBehaviour
{
    public static Vector3 playerPos;

    [SerializeField]
    private float playerSpeed = 10f;
    [SerializeField]
    private float x;
    [SerializeField]
    private float z;
    [SerializeField]
    private float distanceToGround = 1.2f;
    [SerializeField]
    private float jumpHeight = 750f;
    [SerializeField]
    private float rollDistance = 500f;

    [SerializeField]
    private Vector3 move;

    private Rigidbody rb;

    private PlayerControls controls;

    [SerializeField]
    private GameObject mainCamera;

    [SerializeField]
    // Pivot is an empty gameobject that shares the position of the
    // camera, but always faces forward. It's rotation is not
    // changed.
    private GameObject pivot;

    [SerializeField]
    private bool isGrounded;
    private bool isJumping;
    private bool isCrouching;
    private bool isRunning;

    [SerializeField]
    private LayerMask groundLayer;

    void Awake()
    {
        controls = new PlayerControls();
        rb = gameObject.GetComponent<Rigidbody>();

        // Controller input.
        controls.Gameplay.PlayerMoveX.performed += ctx => x 
        = ctx.ReadValue<float>();
        controls.Gameplay.PlayerMoveX.canceled += ctx => x = 0f;

        controls.Gameplay.PlayerMoveZ.performed += ctx => z 
        = ctx.ReadValue<float>();
        controls.Gameplay.PlayerMoveZ.canceled += ctx => z = 0f;

        controls.Gameplay.PlayerJump.performed += ctx => Jump();
        controls.Gameplay.PlayerCrouch.performed += ctx => Crouch();
        controls.Gameplay.PlayerRun.performed += ctx => Run();
    }

    void Update()
    {
        playerPos = transform.position;

        pivot.transform.position = mainCamera.transform.position;

        var euler = mainCamera.transform.rotation.eulerAngles;
        var rot = Quaternion.Euler(0, euler.y, 0);
        pivot.transform.rotation = rot;

        // Player moves on the Z axis based on the camera's rotation.
        move = mainCamera.transform.right * x + pivot.transform.forward * z;

        // If there's not input from the input system, 
        // check for alternative input.
        // This is temporary, for Zack.
        if (move == new Vector3(0f, 0f, 0f)) 
        {
            move = KeyboardMovement(move);
        }

        if (Physics.Raycast(transform.position, Vector3.down, 
            distanceToGround, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        Vector3 relativePos = move - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);

        transform.rotation = rotation;
    }

    // Physics.
    void FixedUpdate()
    {
        // Move the player.
        rb.position += (move * playerSpeed * Time.deltaTime);

        // Execute jump.
        if (isJumping && isGrounded)
        {
            rb.AddForce(0f, jumpHeight, 0f, ForceMode.Impulse);
            isJumping = false;
        }

        // Execute crouch.
        if (isCrouching)
        {
            transform.localScale = new Vector3(1f, 0.5f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Jump()
    {
        isJumping = true;
    }

    void Crouch()
    {
        if (isCrouching)
        {
            isCrouching = false;

            playerSpeed = 7.5f;
            jumpHeight = 750f;
        }
        else
        {
            isCrouching = true;

            playerSpeed = 3.33f;
            jumpHeight = 700f;
        }
    }

    void Run() 
    {
        if (isRunning)
        {
            isRunning = false;

            playerSpeed = 7.5f;
        }
        else
        {
            isRunning = true;

            playerSpeed = 15f;
        }
    }

    // Temporary alternative movement.
    Vector3 KeyboardMovement(Vector3 move) 
    {
        float keyboardX = Input.GetAxis("Horizontal");
        float keyboardZ = Input.GetAxis("Vertical");

        move = mainCamera.transform.right * keyboardX + pivot.transform.forward 
            * keyboardZ;

        return move;
    }
}