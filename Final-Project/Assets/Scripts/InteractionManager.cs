using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set;}

    public Weapon hoveredWeapon;

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
    }
  }
}
