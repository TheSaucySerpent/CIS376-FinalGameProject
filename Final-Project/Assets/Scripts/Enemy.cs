using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;

    private NavMeshAgent navAgent; // reference to the NavMeshAgent component

  private void Start()
  {
    animator = GetComponent<Animator>(); // get a reference to the Animator component
    navAgent = GetComponent<NavMeshAgent>(); // get a reference to the NavMeshAgent component
  }

  public void TakeDamage(int damageAmount)
  {
    // decrease the HP of the enemy by the damage amount
    HP -= damageAmount;

    // check if the enemy is dead
    if (HP <= 0)
    {
      int randomValue = Random.Range(0, 2); // randomValue = 0 or 1

      // pick a random death animation
      if (randomValue == 0)
      {
        animator.SetTrigger("DIE1");
      }
      else
      {
        animator.SetTrigger("DIE2");
      }
      GetComponent<CapsuleCollider>().enabled = false; // disable the collider after death
      navAgent.enabled = false; // Disable the NavMeshAgent on death
    }
    // the enemy is not dead, only damaged
    else
    {
        animator.SetTrigger("DAMAGE");
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, 2.5f); // Attacking // Stop Attacking

    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(transform.position, 18f); // Detection (Start Chasing)

    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, 21f); // Stop Chasing
  }
}