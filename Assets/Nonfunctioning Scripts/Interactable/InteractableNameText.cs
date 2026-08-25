using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
using static UnityEngine.Rendering.DebugUI;
using System.Collections.Generic;
using System.Collections;
using System;
using TMPro;
using static UnityEditor.Progress;
//public class InteractableNameText : MonoBehaviour
//{
//    TextMeshProUGUI text;

//    Transform cameraTransform;

 //     void Start()
//    {
//        text =GetComponent<TextMeshProUGUI>();
//        HideText();
//     public void ShowText(Interactable interactable) //what will be displayed for each interactable item
//    {
 //       if (interactable is PickUpItem)
 //       {
 //           text.text = "Press E to pick up " + interactable.InteractableName;
 //       }
//        else if (interactable is Door)
//        {
//            text.text = "Press E to open " + interactable.InteractableName;
//        }
//        else if (interactable is InvestigateItem)
//        {
//            text.text = "Press E to Investigate " + interactable.InteractableName;
 //       }
       
//    }
//    {
//        text.text = "";
//    }

 //   public void SetInteractableNamePosition(Interactable interactable)
 //   {
 //       if (interactable.TryGetComponent(out BoxCollider boxCollider))
//        {
//            transform.position = interactable.transform.position + Vector3.up * boxCollider.bounds.size.y;
//            transform.LookAt(2 * transform.position - cameraTransform.position);
//        }
//        else if (interactable.TryGetComponent(out CapsuleCollider capsuleCollider))
//        {
  //          transform.position = interactable.transform.position + Vector3.up * capsuleCollider.height;
  //          transform.LookAt(2 * transform.position - cameraTransform.position);
  //      }
 //       else
 //       {
 //           print("Error, no collider found on interactable object: " + interactable.name);
 //       }

        //Vector3 screenPosition = Camera.main.WorldToScreenPoint(interactable.transform.position);
        //text.transform.position = screenPosition;
   // }
   