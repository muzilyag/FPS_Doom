using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class HUDManager : MonoBehaviour
{
    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    //[Header("Trowables")]
    //public Image lethalUI;
    //public TextMeshProUGUI lethalAmountUI;

    //public Image tacticalUI;
    //public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;

    public GameObject middleDot;
    public static HUDManager Instance { get; set; }
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
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if(activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModelEnum model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);
            //print(activeWeaponUI.sprite.rect.size);
            if(unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";
            ammoTypeUI.sprite = emptySlot;
            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModelEnum model)
    {
        switch (model)
        {
            case Weapon.WeaponModelEnum.M1911:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModelEnum.M1A1:
                return Resources.Load<GameObject>("Riffle_Ammo").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModelEnum.Shotgun:
                return Resources.Load<GameObject>("Shotgun_Ammo").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    private Sprite GetWeaponSprite(Weapon.WeaponModelEnum model)
    {
        switch(model)
        {
            case Weapon.WeaponModelEnum.M1911:
                return Resources.Load<GameObject>("M1911_Weapon").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModelEnum.M1A1:
                return Resources.Load<GameObject>("M1A1_Weapon").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModelEnum.Shotgun:
                return Resources.Load<GameObject>("Shotgun_Weapon").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }
    private GameObject GetUnActiveWeaponSlot()
    {
        foreach(GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if(weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }

}
