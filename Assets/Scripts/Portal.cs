using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Portal : MonoBehaviour
{
    [Header("Scene Teleport Settings")]
    [Tooltip("Exact name of the scene to load (must be added to Build Settings).")]
    public string targetSceneName;

    [Header("Teleport Location (in target scene)")]
    [Tooltip("Exact coordinates to place the player after loading the scene.")]
    public Vector3 teleportLocation = new Vector3(0f, 0f, 0f);

    [Tooltip("Tag that identifies the player object.")]
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("🌀 Player entered the portal! Loading scene: " + targetSceneName);
            StartCoroutine(LoadSceneAndTeleport(other.gameObject));
        }
    }

    private IEnumerator LoadSceneAndTeleport(GameObject player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();

        // Optional: temporarily disable player velocity for clean teleport
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Keep player during scene load
        DontDestroyOnLoad(player);

        // Load the new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
        while (!asyncLoad.isDone)
            yield return null;

        // Wait a frame for scene initialization
        yield return null;

        // --- Teleport player to the chosen location ---
        player.transform.position = teleportLocation;
        Debug.Log($"✅ Player teleported to {teleportLocation} in scene {targetSceneName}");

        // Reset Rigidbody
        if (rb != null)
        {
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
        }

        Debug.Log("🌍 Teleport complete!");
    }
}