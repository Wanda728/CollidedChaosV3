using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour
{
    [Header("Respawn Settings")]
    public Transform spawnPoint;      
    public float respawnDelay = 3f;   

    private bool isRespawning = false;

    public void OnPlayerDeath(PlayerHealth player)
    {
        if (!isRespawning)
        {
            StartCoroutine(RespawnPlayer(player));
        }
    }

    private IEnumerator RespawnPlayer(PlayerHealth player)
    {
        isRespawning = true;

        Debug.Log("Player died! Respawning in " + respawnDelay + " seconds...");
        yield return new WaitForSeconds(respawnDelay);

        if (player != null && spawnPoint != null)
        {
            player.Respawn(spawnPoint.position);
        }
        else
        {
            Debug.LogError("RespawnManager: Missing player reference or spawn point!");
        }

        isRespawning = false;
    }
}
