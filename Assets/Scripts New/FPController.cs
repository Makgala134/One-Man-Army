using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
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

    [Header("Pickup Prompt")]
    [SerializeField] private string pickupPrompt = "Press E to pick up";
    private TextMeshProUGUI pickupPromptText;

    [Header("Throw Settings")]
    public float throwForce = 10f;
    public float throwUpwardBoost = 1f;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    private int lastPickupInputFrame = -1;

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

        if (cameraTransform == null)
        {
            Debug.LogError("FPController requires a camera transform.", this);
            enabled = false;
            return;
        }

        EnsureHoldPoint();

        originalMoveSpeed = moveSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CreatePickupPrompt();
    }
    private void Update()
    {
        if (cameraTransform == null) return;

        //Debug.Log("Controller Heigh: " + controller.height);
        HandleMovement();
        HandleLook();
        UpdatePickupPrompt();

        // This fallback keeps E working even if the PlayerInput UnityEvent is not wired. (this was added due to the input system gltiching and not working.
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            TryPickUpOrDrop();

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
        TryPickUpOrDrop();
    }

    private void TryPickUpOrDrop()
    {
       
        if (lastPickupInputFrame == Time.frameCount) return;
        lastPickupInputFrame = Time.frameCount;

        if (heldObject == null)
        {
            if (TryGetPickupInFront(out PickUpObject pickUp))
            {
                pickUp.PickUp(holdPoint);
                heldObject = pickUp;
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

    private void CreatePickupPrompt() //This code is for text display to happen when an object is eligble for pick up.
       
    {
        GameObject canvasObject = new GameObject("Pickup Prompt Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        GameObject textObject = new GameObject("Pickup Prompt");
        textObject.transform.SetParent(canvasObject.transform, false);
        pickupPromptText = textObject.AddComponent<TextMeshProUGUI>();
        pickupPromptText.text = pickupPrompt;
        pickupPromptText.font = TMP_Settings.defaultFontAsset;
        pickupPromptText.fontSize = 28;
        pickupPromptText.alignment = TextAlignmentOptions.Center;
        pickupPromptText.color = Color.white;
        pickupPromptText.textWrappingMode = TextWrappingModes.NoWrap;

        RectTransform rect = pickupPromptText.rectTransform;
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0f, -90f);
        rect.sizeDelta = new Vector2(500f, 60f);

        pickupPromptText.gameObject.SetActive(false); //This is to have the text not appear when not in front of a pick up object.
    }

    private void EnsureHoldPoint()
    {
        //This specific line of code is to keep the 
        if (holdPoint != null) return;

        //the code referring to the point at which the object will be at once the player picks up the item. (eye Level)
        GameObject holdPointObject = new GameObject("Hold Point");
        holdPoint = holdPointObject.transform;
        holdPoint.SetParent(cameraTransform, false);
        holdPoint.localPosition = new Vector3(0f, -0.35f, 2f);
        holdPoint.localRotation = Quaternion.identity;
    }

    private void UpdatePickupPrompt()
    {
        //this line of code activates and deactivates the text that is prompted when the player stands in front of the object.
        if (pickupPromptText == null) return;

        pickupPromptText.gameObject.SetActive(heldObject == null && TryGetPickupInFront(out _));
    }
    
    private bool TryGetPickupInFront(out PickUpObject pickup)
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
                out RaycastHit hit, pickupRange))
        {
            pickup = hit.collider.GetComponentInParent<PickUpObject>();
            if (pickup != null) return true;
        }

        PickUpObject nearestPickup = null;
        float nearestDistance = float.MaxValue;

        foreach (PickUpObject candidate in FindObjectsByType<PickUpObject>(FindObjectsSortMode.None))
        {
            Vector3 toCandidate = candidate.transform.position - cameraTransform.position;
            float distance = toCandidate.magnitude;
            if (distance <= pickupRange && distance > 0.01f &&
                Vector3.Dot(cameraTransform.forward, toCandidate / distance) >= 0.96f)
            {
                pickup = candidate;
                return true;
            }

            if (distance <= pickupRange && distance < nearestDistance)
            {
                nearestPickup = candidate;
                nearestDistance = distance;
            }
        }

        //this code allows for pick up to work on the nearest object. this is because pick up was struggling to render in the previous code.
        pickup = nearestPickup;
        return pickup != null;
    }
}

