using UnityEngine;
using System.Collections;
using Unity.VisualScripting; // need for using IEnumerator
public class Weapon : MonoBehaviour
{   
    // Is this weapon active?
    public bool isActiveWeapon;
    public int weaponDamage; // the damage bullets from this weapon will deal

    [Header("Shooting")]
    // Shooting
    public bool isShooting, readyToShoot;
    private bool allowReset = true;
    public float shootingDelay = 2f;

    [Header("Burst")]
    // Burst shooting mode
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;
    
    [Header("Spread")]
    // Spread 
    public float spreadIntensity;
    public float hipSpreadIntensity; // hip spread
    public float adsSpreadIntensity; // ads spread

    [Header("Bullet")]
    // Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifetime = 3f;

    [Header("Muzzle Effect")]
    // Muzzle Effect
    public GameObject muzzleEffect;

    // Reference to the animator
    internal Animator animator;

    [Header("Reloading")]
    // Loading the Weapon
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    [Header("Weapon Spawning")]
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    [Header("ADS")]
    // boolean to prevent undesired animations from being queued when in ADS
    bool isADS;

    // Different kinds of weapons
    public enum WeaponModel {
        PistolM1911,
        RifleM4_8,
    }

    // the current weapon model
    public WeaponModel thisWeaponModel;

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
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize; // the current bullets left is equal to the magazine size

        spreadIntensity = hipSpreadIntensity; // start with hip spread
    }

    // helper method to set the layer of the weapon recursively
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    // Update is called once per frame
    void Update() {
        // Don't do anything if the weapon is not active
        if (isActiveWeapon) {
            // only set weapon layer to WeaponRender if the weapon is active
            SetLayerRecursively(gameObject, LayerMask.NameToLayer("WeaponRender"));

            // check if the weapon should enter ADS mode
            // don't want to trigger animation each the frame
            if (Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }
            if (Input.GetMouseButtonUp(1))
            {
                ExitADS();
            }

            // prevent an active weapon from ever being outlined
            GetComponent<Outline>().enabled = false;

            if (bulletsLeft == 0 && isShooting) {
                // play the empty magazine sound (same for all weapons)
                SoundManager.Instance.ShootingChannel.PlayOneShot(SoundManager.Instance.emptyMagazine);
            }

            if (currentShootingMode == ShootingMode.Auto) {
                // Holding down the mouse button will shoot the weapon
                isShooting = Input.GetKey(KeyCode.Mouse0); // get key = true if held
            }
            else if (currentShootingMode == ShootingMode.Single || 
                currentShootingMode == ShootingMode.Burst) {
                // Clicking the mouse button will shoot the weapon
                isShooting = Input.GetKeyDown(KeyCode.Mouse0); // get key = true if pressed down once
            }

            if (readyToShoot && isShooting && bulletsLeft > 0) {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }

            // Allow reloading only if weapon is not full on ammo and already reloading (manual reloading)
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0) {
                Reload();
            }
            // automatic reloading
            // else if (readyToShoot && !isShooting && !isReloading && bulletsLeft == 0) {
            //     Reload();
            // }
        }
        else
        {
            // set weapon layer to Default if the weapon is not active
            SetLayerRecursively(gameObject, LayerMask.NameToLayer("Default"));
        }
    }

    private void EnterADS()
    {
        animator.SetTrigger("enterADS");
        isADS = true;
        spreadIntensity = adsSpreadIntensity; // set the spread to the ads spread

        // for if we want to enable/disable middle dot on ADS
        // HUDManager.Instance.middleDot.SetActive(false);
    }

    private void ExitADS()
    {
        animator.SetTrigger("exitADS");
        isADS = false;
        spreadIntensity = hipSpreadIntensity; // set the spread to the hip spread

        // for if we want to enable/disable middle dot on hip fire
        // HUDManager.Instance.middleDot.SetActive(true);
    }

    private void FireWeapon() {
        bulletsLeft--; // decrement the number of bullets left

        // activate the muzzle effect
        muzzleEffect.GetComponent<ParticleSystem>().Play();

        if (isADS) 
        {
            animator.SetTrigger("RECOIL_ADS"); // trigger the recoil animation (ADS)
        }
        else 
        {
            animator.SetTrigger("RECOIL"); // trigger the recoil animation (hip fire)
        }
        
        // SoundManager.Instance.shootingSoundM1911.Play(); // play the shooting sound

        // play the appropriate shooting sound
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false; // don't allow shooting unless shot is done

        Vector3 shootingDirection = CalculateDirectionandSpread().normalized;

        // instantiate a bullet rather than using a ray cast
        // create a bullet at the bullet spawn point with default rotation
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        // reach into the bullet script and set the damage it should do 
        bullet.GetComponent<Bullet>().bulletDamage = weaponDamage;

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

    private void Reload() {
        isReloading = true;
        // SoundManager.Instance.reloadingSoundM1911.Play(); // play the reload sound

        // play the appropriate reloading sound for the weapon
        SoundManager.Instance.PlayReloadingSound(thisWeaponModel);

        if (isADS) 
        {
            // trigger the reload animation (ADS)
        }
        else 
        {
            animator.SetTrigger("RELOAD"); // trigger the reload animation (hip fire)
        }
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted() {
        if (WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize) {
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        else {
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        isReloading = false;
    }

    private void ResetShot() {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionandSpread()  {
        // Shooting from the middle of the screen to check where we are pointing
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
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
