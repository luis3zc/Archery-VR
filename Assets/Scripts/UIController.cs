using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public static UIController instance;

    public Text health_text;
    public Slider health_slider;
    public Image waveAnn_img;
    public Text waveAnn_text;
    public RectTransform start;
    public RectTransform end;
    public RectTransform left_mid;
    public RectTransform right_mid;
    public Image fin_menu;
    public Text fin_mainText;
    public Text fin_msgText;
    public Image restart;
    public Image main_menu;
    public Image quit;

    private float startTime;
    private float journeyLength;
    private int movement_stage = 1;
    private float startExit_speed = 300f;
    private float mid_speed = 50f;
    private float movement_speed = 200f;
    private int wave_num = -1;
    private Vector3 pointA;
    private Vector3 pointB;

    private float max_health = 100;
    [HideInInspector]
    public float curr_health = 100;
    private float enemy_impact = 0.01f;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void Update()
    {
        UpdateHealth();
        SendWaveAnnouncement(wave_num);
    }

    private void UpdateHealth()
    {
        //calculate amount of health to take away depending on num enemies at gate
        float health_loss = (float)GameManager.instance.enemies_at_gate * enemy_impact;
        curr_health -= health_loss;

        //update health text
        health_text.text = "Gate Integrity: " + (int)curr_health + "%";

        //update health bar
        health_slider.value = curr_health / max_health;
    }

    private void SendWaveAnnouncement(int wave)
    {
        //don't send if wave_num = -1
        if (wave == -1)
        {
            return;
        }

        //check stage
        if (movement_stage == 1 && waveAnn_img.rectTransform.position.x >= left_mid.position.x)
        {
            Debug.Log("reached mid");
            //at middle
            movement_stage = 2;
            movement_speed = mid_speed;
            pointA = waveAnn_img.rectTransform.position;
            pointB = right_mid.position;
            journeyLength = Vector3.Distance(pointA, pointB);
            startTime = Time.time;
        }
        else if (movement_stage == 2 && waveAnn_img.rectTransform.position.x >= right_mid.position.x)
        {
            //passed middle, exit now
            movement_stage = 3;
            movement_speed = startExit_speed;
            pointA = waveAnn_img.rectTransform.position;
            pointB = end.position;
            journeyLength = Vector3.Distance(pointA, pointB);
            startTime = Time.time;
        } else if (movement_stage == 3 && waveAnn_img.rectTransform.position.x >= end.position.x)
        {
            //done, reset
            waveAnn_img.gameObject.SetActive(false);
            wave_num = -1;
            return;
        }

        float distCovered = (Time.time - startTime) * movement_speed;
        float fracJourney = distCovered / journeyLength;
        waveAnn_img.rectTransform.position = Vector3.Lerp(pointA, pointB, fracJourney);
    }

    public void PrepareWaveAnnouncement(int wave)
    {
        //enable and write correct text
        waveAnn_img.gameObject.SetActive(true);
        waveAnn_text.text = "Wave " + wave;

        //begin from start
        waveAnn_img.rectTransform.position = start.position;
        movement_stage = 1;

        //set points & dist
        pointA = start.position;
        pointB = left_mid.position;
        journeyLength = Vector3.Distance(pointA, pointB);

        //allow SendWaveAnnouncement to begin
        startTime = Time.time;
        movement_speed = startExit_speed;
        wave_num = wave;
    }

    public void DisplayFin(bool won)
    {
        //enable & adjust text
        if (won)
        {
            fin_mainText.text = "Mission Successful!";
            fin_msgText.text = "The town remains safe.";
        } else
        {
            fin_mainText.text = "Mission Failed!";
            fin_msgText.text = "The town is destroyed.";
        }
        fin_menu.gameObject.SetActive(true);
    }

    public void Unhighlight()
    {
        restart.color = Color.white;
        main_menu.color = Color.white;
        quit.color = Color.white;
    }

    public void Highlight(char option)
    {
        switch(option)
        {
            case 'R':
                restart.color = Color.red;
                break;
            case 'M':
                main_menu.color = Color.red;
                break;
            case 'Q':
                quit.color = Color.red;
                break;
            default:
                break;
        }
    }
}
