using UnityEngine;

public class Player : MonoBehaviour
{
    public int HP = 100;

    public void TakeDamage(int damageAmount)
  {
    // decrease the HP of the enemy by the damage amount
    HP -= damageAmount;

    // check if the player is dead
    if (HP <= 0)
    {
        print("Player Dead");

        // Game over
        // Re Spawn Player
        // Dying Animation
    }
    else
    {
        print("Player Hit");
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    // check if the ZombieHand hit us
    if (other.CompareTag("ZombieHand"))
    {
        // get the damage amount from the ZombieHand script
        TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
    }
  }
}
