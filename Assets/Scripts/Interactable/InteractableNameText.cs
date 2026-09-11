using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
using static UnityEngine.Rendering.DebugUI;
using System.Collections.Generic;
using System.Collections;
using System;
using TMPro;

public class InteractableNameText : MonoBehaviour
{
    TextMeshProUGUI text;

    Transform cameraTransform;

      void Start()
    {
        text =GetComponent<TextMeshProUGUI>();
        cameraTransform = Camera.main.transform;
        HideText();
    }
     public void ShowText(Interactable interactable) //what will be displayed for each interactable item
    {
        if (interactable is PickUpItem)
        {
            text.text = "Press E to pick up " + interactable.InteractableName;
        }
        //else if (interactable is Door)
       // {
        //    text.text = "Press E to open " + interactable.InteractableName;
       // }
       //else if (interactable is InvestigateItem)
       // {
       //     text.text = "Press E to Investigate " + interactable.InteractableName;
       // }
        else if (interactable is NPC)
        {
            text.text = "Press E to talk to " + interactable.InteractableName;
        }
        
    }
    public void HideText() //to keep the text hidden when not in front of an object
    {
        text.text = "";
    }

    public void SetInteractableNamePosition(Interactable interactable)
    {
        if (interactable.TryGetComponent(out BoxCollider boxCollider))
        {
            transform.position = interactable.transform.position + Vector3.up * boxCollider.bounds.size.y;
            transform.LookAt(2 * transform.position - cameraTransform.position);
        }
        else if (interactable.TryGetComponent(out CapsuleCollider capsuleCollider))
        {
            transform.position = interactable.transform.position + Vector3.up * capsuleCollider.height;
            transform.LookAt(2 * transform.position - cameraTransform.position);
        }
        else
        {
            print("Error, no collider found on interactable object: " + interactable.name);
        }

        //Vector3 screenPosition = Camera.main.WorldToScreenPoint(interactable.transform.position);
        //text.transform.position = screenPosition;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
