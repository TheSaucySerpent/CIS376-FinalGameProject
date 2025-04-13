using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set;}

    public AudioSource shootingSoundM1911;

    private void Awake() {
        // only want one instance at a time -- singleton design pattern
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }
}
