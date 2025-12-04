using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Transform holdPoint;
    public float range = 1.5f;
    public float moveSpeed = 3f;
    public KeyCode key = KeyCode.E;

    GameObject held;
    Rigidbody heldRb;
    Collider heldCol;

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (held == null) TryPickup();
            else Drop();
        }

        if (held != null)
        {
            Vector3 move = Vector3.zero;

            // F = forward  |  B = backward
            if (Input.GetKey(KeyCode.F)) move += holdPoint.forward;
            if (Input.GetKey(KeyCode.B)) move -= holdPoint.forward;

            if (move == Vector3.zero)
            {
                // No movement → keep object at the hold point
                held.transform.position = holdPoint.position;
            }
            else
            {
                // Apply movement
                held.transform.position += move * moveSpeed * Time.deltaTime;
            }
        }
    }

    void TryPickup()
    {
        foreach (var hit in Physics.OverlapSphere(transform.position, range))
        {
            if (!hit.CompareTag("Blocks")) continue;

            held = hit.gameObject;
            heldRb = held.GetComponent<Rigidbody>();
            heldCol = held.GetComponent<Collider>();

            if (heldRb) heldRb.isKinematic = true;
            if (heldCol) heldCol.enabled = false;

            held.transform.parent = holdPoint;
            held.transform.position = holdPoint.position;
            return;
        }
    }

    void Drop()
    {
        if (heldRb) heldRb.isKinematic = false;
        if (heldCol) heldCol.enabled = true;

        held.transform.parent = null;

        held = null;
        heldRb = null;
        heldCol = null;
    }
}
