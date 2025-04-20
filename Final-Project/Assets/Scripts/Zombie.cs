using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;

  private void Start()
  {
    // get a reference to the Animator component
    animator = GetComponent<Animator>();
  }

  public void TakeDamage(int damageAmount)
  {
    // decrease the HP of the zombie by the damage amount
    HP -= damageAmount;

    // check if the zombie is dead
    if (HP <= 0)
    {
      // play the death animation
      animator.SetTrigger("DIE");
      Destroy(gameObject); // for now just destroy the zombie
    }
    // the zombie is not dead, only damaged
    else
    {
        animator.SetTrigger("DAMAGE");
    }
  }
}
