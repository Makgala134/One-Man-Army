using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractableDoor : Interactable
{
    private Animator animator;
    [Header("Door Properties")]
    public bool isClosed;
    public string chestID;
    public bool isOpen;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        isOpen = false;
    }

    protected override void Interaction()
    {
        base.Interaction();

        if (!isClosed)
        {
            if (!isOpen)
            {
                OpenDoor();
                print("Opening the door");
            }
            else
            {
                CloseDoor();
                print("Closing the door");
            }
        }
    }

    void CloseDoor()
    {
        animator.SetTrigger("CloseDoor");
        isOpen = !isOpen;
    }

    void OpenDoor()
    {
        animator.SetTrigger("OpenDoor");
        isOpen = !isOpen;
    }
}
    

