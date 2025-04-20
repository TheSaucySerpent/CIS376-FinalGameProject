using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set;}

    public Weapon hoveredWeapon;
    public AmmoBox hoveredAmmoBox;

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
    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

    if (Physics.Raycast(ray, out RaycastHit hit))
    {
      GameObject objectHitByRaycast = hit.transform.gameObject;

      // ensure the object hit by raycast is weapon and is not an already active weapon
      if (objectHitByRaycast.GetComponent<Weapon>() && 
        !objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon)
      {
        hoveredWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
        hoveredWeapon.GetComponent<Outline>().enabled = true;

        if (Input.GetKeyDown(KeyCode.F)) {
            WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
        }
      }
      else
      {
        if (hoveredWeapon)
        {
            hoveredWeapon.GetComponent<Outline>().enabled = false;
        }
      }

      // AmmoBox 
      if (objectHitByRaycast.GetComponent<AmmoBox>())
      {
        hoveredAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();
        hoveredAmmoBox.GetComponent<Outline>().enabled = true;

        // pickup and destroy the ammo box
        if (Input.GetKeyDown(KeyCode.F)) {
            WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
            Destroy(objectHitByRaycast.gameObject);
        }
      }
      else
      {
        if (hoveredAmmoBox)
        {
          hoveredAmmoBox.GetComponent<Outline>().enabled = false;
        }
      }
    }
  }
}
