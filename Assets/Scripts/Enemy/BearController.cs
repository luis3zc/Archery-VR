using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour {

    Animator anim;
    public GameObject fire_prefab;
    private float death_anim_time = 1.5f;
    private float times_hit = 0f;       //must be hit twice in order to die
    [HideInInspector]
    public bool reached_gate = false;

    void Start () {
        anim = GetComponent<Animator>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ARROW")
        {
            //Make arrow stick in enemy
            StickArrow(other);
            times_hit++;

            //on fire
            if (other.gameObject.GetComponent<Arrow>().on_fire)
            {
                //Spawn fire on bear
                Instantiate(fire_prefab, this.gameObject.transform, false);
                if (times_hit < 2)
                {
                    StartCoroutine(DieDOT());
                } else
                {
                    StartCoroutine(Die());
                }

            //hit by regular arrow
            } else
            {
                //Die if this is the second time it's been hit
                if (times_hit >= 2)
                {
                    StartCoroutine(Die());
                }
            }
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

    //Die from damage over time from fire arrow
    IEnumerator DieDOT()
    {
        yield return new WaitForSeconds(4f);    //does damage for a few seconds then dies
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        anim.SetBool("Dead", true);
        if (reached_gate)
        {
            GameManager.instance.enemies_at_gate--;
        }
        GameManager.instance.enemies_alive--;
        yield return new WaitForSeconds(death_anim_time);
        Destroy(this.gameObject);
    }
}
