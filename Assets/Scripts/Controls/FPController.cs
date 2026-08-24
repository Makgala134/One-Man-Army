using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
using static UnityEngine.Rendering.DebugUI;
using System.Collections.Generic;
using System.Collections;


public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f; // Controls the downward force applied to the player.The value is negative because gravity pulls the player down.
    public float jumpHeight = 1.5f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standHeight = 4f;
    public float crouchSpeed = 2.5f;
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
    private Vector3 velocity; // Stores the player's current vertical movement, including gravity.
    private float verticalRotation = 0f;
    // Awake runs once when the GameObject is first loaded.
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        originalMoveSpeed = moveSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
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
       
        verticalRotation = Mathf.Clamp(verticalRotation,-verticalLookLimit, verticalLookLimit);
       

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
            controller.height = crouchHeight;
            moveSpeed = crouchSpeed;
        }
        else if (context.canceled)
        {
            controller.height = standHeight;
            moveSpeed = originalMoveSpeed;
        }
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

public class PickUpObject : MonoBehaviour
{
    private Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void PickUp(Transform holdPoint)
    {
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
    }
    public void Drop()
    {
        rb.useGravity = true;
        transform.SetParent(null);
    }
    public void MoveToHoldPoint(Vector3 targetPosition)
    {
        rb.MovePosition(targetPosition);
    }
    public void Throw(Vector3 impulse)
    {
        transform.SetParent(null);
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(impulse, ForceMode.Impulse);
    }
}