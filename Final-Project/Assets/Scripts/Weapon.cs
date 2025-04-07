using UnityEngine;
using System.Collections; // need for using IEnumerator

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVeloocity = 30;
    public float bulletPrefabLifetime = 3f;

    // Update is called once per frame
    void Update() {
        // left mouse click fires the weapon
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            FireWeapon();
        }
    }

    private void FireWeapon() {
        // instantiate a bullet rather than using a ray cast
        // create a bullet at the bullet spawn point with default rotation
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        // shoot the bullet forward (blue axis direction)
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward * bulletVeloocity, ForceMode.Impulse);

        // destroy the bullet after a certain amount of time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifetime));
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay) {
        yield return new WaitForSeconds(delay); // wait for the delay
        Destroy(bullet); // destroy the bullet
    }
}
