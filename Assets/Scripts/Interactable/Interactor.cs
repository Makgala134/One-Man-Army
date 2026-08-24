using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
using static UnityEngine.Rendering.DebugUI;
using System.Collections.Generic;
using System.Collections;
using System;
public class Interactor : MonoBehaviour
{
    [SerializeField]
    public float maxInteractionDistance = 6f;
    public float InteractionRadius = 1f;

    LayerMask layerMask;
    Transform cameraTransform;
    InputAction InteractAction;

    Vector3 origin;
    Vector3 direction;
    Vector3 hitPosition;
    float hitDistance;

    [HideInInspector]
    public Interactable InteractableTarget;
    private float interactingRadius; //the radius at which the spherecast is
    
    private Interactable interactableTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraTransform = Camera.main.transform;
        layerMask = LayerMask.GetMask("Interactable");

        InteractAction = GetComponent<PlayerInput>().actions["Interact"];
        InteractAction.performed += Interact; //calling the interact button
    }

    // Update is called once per frame
    void Update()
    {
        direction = cameraTransform.forward;
        origin = cameraTransform.position;
        RaycastHit hit;

        if(Physics.SphereCast(origin, interactingRadius, direction, out hit, maxInteractionDistance, layerMask))
        {
            hitPosition = hit.point;
            hitDistance = hit.distance;

            if(hit.transform.TryGetComponent<Interactable>(out interactableTarget)) //does the object the player is at have the interactable function
            {
                interactableTarget.TargetOn(); //this will show us what we can interact with
            }
        }
        else if (interactableTarget)
        {
            interactableTarget.TargetOff();
            interactableTarget = null;
        }
    }
     private void Interact(InputAction.CallbackContext obj)
    {
        if (interactableTarget != null)
        {
            if(Vector3.Distance(transform.position, interactableTarget.transform.position) <= interactableTarget.interactionDistance) //if the object is close enough to the player the player will be able to interact with the object.
            {
                interactableTarget.Interact();
            }
        }
     }
    private void OnDestroy()
    {
        InteractAction.performed -= Interact;
    }
}
