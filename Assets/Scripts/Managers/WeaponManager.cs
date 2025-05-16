using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using static Weapon;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalRiffleAmmo = 0;
    public int totalPistolAmmo = 0;
    public int totalShotgunAmmo = 0;


    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }
    }

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

    public void PickupWeapon(GameObject pickedupWeapon)
    {
        //Destroy(pickedUpWeapon);
        AddWeaponIntoActiveSlot(pickedupWeapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedupWeapon)
    {
        DropCurrentWeapon(pickedupWeapon);

        pickedupWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickedupWeapon.GetComponent<Weapon>();

        pickedupWeapon.transform.localPosition = new Vector3(weapon.spawnPositionAfterPickup.x, weapon.spawnPositionAfterPickup.y, weapon.spawnPositionAfterPickup.z);
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotationAfterPickup);
        
        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickedupWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedupWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedupWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = Quaternion.Euler(weaponToDrop.GetComponent<Weapon>().spawnRotationAfterDrop);
        }
    }


    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if(activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
        
    }

    internal void PickupAmmo(AmmoBox ammo)
    {
        switch(ammo.ammoType)
        {
            case AmmoBox.AmmoTypeEnum.PistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoTypeEnum.RiffleAmmo:
                totalRiffleAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoTypeEnum.ShotgunAmmo:
                totalShotgunAmmo += ammo.ammoAmount;
                break;
        }
    }
    public int CheckAmmoLeftFor(Weapon.WeaponModelEnum thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case WeaponModelEnum.M1911:
                return Instance.totalPistolAmmo;
            case WeaponModelEnum.M1A1:
                return Instance.totalRiffleAmmo;
            case WeaponModelEnum.Shotgun:
                return Instance.totalShotgunAmmo;
            default:
                return 0;
        }
    }
    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponModelEnum thisWeaponModel)
    {
        switch(thisWeaponModel)
        {
            case Weapon.WeaponModelEnum.M1911:
                totalPistolAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModelEnum.M1A1:
                totalRiffleAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModelEnum.Shotgun:
                totalShotgunAmmo -= bulletsToDecrease;
                break;
        }
    }
}
