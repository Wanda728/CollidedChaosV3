using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Transform cameraTransform;        
    public Transform hookModel;              
    public Transform hookOrigin;             
    public LayerMask grappleLayer;
    public LineRenderer rope;

    [Header("Settings")]
    public float maxDistance = 40f;
    public float hookSpeed = 20f;
    public float pullSpeed = 20f;
    public float stopDistance = 2f;
    public float objectPullSpeed = 12f;     

    private Vector3 hookPoint;
    private bool isShooting;
    private bool isPullingPlayer;
    private bool isPullingObject;           

    private Rigidbody hookedObjectRB;       
    private PlayerInput input;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        hookModel.gameObject.SetActive(false);
        rope.enabled = false;

        rope.positionCount = 2;
        rope.useWorldSpace = true;
    }

    private void Update()
    {
        if (input.actions["Hookshot"].triggered && !isShooting && !isPullingPlayer && !isPullingObject)
        {
            TryShootHook();
        }

        if (isShooting)
            UpdateHookShotTravel();

        if (isPullingPlayer)
            UpdatePlayerPull();

        if (isPullingObject)
            UpdateObjectPull();   

        UpdateRope();
    }

    private void TryShootHook()
    {
        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, maxDistance, grappleLayer))
        {
            hookPoint = hit.point;

            // checks rigid body
            hookedObjectRB = hit.collider.GetComponent<Rigidbody>();
            isPullingObject = hookedObjectRB != null;

            // If no rigidbody, fall back to pulling player
            if (!isPullingObject)
                isPullingPlayer = true;

            isShooting = true;

            hookModel.gameObject.SetActive(true);
            hookModel.position = hookOrigin.position;
            hookModel.LookAt(hookPoint);

            rope.enabled = true;
        }
    }

    private void UpdateHookShotTravel()
    {
        hookModel.position = Vector3.MoveTowards(
            hookModel.position,
            hookPoint,
            hookSpeed * Time.deltaTime
        );

        if (Vector3.Distance(hookModel.position, hookPoint) < 0.3f)
        {
            isShooting = false;
        }
    }

    private void UpdatePlayerPull()
    {
        Vector3 direction = (hookPoint - transform.position).normalized;
        controller.Move(direction * pullSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, hookPoint) < stopDistance)
            StopGrapple();
    }

    private void UpdateObjectPull()   
    {
        if (hookedObjectRB == null)
        {
            StopGrapple();
            return;
        }

        // direction from object to player
        Vector3 direction = (hookOrigin.position - hookedObjectRB.position).normalized;

        hookedObjectRB.MovePosition(
            hookedObjectRB.position + direction * objectPullSpeed * Time.deltaTime
        );

        // object reached player
        if (Vector3.Distance(hookedObjectRB.position, hookOrigin.position) < 1f)
        {
            StopGrapple();
        }
    }

    private void StopGrapple()
    {
        isPullingPlayer = false;
        isPullingObject = false;

        hookedObjectRB = null;

        hookModel.gameObject.SetActive(false);
        hookModel.position = hookOrigin.position;

        rope.enabled = false;
    }

    private void UpdateRope()
    {
        if (!rope.enabled) return;

        Vector3 offset = new Vector3(0, -0.4f, 0);

        rope.SetPosition(0, hookOrigin.position + offset);
        rope.SetPosition(1, hookModel.position);
    }
}

