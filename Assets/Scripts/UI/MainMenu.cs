using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "POINTER")
        {
            SceneManager.LoadScene("Menu");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        this.gameObject.GetComponent<Image>().color = Color.white;
    }
}
