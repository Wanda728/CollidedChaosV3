using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    [Header("Weapons")]
    public GameObject sword;             
    public GameObject grapplingHook;     

    void Start()
    {
       
        EquipSword();
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipSword();
        }

        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipGrapplingHook();
        }
    }

    void EquipSword()
    {
        sword.SetActive(true);
        grapplingHook.SetActive(false);
    }

    void EquipGrapplingHook()
    {
        sword.SetActive(false);
        grapplingHook.SetActive(true);
    }
}