using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Damage Settings")]
    public float touchDamage = 10f;          
    public float damageCooldown = 1.0f;      

    [Header("Regeneration Settings")]
    public bool autoRegen = false;
    public float regenRate = 2f;
    public float regenDelay = 3f;

    [Header("Death Settings")]
    public float respawnDelay = 3f;          

    private bool isDead = false;
    private float lastDamageTime;
    private Collider playerCollider;
    private Rigidbody playerRb;

    void Start()
    {
        currentHealth = maxHealth;
        playerCollider = GetComponent<Collider>();
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (autoRegen && !isDead && Time.time - lastDamageTime > regenDelay)
        {
            RegenerateHealth();
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        lastDamageTime = Time.time;

        Debug.Log($"Player took {damage} damage! Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"Player healed {amount}. Current Health: {currentHealth}");
    }

    private void RegenerateHealth()
    {
        currentHealth = Mathf.Min(currentHealth + regenRate * Time.deltaTime, maxHealth);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("💀 Player has died!");

        
        if (playerCollider != null) playerCollider.enabled = false;
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.isKinematic = true;
        }

        

        
        RespawnManager respawn = FindObjectOfType<RespawnManager>();
        if (respawn != null)
        {
            respawn.OnPlayerDeath(this); 
        }
    }

    
    public void Respawn(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
        currentHealth = maxHealth;
        isDead = false;

        if (playerCollider != null) playerCollider.enabled = true;
        if (playerRb != null)
        {
            playerRb.isKinematic = false;
            playerRb.linearVelocity = Vector3.zero;
        }

        

        Debug.Log("✅ Player respawned at spawn point!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && Time.time - lastDamageTime >= damageCooldown)
        {
            TakeDamage(touchDamage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && Time.time - lastDamageTime >= damageCooldown)
        {
            TakeDamage(touchDamage);
        }
    }
}