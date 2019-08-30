using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    private float stick_time = 15f;
    [HideInInspector]
    public bool on_fire = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        // If collided with bow, attach to bow
        if (other.gameObject.name == "Golden Bow")
        {
            ArrowManager.instance.AttachArrowToBow();
        } else if (other.gameObject.transform.parent.tag == "ENV")
        {
            //collided with terrain...stick to other collider, stay for 15 sec, then destroy
            StartCoroutine(Stick(other));
        }
    }

    IEnumerator Stick(Collider other)
    {
        //disable collider
        this.gameObject.GetComponent<Collider>().enabled = false;

        //stop physics
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        //parent to hit object and make arrow stick out a bit
        this.transform.parent = other.transform;
        //make arrow stick out a bit
        this.transform.Translate(0, 0, -1f);

        //wait for stick_time and then destroy
        yield return new WaitForSeconds(stick_time);
        Destroy(this.gameObject);
    }

}
