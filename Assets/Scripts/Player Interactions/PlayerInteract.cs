using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactRange = 2f;
            Collider[] colliderArry = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArry)
                if (collider.TryGetComponent(out NPC npcInteractable))
                    npcInteractable.Interact();
           
        }
    }


}