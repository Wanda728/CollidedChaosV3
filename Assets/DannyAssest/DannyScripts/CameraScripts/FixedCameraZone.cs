using UnityEngine;
using System.Collections.Generic;

public class FixedCameraZone : MonoBehaviour
{
    [Header("Assign the camera for this zone")]
    public Camera zoneCamera;

    // Stack keeps track of entered cameras
    private static Stack<Camera> cameraStack = new Stack<Camera>();

    // The camera currently active
    private static Camera currentCamera;

    // The original starting camera (the one active when the scene loads)
    private static Camera originalCamera;

    private void Start()
    {
        // Register the original camera once at startup
        if (originalCamera == null)
            originalCamera = Camera.main;

        // Make sure this zone camera is off by default
        if (zoneCamera != null)
            zoneCamera.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Character player = other.GetComponent<Character>();
        if (player == null) return;

        // Push current camera (if any)
        if (currentCamera != null)
        {
            cameraStack.Push(currentCamera);
            currentCamera.gameObject.SetActive(false);
        }
        else
        {
            // If there’s no active camera, use the original one
            cameraStack.Push(originalCamera);
            originalCamera.gameObject.SetActive(false);
        }

        // Activate the new camera
        zoneCamera.gameObject.SetActive(true);
        currentCamera = zoneCamera;

        // Update player's camera reference
        player.cameraTransform = zoneCamera.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Character player = other.GetComponent<Character>();
        if (player == null) return;

        // Turn off the zone camera
        zoneCamera.gameObject.SetActive(false);

        // Return to the previous camera if available
        if (cameraStack.Count > 0)
        {
            Camera previous = cameraStack.Pop();
            previous.gameObject.SetActive(true);
            currentCamera = previous;

            // Update player's camera reference
            player.cameraTransform = previous.transform;
        }
        else
        {
            // If no cameras left in stack, revert to original
            originalCamera.gameObject.SetActive(true);
            currentCamera = originalCamera;
            player.cameraTransform = originalCamera.transform;
        }
    }
}
