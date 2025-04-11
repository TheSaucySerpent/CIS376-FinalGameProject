using UnityEngine;
using System.Collections; // need for using IEnumerator

public class Weapon : MonoBehaviour
{
    public Camera playerCamera;
    
    // Shooting
    public bool isShooting, readyToShoot;
    private bool allowReset = true;
    public float shootingDelay = 2f;

    // Burst shooting mode
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;
    
    // Spread
    public float spreadIntensity;

    // Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifetime = 3f;

    // Shooting Modes
    public enum ShootingMode {
        Single,
        Burst,
        Auto
    }
    public ShootingMode currentShootingMode; // the current shooting mode of the weapon

    private void Awake() {
        readyToShoot = true; // the weapon is ready to shoot
        burstBulletsLeft = bulletsPerBurst; // the current burst is equal to the number of bullets per burst
    }

    // Update is called once per frame
    void Update() {
        if (currentShootingMode == ShootingMode.Auto) {
            // Holding down the mouse button will shoot the weapon
            isShooting = Input.GetKey(KeyCode.Mouse0); // get key = true if held
        }
        else if (currentShootingMode == ShootingMode.Single || 
            currentShootingMode == ShootingMode.Burst) {
            // Clicking the mouse button will shoot the weapon
            isShooting = Input.GetKeyDown(KeyCode.Mouse0); // get key = true if pressed down once
        }

        if (readyToShoot && isShooting) {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon() {
        readyToShoot = false; // don't allow shooting unless shot is done

        Vector3 shootingDirection = CalculateDirectionandSpread().normalized;

        // instantiate a bullet rather than using a ray cast
        // create a bullet at the bullet spawn point with default rotation
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        // point the bullet in the direction of the shooting direction
        bullet.transform.forward = shootingDirection;

        // shoot the bullet in the shooting direction, applying spread
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        // destroy the bullet after a certain amount of time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifetime));

        // Checking if we are done shooting
        if (allowReset) {
            Invoke("ResetShot", shootingDelay); // lock to only reset shot once
            allowReset = false;
        }

        // we already shot once before this check
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void ResetShot() {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionandSpread()  {
        // Shooting from the middle of the screen to check where we are pointing
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit)) {
            // we hit something
            targetPoint = hit.point;
        }
        else {
            // we didn't hit anything, where should the bullet fly to?
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        // spread can apply to both x and y axis
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0); // calculate direction and spread
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay) {
        yield return new WaitForSeconds(delay); // wait for the delay
        Destroy(bullet); // destroy the bullet
    }
}
