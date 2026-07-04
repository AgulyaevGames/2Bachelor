using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float ogSpeed;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float smoothTime = 0.1f;

    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float sidewaysSpeed = 6f;
    [SerializeField] private float backwardSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;
    private float xRotation = 0f;
    private float yRotation = 0f;

    private float moveHorizontal;
    private float moveVertical;
    public bool jumpRequested;
    private bool isGrounded;
    private Vector3 currentVelocity;

    private bool sprinting = false;
    
    [SerializeField] private AK.Wwise.Event jumpSoundEvent;
    [SerializeField] private AK.Wwise.Event landSoundEvent;

    
    private FootstepController footstepController;
    private float ogFootsteps;
    private void Start()
    {
        ogSpeed = forwardSpeed;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        
        footstepController = GetComponent<FootstepController>();

        ogFootsteps = footstepController.footstepInterval;
    }

    private void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        // Buffer the jump request in Update so no press is missed between FixedUpdate ticks
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            jumpRequested = true;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprinting = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
            sprinting = false;
        RotateWithMouse();
    }

    private void FixedUpdate()
    {
        Move();
        HandleJump();
        HandleSprint();
    }

    private void HandleSprint()
    {
        if (!sprinting)
        {
            footstepController.footstepInterval = ogFootsteps;
            forwardSpeed = ogSpeed;
            return;
        }
        footstepController.footstepInterval = ogFootsteps/2f;
        forwardSpeed = ogSpeed + 5f;
    }
    private void Move()
    {
        float vertical = moveVertical >= 0
            ? moveVertical * forwardSpeed
            : moveVertical * backwardSpeed;

        float horizontal = moveHorizontal * sidewaysSpeed;

        Vector3 targetDirection = transform.right * horizontal + transform.forward * vertical;

        Vector3 targetVelocity = targetDirection.magnitude > 0.01f
            ? targetDirection
            : Vector3.zero;

        targetVelocity.y = rb.velocity.y;

        Vector3 smoothedVelocity = Vector3.SmoothDamp(
            rb.velocity,
            targetVelocity,
            ref currentVelocity,
            smoothTime
        );

        rb.velocity = smoothedVelocity;
    }

    private void HandleJump()
    {
        Debug.Log("");
        if (!jumpRequested) return;

        jumpSoundEvent.Post(gameObject);
        
        Debug.Log("JUMP");
        // Zero out any downward velocity first for a consistent jump height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpRequested = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            landSoundEvent.Post(gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = false;
    }

    private void RotateWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}