using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour {
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "POINTER")
        {
            this.gameObject.GetComponent<Image>().color = Color.red;
            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                //pressed
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        this.gameObject.GetComponent<Image>().color = Color.white;
    }
}
