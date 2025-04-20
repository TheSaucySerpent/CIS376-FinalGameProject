using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{
    // reference to the player and zombie agent
    Transform player;
    NavMeshAgent agent;

    public float chaseSpeed = 6f; // speed the zombie will chase at
    public float stopChasingDistance = 21; // how close the zombie will get before it stops chasing
    public float attackingDistance = 2.5f; // how close the zombie will get before it attacks


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // initialize the player and agent
       player = GameObject.FindGameObjectWithTag("Player").transform;
       agent = animator.GetComponent<NavMeshAgent>();
       
       agent.speed = chaseSpeed; // the speed of the zombie when chasing
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // the player is our destination
       agent.SetDestination(player.position);
       animator.transform.LookAt(player); // look at the player when chasing

       // the distance of the zombie from the player
       float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
       
       // should we stop chasing?
       if (distanceFromPlayer > stopChasingDistance)
       {
        animator.SetBool("isChasing", false);
       }

       // should we start attacking?
       if (distanceFromPlayer < attackingDistance)
       {
        animator.SetBool("isAttacking", true);
       }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // stop the agent from moving
       agent.SetDestination(agent.transform.position);
    }
}
