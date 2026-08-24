using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
using static UnityEngine.Rendering.DebugUI;
using System.Collections.Generic;
using System.Collections;
using System;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Objects")]
    public string InteractableName = "";
    public float interactionDistanace = 6f;

    InteractableNameText interactableNameText;
    GameObject interactableNameCanvas;

    [SerializeField]
    bool isInteractable = true;
    internal float interactionDistance;

    public void Interact()
    {
        if (isInteractable) Interaction();
    }

    protected virtual void Interaction()
    {

    }
    
    public virtual void Start()
    {
        interactableNameCanvas = GameObject.FindGameObjectWithTag("Canvas");
        interactableNameText = interactableNameCanvas.GetComponent<InteractableNameText>();
    }
    public void TargetOn() //This is the code that will show the text of the object the player can interact with
    {
        interactableNameText.ShowText(this);
        interactableNameText.SetInteractableNamePosition(this);
    }

    public void TargetOff() //Code that hides the text of the object when player isnt close enough to the object.
    {
        interactableNameText.HideText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

