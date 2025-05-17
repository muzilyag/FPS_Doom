using System;
using System.Collections;
using System.Collections.Generic;
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

    #region Shotgun
    [Header("Shotgun Settings")]
    public int pelletsPerShot = 8; // Количество дробин
    public float shotgunSpreadAngle = 7f; // Угол разброса
    public bool isShotgun; // Автоматически определяется в Awake()
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
    public Vector3 spawnRotationAfterDrop;

    #region Layers
    private int weaponLayer = 6;
    private int defaultLayer;
    #endregion

    public enum WeaponModelEnum { M1911, M1A1, Shotgun }
    public WeaponModelEnum thisWeaponModel;

    public enum ShootingModeEnum { Single, Burst, Auto }
    public ShootingModeEnum currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
        spreadIntensity = hipSpreadIntensity;
        defaultLayer = LayerMask.NameToLayer("Default");

        // Автоматически помечаем как дробовик если выбрана соответствующая модель
        isShotgun = (thisWeaponModel == WeaponModelEnum.Shotgun);
    }

    private void Update()
    {
        if (isActiveWeapon)
        {
            // Установка слоя для визуализации оружия
            foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = weaponLayer;
            }

            // Прицеливание
            if (Input.GetMouseButtonDown(1)) EnterADS();
            if (Input.GetMouseButtonUp(1)) ExitADS();

            GetComponent<Outline>().enabled = false;

            // Звук пустого магазина
            if (bulletsLeft < 1 && isShooting)
            {
                SoundManager.Instance.emptyMagazineM1911.Play();
            }

            // Режимы стрельбы
            if (currentShootingMode == ShootingModeEnum.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            // Перезарядка
            if (Input.GetKeyDown(KeyCode.R) && !isReloading && bulletsLeft < magazineSize &&
                WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }

            // Автоперезарядка
            if (readyToShoot && !isShooting && !isReloading && bulletsLeft < 1)
            {
                //Reload();
            }

            // Стрельба
            if (readyToShoot && isShooting && !isReloading && bulletsLeft > 0)
            {
                burstBulletLeft = bulletsPerBurst;
                FireWeapon();
            }
        }
        else
        {
            // Сброс слоя когда оружие не активно
            foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = defaultLayer;
            }
        }
    }

    private void FireWeapon()
    {
        bulletsLeft--;

        // Эффекты
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger(isADS ? "RECOIL_ADS" : "RECOIL");
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        if (isShotgun)
        {
            // Режим дробовика - создаем несколько пуль с разбросом
            for (int i = 0; i < pelletsPerShot; i++)
            {
                Vector3 direction = CalculateShotgunSpread();
                CreateBullet(direction);
            }
        }
        else
        {
            // Обычный режим - одна пуля
            Vector3 direction = CalculateDirectionAndSpread();
            CreateBullet(direction);
        }

        // Сброс стрельбы
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // Очередь в режиме Burst
        if (currentShootingMode == ShootingModeEnum.Burst && burstBulletLeft > 1)
        {
            burstBulletLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void CreateBullet(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;
        bullet.transform.forward = direction;
        bullet.GetComponent<Rigidbody>().AddForce(direction * bulletVelocity, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint = ray.GetPoint(100f);
        Vector3 perfectDirection = (targetPoint - bulletSpawn.position).normalized;

        float spreadX = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float spreadY = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        Vector3 right = Camera.main.transform.right * spreadX;
        Vector3 up = Camera.main.transform.up * spreadY;

        return (perfectDirection + right + up).normalized;
    }

    private Vector3 CalculateShotgunSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint = ray.GetPoint(100f);
        Vector3 perfectDirection = (targetPoint - bulletSpawn.position).normalized;

        // Более интенсивный разброс для дробовика
        float spreadX = Mathf.Tan(shotgunSpreadAngle * Mathf.Deg2Rad) * UnityEngine.Random.Range(-1f, 1f);
        float spreadY = Mathf.Tan(shotgunSpreadAngle * Mathf.Deg2Rad) * UnityEngine.Random.Range(-1f, 1f);

        Vector3 right = Camera.main.transform.right * spreadX;
        Vector3 up = Camera.main.transform.up * spreadY;

        return (perfectDirection + right + up).normalized;
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

    private void Reload()
    {
        isReloading = true;
        animator.SetTrigger(thisWeaponModel == WeaponModelEnum.Shotgun && isADS ? "RELOAD_ADS" : "RELOAD");
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = Mathf.Min(magazineSize, WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel));
        WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bullet != null) Destroy(bullet);
    }
}