using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
using static UnityEngine.Rendering.DebugUI;
using System.Collections.Generic;
using System.Collections;
using System;
public class PickUpObject : MonoBehaviour
{
    private Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void PickUp(Transform holdPoint)
    {
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        SetCollidersEnabled(false);
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
    }
    public void Drop()
    {
        transform.SetParent(null);
        if (rb != null)
            rb.useGravity = true;

        SetCollidersEnabled(true);
    }
    public void MoveToHoldPoint(Vector3 targetPosition)
    {
        if (rb != null)
            rb.MovePosition(targetPosition);
        else
            transform.position = targetPosition;
    }
    public void Throw(Vector3 impulse)
    {
        transform.SetParent(null);
        SetCollidersEnabled(true);

        if (rb == null)
            return;

        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(impulse, ForceMode.Impulse);
    }

    private void SetCollidersEnabled(bool isEnabled)
    {
        foreach (Collider itemCollider in GetComponents<Collider>())
            itemCollider.enabled = isEnabled;
    }
}


