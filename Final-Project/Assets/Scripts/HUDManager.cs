using TMPro;
using UnityEditor;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set;}

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public UnityEngine.UI.Image ammoTypeUI;

    [Header("Weapon")]
    public UnityEngine.UI.Image activeWeaponUI;
    public UnityEngine.UI.Image inactiveWeaponUI;

    [Header("Throwables")]
    public UnityEngine.UI.Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public UnityEngine.UI.Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot; // want empty slot to be transparent

    private void Awake() {
        // only want one instance at a time -- singleton design pattern
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

  public void Update() {
    Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
    Weapon inactiveWeapon = GetInactiveWeaponSlot().GetComponentInChildren<Weapon>();

    if (activeWeapon) {
        // set the bullets left and total ammo accordinly
        magazineAmmoUI.text = $"{activeWeapon.bulletsLeft}";
        totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

        // set the ammo type sprite
        Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
        ammoTypeUI.sprite = GetAmmoSprite(model);

        activeWeaponUI.sprite = GetWeaponSprite(model);

        if (inactiveWeapon) {
            inactiveWeaponUI.sprite = GetWeaponSprite(inactiveWeapon.thisWeaponModel);
        }
    }
    else {
        magazineAmmoUI.text = "";
        totalAmmoUI.text = "";

        ammoTypeUI.sprite = emptySlot;
        activeWeaponUI.sprite = emptySlot;
        inactiveWeaponUI.sprite = emptySlot;
    }
  }

  private Sprite GetWeaponSprite(Weapon.WeaponModel model) {
    // instantiate the proper weapon sprite
    switch (model) {
        case Weapon.WeaponModel.PistolM1911:
            return Resources.Load<Sprite>("PistolM1911");
        case Weapon.WeaponModel.RifleM4_8:
            return Resources.Load<Sprite>("RifleM4_8");
        default:
            return null;
    }
  }

  private Sprite GetAmmoSprite(Weapon.WeaponModel model) {
    // instantiate the proper ammo sprite
    switch (model) {
        case Weapon.WeaponModel.PistolM1911:
            return Resources.Load<Sprite>("Pistol_Ammo");
        case Weapon.WeaponModel.RifleM4_8:
            return Resources.Load<Sprite>("Rifle_Ammo");
        default:
            return null;
    }
  }

  private GameObject GetInactiveWeaponSlot() {
    // loop through all weapon slots (2) and return the inactive weapon slot
    foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots) {
        if (weaponSlot != WeaponManager.Instance.activeWeaponSlot) {
            return weaponSlot;
        }
    }
    // this will never happen, but something needs to be returned
    return null;
  }
}
