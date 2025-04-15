using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set;}

    private void Awake() {
        // only want one instance at a time -- singleton design pattern
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    public void PickupWeapon(GameObject pickedUpWeapon) {
        // Destroy the weapon for testing
        Destroy(pickedUpWeapon);
    }
}
