using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
using static UnityEngine.Rendering.DebugUI;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;
using TMPro;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Objects")]

    public string InteractableName = "";

    public float interactionDistanace = 6f;

    [SerializeField] private bool isInteractable = true;    

    InteractableNameText interactableNameText;
    GameObject interactableNameCanvas;

    public float interactionDistance { get; internal set; }

    public virtual void Start()
    {
        interactableNameCanvas = GameObject.FindGameObjectWithTag("Canvas");
        interactableNameText = interactableNameCanvas.GetComponentInChildren<InteractableNameText>();
    }

    public void TargetOn()//This is the code that will show the text of the object the player can interact with
    {
        interactableNameText.ShowText(this);
        interactableNameText.SetInteractableNamePosition(this);
    }

    public void TargetOff()//Code that hides the text of the object when player isnt close enough to the object.
    {
        interactableNameText.HideText();
    }

    public void Interact()
    {
        if (isInteractable) Interaction();
    }

    protected virtual void Interaction()
    {
        //print("interacted with " + gameObject.name);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }

    private void OnDestroy()
    {
        TargetOff();
    }
   
}

    
