using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public Weapon hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        //print("!!!");
        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;
           
            // Weapon outline
            if (objectHitByRaycast.GetComponent<Weapon>() && objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false)
            {
                hoveredWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if(Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                    
                        hoveredWeapon.GetComponent<Outline>().enabled = false;
                    
                }

            }
            else
            {
                if (hoveredWeapon)
                {
                    //print($"{objectHitByRaycast.name} is disable");
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }
            
        //    // Ammo outline
            if (objectHitByRaycast.GetComponent<AmmoBox>())
            {
                hoveredAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();
                hoveredAmmoBox.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                    Destroy(objectHitByRaycast.gameObject);
                    //if (objectHitByRaycast.GetComponent<AmmoBox>())
                    //{
                    //    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                    //}
                }
                

            }
            else
            {
                if (hoveredAmmoBox)
                {
                    //print($"{objectHitByRaycast.name} is disable");
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}
