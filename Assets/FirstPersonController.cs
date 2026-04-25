using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 20f;

    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 80f;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;

    private Camera playerCamera;
    private CharacterController controller;
    private AudioSource audioSource;

    private Vector3 moveDirection = Vector3.zero;
    private float yVelocity;
    private float rotationX = 0f;

    private bool isRunning;
    private bool wasGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        playerCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -maxLookAngle, maxLookAngle);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (controller.isGrounded)
        {
            if (!wasGrounded)
            {
                PlayLandSound();
            }

            yVelocity = -2f;

            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpForce;
                PlayJumpSound();
            }
        }
        else
        {
            yVelocity -= gravity * Time.deltaTime;
        }

        moveDirection = move * speed;
        moveDirection.y = yVelocity;

        controller.Move(moveDirection * Time.deltaTime);

        wasGrounded = controller.isGrounded;
    }

    void PlayJumpSound()
    {
        if (jumpSound != null)
            audioSource.PlayOneShot(jumpSound);
    }

    void PlayLandSound()
    {
        if (landSound != null)
            audioSource.PlayOneShot(landSound);
    }
}