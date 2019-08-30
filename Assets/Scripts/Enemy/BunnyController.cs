using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyController : MonoBehaviour {

    Animator anim;
    public GameObject fire_prefab;
    private float death_anim_time = 1.5f;
    [HideInInspector]
    public bool reached_gate = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    //Hit by arrow
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ARROW")
        {
            Debug.Log("arrow");
            //Make arrow stick in enemy
            StickArrow(other);

            if (other.gameObject.GetComponent<Arrow>().on_fire)
            {
                //spawn fire
                Instantiate(fire_prefab, this.gameObject.transform, false);
            }

            //Die
            StartCoroutine(Die());
        }
    }

    void StickArrow(Collider arrow_col)
    {
        //disable collider
        arrow_col.enabled = false;

        //stop physics of arrow
        arrow_col.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        //parent arrow to bunny
        arrow_col.gameObject.transform.parent = this.transform;
        //make arrow stick out a bit
        arrow_col.gameObject.transform.Translate(0, 0, -0.25f);
    }

    IEnumerator Die()
    {
        Debug.Log("dying");
        anim.SetBool("Dead", true);
        if (reached_gate)
        {
            GameManager.instance.enemies_at_gate--;
        }
        GameManager.instance.enemies_alive--;
        yield return new WaitForSeconds(death_anim_time);

        Debug.Log("SHOULD BE NULL ");
        Destroy(this.gameObject);
    }

}
