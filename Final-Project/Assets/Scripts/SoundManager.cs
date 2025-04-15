using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set;}

    public AudioSource ShootingChannel;
    public AudioClip M1911Shot;
    public AudioClip M4_8Shot;

    public AudioClip M1911Reload;
    public AudioClip M4_8Reload;
    public AudioClip emptyMagazine;

    private void Awake() {
        // only want one instance at a time -- singleton design pattern
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    public void PlayShootingSound(WeaponModel weapon) {
        switch(weapon) {
            case WeaponModel.PistolM1911:
                ShootingChannel.PlayOneShot(M1911Shot);
                break;
            case WeaponModel.RifleM4_8:
                ShootingChannel.PlayOneShot(M4_8Shot);
                break;
        }
    }

    public void PlayReloadingSound(WeaponModel weapon) {
        switch(weapon) {
            case WeaponModel.PistolM1911:
                ShootingChannel.PlayOneShot(M1911Reload);
                break;
            case WeaponModel.RifleM4_8:
                ShootingChannel.PlayOneShot(M4_8Reload);
                break;
        }
    }
}
