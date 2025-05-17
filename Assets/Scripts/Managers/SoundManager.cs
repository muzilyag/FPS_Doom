using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;
    public AudioSource ZombieChannel;
    public AudioSource ZombieChannel2;

    [Header("Empty")]
    public AudioSource emptyMagazineM1911;

    [Header("Reload")]
    public AudioSource reloadSoundM1911;
    public AudioSource reloadSoundM1A1;
    public AudioSource reloadSoundShotgun;

    [Header("Shooting")]
    public AudioClip shotM1911;
    public AudioClip shotM1A1;
    public AudioClip shotShotgun;

    [Header("Zombie")]
    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;


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
                ShootingChannel.PlayOneShot(shotM1911);
                break;
            case Weapon.WeaponModelEnum.M1A1:
                ShootingChannel.PlayOneShot(shotM1A1);
                break;
            case Weapon.WeaponModelEnum.Shotgun:
                ShootingChannel.PlayOneShot(shotShotgun);
                break;
        }
    }

    public void PlayReloadSound(Weapon.WeaponModelEnum weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModelEnum.M1911:
                reloadSoundM1911.Play();
                break;
            case Weapon.WeaponModelEnum.M1A1:
                reloadSoundM1A1.Play();
                break;
            case Weapon.WeaponModelEnum.Shotgun:
                reloadSoundShotgun.Play();
                break;
        }
    }
}
