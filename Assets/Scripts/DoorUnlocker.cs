using UnityEngine;

public class DoorUnlocker : MonoBehaviour
{
    [Header("Required enemy kills to remove door")]
    public int killsNeeded = 5;

    private void Update()
    {
        if (EnemyKillTracker.Instance.totalKills >= killsNeeded)
        {
            Debug.Log("Door unlocked!");
            Destroy(gameObject);   // door disappears
        }
    }
}