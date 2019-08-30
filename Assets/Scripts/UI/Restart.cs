using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "POINTER")
        {
            this.gameObject.GetComponent<Image>().color = Color.red;
            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                //pressed
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        this.gameObject.GetComponent<Image>().color = Color.white;
    }
}
