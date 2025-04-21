using UnityEngine;

public class Zombie : MonoBehaviour
{
    // reference to the zombie hand
    public ZombieHand zombieHand;

    // damage the zombie will inflict on the player
    public int zombieDamage;

    public void Start()
    {
        zombieHand.damage = zombieDamage; // the zombie hand will inflict the zombie damage
    }
}
