using UnityEngine;
using System.Collections;
public class Sword : MonoBehaviour
{
    [Header("Attack Settings")]
    public float damage = 25f;
    public KeyCode attackKey = KeyCode.Mouse0;   
    public float attackCooldown = 0.5f;

    [Header("References")]
    public Animator playerAnimator;              
    public Collider swordCollider;             

    private bool canAttack = true;
    private bool isAttacking = false;

    void Start()
    {
        
        if (playerAnimator == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                playerAnimator = player.GetComponent<Animator>();
        }

        if (swordCollider == null)
            swordCollider = GetComponent<Collider>();

        if (swordCollider != null)
            swordCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        isAttacking = true;

        if (playerAnimator != null)
        {
            int randomAttack = Random.Range(1, 4); // Pick 1, 2, or 3
            string triggerName = "Slash" + randomAttack;
            playerAnimator.SetTrigger(triggerName);
        }

        if (swordCollider != null)
            swordCollider.enabled = true;

        yield return new WaitForSeconds(0.2f);

        if (swordCollider != null)
            swordCollider.enabled = false;

        isAttacking = false;

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            Enemydamage enemy = other.GetComponent<Enemydamage>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Enemy hit for " + damage + " damage!");
            }
        }
    }
}