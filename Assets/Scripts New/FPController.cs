using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
using static UnityEngine.Rendering.DebugUI;
using System.Collections.Generic;
using System.Collections;


public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public Camera cam;
    public float lookSensitivity = 0.1f;
    public float verticalLookLimit = 90f;

    [Header("Crouch Settings")]
    public float crouchHeight = 0.5f;
    public float standHeight = 1f;
    public float crouchSpeed = 4f;
    private float originalMoveSpeed;

    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public Transform holdPoint;
    private PickUpObject heldObject;

    [Header("Throw Settings")]
    public float throwForce = 10f;
    public float throwUpwardBoost = 1f;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("FPController requires a CharacterController component.", this);
            enabled = false;
            return;
        }

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        originalMoveSpeed = moveSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (cameraTransform == null) return;

        //Debug.Log("Controller Heigh: " + controller.height);
        HandleMovement();
        HandleLook();
        if (heldObject != null)
        {
            heldObject.MoveToHoldPoint(holdPoint.position);
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {

        moveInput = context.ReadValue<Vector2>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        OnMove(context);
    }

    public void OnLook(InputAction.CallbackContext context)
    {

        lookInput = context.ReadValue<Vector2>();
    }

    public void HandleMovement()
    {

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
    public void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;

        float mouseY = lookInput.y * lookSensitivity;

        verticalRotation -= mouseY;

        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);


        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //float temp = crouchHeight - controller.height;
            controller.height = crouchHeight;
            moveSpeed = crouchSpeed;
            //controller.center += new Vector3(0f, temp / 2f, 0f);
        }
        else if (context.canceled)
        {
            //float temp = standHeight - controller.height;
            controller.height = standHeight;
            moveSpeed = originalMoveSpeed;
            //controller.center += new Vector3(0f, temp / 2f, 0f);
        }
    }


    private void SetHeight(float newHeight) //code fixing the issue of the player falling through the ground
    {
        //capsule was falling through the floor due to the standing up height that would make it come back up underneath the plane.
        //The center of the capsule would cause the player to fall through the plane, this code was suppoused to fix that however it failed to do so

        float heightDifference = newHeight - controller.height;
        controller.height = newHeight;
        controller.center += new Vector3(0f, heightDifference / 2f, 0f);
    }
    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (heldObject == null)
        {
            Ray ray = new Ray(cameraTransform.position,
            cameraTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                PickUpObject pickUp =
                hit.collider.GetComponent<PickUpObject>();
                if (pickUp != null)
                {
                    pickUp.PickUp(holdPoint);
                    heldObject = pickUp;
                }
            }
        }
        else
        {
            heldObject.Drop();
            heldObject = null;
        }
    }
    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (heldObject == null) return;
        Vector3 dir = cameraTransform.forward;
        Vector3 impulse = dir * throwForce + Vector3.up *
        throwUpwardBoost;
        heldObject.Throw(impulse);
        heldObject = null;
    }
}

