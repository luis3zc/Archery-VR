using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement_Bear : StateMachineBehaviour {

    GameObject enemy;
    GameObject wp_parent;
    Transform[] waypoints;
    int currWP;
    //should be assigned when instantiated
    [HideInInspector]
    public string wp_parent_tag = "";

    private float rotation_speed = 5.0f;
    private float movement_speed = 3.0f;
    private float transition_dist = 2.0f;
    private bool stop_transition = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        enemy = animator.gameObject;
        currWP = 0;
           
        //game manager is up, assign respective waypoints
        wp_parent = GameObject.FindWithTag(wp_parent_tag);
        waypoints = new Transform[wp_parent.transform.childCount];
        for (int i = 0; i < wp_parent.transform.childCount; i++)
        {
             waypoints[i] = wp_parent.transform.GetChild(i);
        }
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //Debug.Log(waypoints[currWP].gameObject.name);
        if (waypoints.Length == 0)
        {
            return;
        }

        //move to next waypoint
        if (Vector3.Distance(waypoints[currWP].position, enemy.transform.position) < transition_dist && !stop_transition)
        {
            currWP++;
            if (currWP >= waypoints.Length)
            {
                //reached gate...start attacking
                animator.SetBool("AtGate", true);
                stop_transition = true;
                enemy.GetComponent<BearController>().reached_gate = true;
                GameManager.instance.enemies_at_gate++;
            }
        }

        //rotate towards target and move towards wp
        var dir = waypoints[currWP].position - enemy.transform.position;
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(dir), rotation_speed * Time.deltaTime);
        if (!stop_transition)
        {
            enemy.transform.Translate(0, 0, Time.deltaTime * movement_speed);
        }
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
