using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class ZombiePatrollingState : StateMachineBehaviour
{
    float timer;
    public float patrollingTime = 10f;

    // reference to the player and zombie agent
    Transform player;
    NavMeshAgent agent;

    public float detectionAreaRadius = 18f; // same as ZombieIdleState
    public float patrolSpeed = 2f; // speed the zombie will patrol at

    // waypoints for the zombie to patrol between
    List<Transform> waypointList = new List<Transform>();


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // initialize the player and agent
       player = GameObject.FindGameObjectWithTag("Player").transform;
       agent = animator.GetComponent<NavMeshAgent>();

       // set agent speed to patrol speed
       agent.speed = patrolSpeed;
       timer = 0; // reset timer

       // get all waypoints and move to the first one
       GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
       foreach (Transform t in waypointCluster.transform)
       {
           waypointList.Add(t);
       }
       Vector3 nextPosition = waypointList[Random.Range(0, waypointList.Count)].position;
       agent.SetDestination(nextPosition);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // did the zombie reach the waypoint? Go to another one
       if (agent.remainingDistance < agent.stoppingDistance)
       {
        agent.SetDestination(waypointList[Random.Range(0, waypointList.Count)].position);
       }

       // are we dont patrolling? Go back to idle
       timer += Time.deltaTime;
       if (timer > patrollingTime)
       {
        animator.SetBool("isPatrolling", false);
       }

       // Is the player inside the detection area? Chase them!
       float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
       if (distanceFromPlayer < detectionAreaRadius)
       {
        animator.SetBool("isChasing", true);
       }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // stop the agent from moving
       agent.SetDestination(agent.transform.position);
    }
}
