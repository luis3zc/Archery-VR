using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour {

    public static ArrowManager instance;

    public GameObject arrow_prefab;
    public GameObject flame_prefab;
    [HideInInspector]
    public GameObject current_arrow;
    public GameObject right_hand_anchor;
    public GameObject string_anchor;
    public GameObject arrow_attach_anchor;
    public GameObject laser;
    public GameObject bow;

    private float pull_strenth;
    private bool isAttached;
    private bool isPulling;
    private bool flameAttached;
    private float force = 120f;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    void Update()
    {
        AttachAuto();
        PullString();
        CheckForFire();
    }

    //Attaches arrow to right hand
    //public void AttachArrow()
    //{
    //    current_arrow = Instantiate(arrow_prefab, right_hand_anchor.transform, false);
    //}

    //Attaches arrow to bow
    public void AttachArrowToBow()
    {
        current_arrow.transform.parent = string_anchor.transform;

        current_arrow.transform.localPosition = arrow_attach_anchor.transform.localPosition;
        current_arrow.transform.localRotation = arrow_attach_anchor.transform.localRotation;

        isAttached = true;
    }

    //Attaches arrow to bow automatically
    public void AttachAuto()
    {
        if (!current_arrow)
        {
               current_arrow = Instantiate(arrow_prefab, (right_hand_anchor.transform), false);
               flameAttached = false;
        }
    }

    public void AttachFlameToBow()
    {
        if (!flameAttached)
        {
            flameAttached = true;
            GameObject fire = Instantiate(flame_prefab, current_arrow.transform, false);
            fire.transform.localPosition = new Vector3(0, 0, 3.28f);
            current_arrow.GetComponent<Arrow>().on_fire = true;
        }
    }

    //Pulls string if attached to bow and am pressing index trigger
    private void PullString()
    {
        if (isAttached && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            //0.1 -> 0.5
            pull_strenth = (string_anchor.transform.position - right_hand_anchor.transform.position).magnitude;
            string_anchor.transform.localPosition = new Vector3(pull_strenth * 9f, 0, 0);
            isPulling = true;
        }
    }

    //Fires arrow if released button while pulling
    private void CheckForFire()
    {
        if (isPulling && !OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            current_arrow.transform.parent = null;
            Rigidbody rb = current_arrow.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.velocity = current_arrow.transform.forward * pull_strenth * force;

            string_anchor.transform.localPosition = new Vector3(1.416228f, 0, 0);
            current_arrow = null;
            isAttached = false;
            isPulling = false;
        }
    }

    private void CheckArrowRay()
    {
        RaycastHit hit;
        Debug.DrawRay(arrow_attach_anchor.transform.position, arrow_attach_anchor.transform.forward * 500, Color.red);
        if (Physics.Raycast(arrow_attach_anchor.transform.position, arrow_attach_anchor.transform.forward, out hit, Mathf.Infinity))
        {
            //hit something
            if (hit.collider.gameObject.name == "Restart Option")
            {
                Debug.Log("RESTART");
                UIController.instance.Highlight('R');
            }
            if (hit.collider.gameObject.name == "Main Menu Option")
            {
                Debug.Log("MM");
                UIController.instance.Highlight('M');
            }
            if (hit.collider.gameObject.name == "Quit Option")
            {
                Debug.Log("QUIT");
                UIController.instance.Highlight('Q');
            }
            if (hit.collider.gameObject.name != "Restart Option" && hit.collider.gameObject.name != "Main Menu Option" && hit.collider.gameObject.name != "Quit Option")
            {
                UIController.instance.Unhighlight();
            }
        }
    }


    public void AttachLaser()
    {
        //disable bow
        bow.SetActive(false);

        GameObject l = Instantiate(laser, right_hand_anchor.transform, false);
        l.transform.localPosition = new Vector3(0, 0, 200f);
    }
}
