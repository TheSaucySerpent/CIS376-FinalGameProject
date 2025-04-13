using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // need for using TextMeshProUGUI

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance { get; set;}

    // UI
    public TextMeshProUGUI ammoDisplay;

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
