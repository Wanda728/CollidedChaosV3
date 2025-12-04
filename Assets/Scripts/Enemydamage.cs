using UnityEngine;

public class Enemydamage : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Death Settings")]
    public float despawnDelay = 0f;
    public GameObject deathEffect;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Tell the kill tracker an enemy died
        EnemyKillTracker.Instance.RegisterKill();   // <--- ADDED THIS

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, despawnDelay);
    }
}
