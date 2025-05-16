using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;

    [Header("Empty")]
    public AudioSource emptyMagazine_m1911;

    [Header("Reload")]
    public AudioSource reloadSound_m1911;
    public AudioSource reloadSound_m1a1;
    public AudioSource reloadSound_shotgun;

    [Header("Shooting")]
    public AudioClip shot_m1911;
    public AudioClip shot_m1a1;
    public AudioClip shot_shotgun;

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

    public void PlayShootingSound(Weapon.WeaponModelEnum weapon)
    {
        switch(weapon)
        {
            case Weapon.WeaponModelEnum.M1911:
                ShootingChannel.PlayOneShot(shot_m1911);
                break;
            case Weapon.WeaponModelEnum.M1A1:
                ShootingChannel.PlayOneShot(shot_m1a1);
                break;
            case Weapon.WeaponModelEnum.Shotgun:
                ShootingChannel.PlayOneShot(shot_shotgun);
                break;
        }
    }

    public void PlayReloadSound(Weapon.WeaponModelEnum weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModelEnum.M1911:
                reloadSound_m1911.Play();
                break;
            case Weapon.WeaponModelEnum.M1A1:
                reloadSound_m1a1.Play();
                break;
            case Weapon.WeaponModelEnum.Shotgun:
                reloadSound_shotgun.Play();
                break;
        }
    }
}
