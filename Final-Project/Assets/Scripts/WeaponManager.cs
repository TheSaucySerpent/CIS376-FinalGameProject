using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set;}

    public List<GameObject> weaponSlots;
    public GameObject activeWeaponSlot;

  public void Start()
  {
    // set the first weapon slot as the active weapon slot
    activeWeaponSlot = weaponSlots[0];
  }

  private void Awake() {
        // only want one instance at a time -- singleton design pattern
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    private void Update() {
        // loop through all weapon slots (2) and set the active weapon slot
        foreach(GameObject weaponSlot in weaponSlots) {
            if (weaponSlot == activeWeaponSlot) {
                weaponSlot.SetActive(true);
            }
            else {
                weaponSlot.SetActive(false);
            }
        }

        // change the active weapon slot based on the key pressed (1 or 2)
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SwitchActiveSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SwitchActiveSlot(1);
        }
    }

    public void PickupWeapon(GameObject pickedUpWeapon) {
        // add the weapon into the appropriate active weapon slot
        AddWeaponIntoActiveSlot(pickedUpWeapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedUpWeapon) {
        // only allow one weapon to be picked up at a time (for now)
        DropCurrentWeapon(pickedUpWeapon);

        // set the active weapon slot to the weapon slot that the weapon is picked up from
        pickedUpWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        // get the weapon component from the picked up weapon
        Weapon weapon = pickedUpWeapon.GetComponent<Weapon>();

        // set the weapon position and rotation to the weapon's spawn position and rotation
        pickedUpWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        // set the weapon to be active for allowing shooting and reloading
        weapon.isActiveWeapon = true;

        // enable animations for the weapon
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickedUpWeapon) {
        // ensure there is a weapon in the active weapon slot
        if (activeWeaponSlot.transform.childCount > 0) {
            // get the weapon component from the current active weapon slot (the weapon to drop)
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            // disable the weapon from being active and disable animations
            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Animator>().enabled = false;

            // set the parent of the weapon to be the parent of the picked up weapon
            // set the position and rotation of the weapon to be the position and rotation 
            // of the picked up weapon (makes it look like you switched them out!)
            weaponToDrop.transform.SetParent(pickedUpWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedUpWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedUpWeapon.transform.localRotation;
        }
    }

    public void SwitchActiveSlot(int slotNumber) {
        // do we have something in the active weapon slot?
        if (activeWeaponSlot.transform.childCount > 0) {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).gameObject.GetComponent<Weapon>();
            // this weapon is no longer the active weapon, since we are switching to a new weapon
            currentWeapon.isActiveWeapon = false;
        }

        // set the new active weapon slot to desired weapon slot passed in (1 or 2)
        activeWeaponSlot = weaponSlots[slotNumber];

        // do we have something in the new active weapon slot?
        if (activeWeaponSlot.transform.childCount > 0) {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).gameObject.GetComponent<Weapon>();
            // this is now the active weapon, since we have switched to it
            newWeapon.isActiveWeapon = true; 
        }
    }
}
