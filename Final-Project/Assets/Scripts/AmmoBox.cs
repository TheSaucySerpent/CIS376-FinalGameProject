using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 200;
    public AmmoType ammoType;

    // the different types of ammo. Guns of the same type share the same ammo type.
    public enum AmmoType
    {
        RifleAmmo,
        PistolAmmo,
    }
}
