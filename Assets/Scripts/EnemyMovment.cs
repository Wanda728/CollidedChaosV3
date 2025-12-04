using UnityEngine;

public class EnemyMovment: MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3.0f;
    public float rotSpeed = 1.0f;

    [Header("Chase Settings")]
    public GameObject player;
    public float chaseRange = 5.0f;
    public float returnRange = 10.0f;
    private bool isChasing = false;

    [Header("Push Back Settings")]
    public float pushForce = 5f;      
    public bool usePhysicsPush = true; 

    private Vector3 originalPosition;
    private Rigidbody enemyRb;

    void Start()
    {
        originalPosition = transform.position;

        enemyRb = GetComponent<Rigidbody>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null) return;

        float playerDistance = Vector3.Distance(transform.position, player.transform.position);

        if (playerDistance < chaseRange)
            isChasing = true;
        else if (playerDistance > returnRange)
            isChasing = false;

        if (isChasing)
            ChasePlayer();
        else
            ReturnToOriginalPosition();
    }

    void ChasePlayer()
    {
        Quaternion lookatPlayer = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookatPlayer, rotSpeed * Time.deltaTime);

        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    void ReturnToOriginalPosition()
    {
        Quaternion lookAtOriginalPosition = Quaternion.LookRotation(originalPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtOriginalPosition, rotSpeed * Time.deltaTime);

        transform.Translate(0, 0, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, originalPosition) < 1.0f)
        {
            transform.position = originalPosition;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Calculate horizontal-only push direction (ignore Y axis)
            Vector3 pushDirection = (transform.position - collision.transform.position).normalized;
            pushDirection.y = 0f;

            if (usePhysicsPush && enemyRb != null)
            {
                // Apply force only along the ground
                enemyRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
            else
            {
                // Simple positional pushback
                transform.position += pushDirection * 0.5f;
            }

            Debug.Log("Enemy was pushed back by player!");
        }
    }
}