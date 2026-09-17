using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using JetBrains.Annotations;


public class InteractableNameText : MonoBehaviour
{
    TextMeshProUGUI text;

    Transform cameraTransform;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        cameraTransform = Camera.main.transform;
        HideText();
    }

public void ShowText(Interactable interactable)
{
    if (interactable is PickUpItem)
    {
        text.text = interactable.InteractableName + " (Press E to pick up)"; 
    }
    else if (interactable is InteractableFile)
    {
        if (((InteractableFile)interactable).isClosed)
        {
            text.text = interactable.InteractableName + " (Press E to open)";
        }
        else
        {
            text.text = interactable.InteractableName + " (Press E to read)";
        }
    }
    if (interactable is InteractableDoor)
        {
            if (((InteractableDoor)interactable).isOpen)
            {
                text.text = interactable.InteractableName + " (Press E to close)";
            }
            else
            {
                text.text = interactable.InteractableName + " (Press E to open)";
            }
        }
        else
        {
            text.text = interactable.InteractableName + " (Press E to interact)";
        }
        if (interactable is InteractableBox)
        {
            if (((InteractableBox)interactable).isClosed)
            {
                text.text = interactable.InteractableName + " (Press E to open)";
            }
            else
            {
                text.text = interactable.InteractableName + " (Press E to close)";
            }
        }

    }        
    public void HideText()
    {
        text.text = "";
    }

    public void SetInteractableNmaePosition(Interactable interactable)
    {
        if(interactable.TryGetComponent(out BoxCollider boxCollider))
        {
            transform.position = interactable.transform.position + Vector3.up * boxCollider.bounds.size.y;
            transform.LookAt(2 * transform.position - cameraTransform.position);
        }
        else if (interactable.TryGetComponent(out CapsuleCollider CapsCollider))
        {
            transform.position = interactable.transform.position + Vector3.up * CapsCollider.height;
            transform.LookAt(2 * transform.position - cameraTransform.position);
        }
        else
        {
            print("Error, no collider found!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
