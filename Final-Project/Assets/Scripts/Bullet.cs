using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        // destroy the bullet when it collides with a target
        if (collision.gameObject.CompareTag("Target")) {
            print("hit " + collision.gameObject.name + " !"); 
            Destroy(gameObject);
        }

        // destroy the bullet when it collides with a wall
        if (collision.gameObject.CompareTag("Wall")) {
            print("hit a wall"); 
            Destroy(gameObject);
        }
    }
}
