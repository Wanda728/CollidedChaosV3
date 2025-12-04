using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    bool canDealDamage; 
    bool hasDealtDamage; 

    [SerializeField] float weaponLength;
    [SerializeField] float weaponDamage; 

    void Start()
    {
        canDealDamage = false; 
        hasDealtDamage = false; 
    }

    void Update()
    {
        if(canDealDamage && !hasDealtDamage)
        {
            RaycastHit hit; 

            int layerMask = 1 << 8; 
            if (Physics.Raycast(transform.position, - transform.up, out hit, weaponLength, layerMask))
            {
                if (hit.transform.TryGetComponent(out HealthSystem health))
                {
                    health.TakeDamage(weaponDamage); 
                    hasDealtDamage = true;
                }
            }
        }
    }

    public void StartDealDamage()
    {
        canDealDamage = true; 
        hasDealtDamage = false; 
    }
    public void EndDealDamage()
    {
        canDealDamage = false; 
    }
    private void OawGizmos()
    {
        Gizmos.color = Color.blue; 
        Gizmos.DrawLine(transform.position, transform.position - transform.up *weaponLength);
    }
}
