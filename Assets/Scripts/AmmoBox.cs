using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 200;
    public AmmoTypeEnum ammoType;
    public enum AmmoTypeEnum
    {
        RiffleAmmo,
        PistolAmmo,
        ShotgunAmmo
    }
}
