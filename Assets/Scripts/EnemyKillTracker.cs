using UnityEngine;

public class EnemyKillTracker : MonoBehaviour
{
    public static EnemyKillTracker Instance;

    public int totalKills = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterKill()
    {
        totalKills++;
        Debug.Log("Enemy killed! Total kills: " + totalKills);
    }
}