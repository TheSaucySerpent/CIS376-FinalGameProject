using UnityEngine;

// want GlobalReferences to be a singleton
public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences Instance { get; set;}

    public GameObject bulletImpactEffectPrefab;

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
