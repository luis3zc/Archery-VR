using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public Transform inst_loc_A;
    public Transform inst_loc_B;
    public GameObject bunny_prefab;
    public GameObject bear_prefab;
    [HideInInspector]
    public int enemies_at_gate = 0;
    public int enemies_alive = 0;

    private int curr_wave = 1;
    private int num_waves = 3;

    private float wave1_wait = 15f;
    private float wave2_wait = 30f;
    private float start_wait = 6f;      //amount of time to wait before wave 1 begins
    private float subwave_wait = 3f;    //amount of time to wait between subwaves in wave
    [HideInInspector]
    public bool fin = false;

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

    void Start () {
        //ignore collisions between enemies
        Physics.IgnoreLayerCollision(9, 10);
        StartCoroutine(SpawnWaves());
    }

    private void Update()
    {
        if (!fin)
        {
            //check for fin success
            if (curr_wave == 3 && enemies_alive <= 0)
            {
                curr_wave = -1;
                fin = true;
                UIController.instance.DisplayFin(true);
                ArrowManager.instance.AttachLaser();
            }

            //check for fail
            if (UIController.instance.curr_health <= 0)
            {
                curr_wave = -1;
                fin = true;
                enemies_at_gate = 0;
                //disable all enemies
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");
                foreach (GameObject en in enemies)
                {
                    en.SetActive(false);
                }
                UIController.instance.DisplayFin(false);
                ArrowManager.instance.AttachLaser();
            }
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(start_wait);

        for (int i = 1; i <= num_waves; i++)
        {
            curr_wave = i;
            switch (curr_wave)
            {
                case 1:
                    Debug.Log("wave 1");
                    UIController.instance.PrepareWaveAnnouncement(1);
                    StartCoroutine(Wave1Handler());
                    enemies_alive += 4;
                    yield return new WaitForSeconds(wave1_wait);
                    break;
                case 2:
                    Debug.Log("wave 2");
                    UIController.instance.PrepareWaveAnnouncement(2);
                    StartCoroutine(Wave2Handler());
                    enemies_alive += 6;
                    yield return new WaitForSeconds(wave2_wait);
                    break;
                case 3:
                    Debug.Log("wave 3");
                    UIController.instance.PrepareWaveAnnouncement(3);
                    StartCoroutine(Wave3Handler());
                    enemies_alive += 8;
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator Wave1Handler()
    {
        InstBunny("WP_C");
        InstBunny("WP_D");
        yield return new WaitForSeconds(subwave_wait);
        InstBunny("WP_A");
        InstBear("WP_E");
    }

    IEnumerator Wave2Handler()
    {
        InstBear("WP_B");
        InstBear("WP_E");
        yield return new WaitForSeconds(subwave_wait);
        InstBunny("WP_C");
        InstBunny("WP_D");
        yield return new WaitForSeconds(subwave_wait);
        InstBear("WP_B");
        InstBunny("WP_D");
    }

    IEnumerator Wave3Handler()
    {
        InstBunny("WP_A");
        InstBear("WP_E");
        yield return new WaitForSeconds(subwave_wait - 1);
        InstBear("WP_C");
        InstBear("WP_D");
        yield return new WaitForSeconds(subwave_wait - 1);
        InstBunny("WP_C");
        InstBunny("WP_D");
        yield return new WaitForSeconds(subwave_wait - 1);
        InstBunny("WP_A");
        InstBear("WP_D");
    }

    void InstBunny(string path)
    {
        if (path == "WP_D" || path == "WP_E")
        {
            GameObject bun = Instantiate(bunny_prefab, inst_loc_B.position, inst_loc_B.rotation);
            bun.GetComponent<Animator>().GetBehaviour<WaypointMovement_Bunny>().wp_parent_tag = path;
        } else
        {
            GameObject bun = Instantiate(bunny_prefab, inst_loc_A.position, inst_loc_A.rotation);
            bun.GetComponent<Animator>().GetBehaviour<WaypointMovement_Bunny>().wp_parent_tag = path;
        }
    }

    void InstBear(string path)
    {
        if (path == "WP_D" || path == "WP_E")
        {
            GameObject bear = Instantiate(bear_prefab, inst_loc_B.position, inst_loc_B.rotation);
            bear.GetComponent<Animator>().GetBehaviour<WaypointMovement_Bear>().wp_parent_tag = path;
        }
        else
        {
            GameObject bear = Instantiate(bear_prefab, inst_loc_A.position, inst_loc_A.rotation);
            bear.GetComponent<Animator>().GetBehaviour<WaypointMovement_Bear>().wp_parent_tag = path;
        }
    }

}
