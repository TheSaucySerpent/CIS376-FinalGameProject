using UnityEngine;
using System.Collections.Generic;

public class BeerBottle : MonoBehaviour
{
    public List<Rigidbody> allParts = new List<Rigidbody>();

    public void Shatter() {
        foreach ( Rigidbody rb in allParts) {
            rb.isKinematic = false; 
        }
    }
}
