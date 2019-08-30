using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ARROW")
        {
            //arrow collided w/ flame collider, attach flame prefab to curr bow
            ArrowManager.instance.AttachFlameToBow();
        }
    }
}
