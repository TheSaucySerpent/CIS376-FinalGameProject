using UnityEngine;

public class ZombieIdleState : StateMachineBehaviour
{
    float timer;
    public float idleTime = 0;

    // reference to the player
    Transform player;

    // how far the zombie can see the player
    public float detectionAreaRadius = 18f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // reset timer
       timer = 0;

       // get reference to the player
       player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // increase the timer
        timer += Time.deltaTime;

        // start patrolling if we've been idle for long enough
        if (timer > idleTime) 
        {
            animator.SetBool("isPatrolling", true);
        }

        // Is the player inside the detection area? Chase them!
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionAreaRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }
}
