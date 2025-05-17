using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public int weaponDamage;
    #region Shooting
    [Header("Shooting")]
    private bool allowReset = true;
    public bool isShooting, readyToShoot;
    public float shootingDelay = 2f;
    #endregion

    #region Burst
    [Header("Burst")]
    public int bulletsPerBurst = 3;
    public int burstBulletLeft;
    #endregion

    #region Bullet
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;
    #endregion

    #region Effects
    [Header("Effects")]
    internal Animator animator;
    public GameObject muzzleEffect;
    #endregion

    #region Loading
    [Header("Loading")]
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;
    #endregion

    #region Spread
    [Header("Spread")]
    public float spreadIntensity;
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;
    #endregion

    public bool isADS;

    [Header("Pickup")]
    public Vector3 spawnPositionAfterPickup;
    public Vector3 spawnRotationAfterPickup;
    [Header("Drop")]
    //public Vector3 spawnPositionAfterDrop;
    public Vector3 spawnRotationAfterDrop;

    #region Layers
    private int weaponLayer;
    private int defaultLayer;
    #endregion
    public enum WeaponModelEnum
    {
        M1911,
        M1A1,
        Shotgun
    }

    public WeaponModelEnum thisWeaponModel;
    public enum ShootingModeEnum
    {
        Single,
        Burst,
        Auto
    }

    public ShootingModeEnum currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
        spreadIntensity = hipSpreadIntensity;
        weaponLayer = 6;
        defaultLayer = LayerMask.NameToLayer("Default");
    }

    private void Update()
    {
        if(isActiveWeapon)
        {
            //transform.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            //int i = 0;
            foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = weaponLayer;
                //print("Hello! Is WeaponRenderLayer!");
            }


            if(Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }
            if(Input.GetMouseButtonUp(1))
            {
                ExitADS();
            }



            GetComponent<Outline>().enabled = false;

            if (bulletsLeft < 1 && isShooting)
            {
                SoundManager.Instance.emptyMagazineM1911.Play();
            }

            if (currentShootingMode == ShootingModeEnum.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && !isReloading && bulletsLeft < magazineSize && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }

            if (readyToShoot && !isShooting && !isReloading && bulletsLeft < 1)
            {
                //Reload();
            }

            if (readyToShoot && isShooting && !isReloading && bulletsLeft > 0)
            {
                burstBulletLeft = bulletsPerBurst;
                FireWeapon();
            }


            //if (AmmoManager.Instance.ammoDisplay != null)
            //{
            //    AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft / bulletsPerBurst}/{magazineSize / bulletsPerBurst}";
            //}
        }
        else
        {
            foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = defaultLayer;
                //print("Unactive");
            }
        }
    }

    private void EnterADS()
    {
        animator.SetTrigger("ENTER_ADS");
        isADS = true;
        HUDManager.Instance.middleDot.SetActive(false);
        spreadIntensity = adsSpreadIntensity;
    }

    private void ExitADS()
    {
        animator.SetTrigger("EXIT_ADS");
        isADS = false;
        HUDManager.Instance.middleDot.SetActive(true);
        spreadIntensity = hipSpreadIntensity;
    }

    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        if(isADS)
        {
            animator.SetTrigger("RECOIL_ADS");
        }
        else
        {
            animator.SetTrigger("RECOIL");
        }

        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab.gameObject, bulletSpawn.position, Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        bullet.transform.forward = shootingDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if(allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if (currentShootingMode == ShootingModeEnum.Burst && burstBulletLeft > 1)
        {
            burstBulletLeft--;
            Invoke("FireWeapon", shootingDelay);
        }

    }

    private void Reload()
    {
        isReloading = true;

        if(thisWeaponModel == WeaponModelEnum.Shotgun && isADS)
        {
            animator.SetTrigger("RELOAD_ADS");
        }
        else
        {
            animator.SetTrigger("RELOAD");
        }

        //SoundManager.Instance.reloadSound_m1911.Play();
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);

        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        if(WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize)
        {
            bulletsLeft = magazineSize;
        }
        else
        {
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
        }

        WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        isReloading = false;
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        //print($"y = {y} z = {z}");
        return direction + new Vector3(0, y, z);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet,  float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
