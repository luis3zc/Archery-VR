using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRefill : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //If right hand reaches arrow supply and there is not current arrow, instantiate arrow
    private void OnTriggerEnter(Collider other)
    {
       //ebug.Log("Collided with " + other.gameObject.name + " current arrow is " + arrow_manager.current_arrow);
        if (other.gameObject.name == "RightHandAnchor" && !ArrowManager.instance.current_arrow)
        {
            // Debug.Log("Called attach Arrow");
            //ArrowManager.instance.AttachArrow();
        }
    }
}
