using System;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackState : StateMachineBehaviour
{
    // reference to the player and zombie agent
    Transform player;
    NavMeshAgent agent;

    // distance to stop attacking
    public float stopAttackingDistance = 2.5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // initialize the player and agent
       player = GameObject.FindGameObjectWithTag("Player").transform;
       agent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // make the zombie always look at the player 
        LookAtPlayer();
        
        // the distance of the zombie from the player
       float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
       
       // should we stop attacking?
       if (distanceFromPlayer > stopAttackingDistance)
       {
        animator.SetBool("isAttacking", false);
       }
    }

    private void LookAtPlayer()
    {
        // rotate the agent to face the player (look rotation - x axis)
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        // apply y rotation as well
        var yRotation = agent.transform.rotation.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
