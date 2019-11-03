﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCicle : MonoBehaviour
{
    enum CYCLE_SKYBOX { MORNING, AFTERNOON, NIGHT};
    public Light main_light;
    public GameObject[] lights;
    public Text hours_text;
    float light_intensity;
    int hour = 0; // 1 hour equals 15 seconds
    int minute = -1;
    float hour_count = 0.0f;
    public Material[] skybox_materials;
    Skybox skybox;
    public delegate void HourAction();
    public static event HourAction OnHourChange;

    // Start is called before the first frame update
    void Start()
    {
        skybox = GetComponent<Skybox>();
        light_intensity = main_light.intensity;
        hour = 5;
        InvokeRepeating("IncreaseTime", 1.50f, 1.50f);
        InvokeRepeating("Minutes", 0.25f, 0.25f);
        lights = GameObject.FindGameObjectsWithTag("Light");
        StartCoroutine("Minutes");
    }

    // Update is called once per frame
    void Update()
    {
        main_light.intensity = light_intensity;


        hours_text.text = "Current time: " + Mathf.FloorToInt(hour) + ":" + minute;
    }

    void IncreaseTime()
    {
        hour_count += 0.1f;
        if(Mathf.Floor(hour_count) >= 1.0f)
        {
            hour++;
            if (hour == 24)
                hour = 0;
            OnHourChange?.Invoke();
            hour_count = 0;
            minute = -1;
        }
        if(hour <= 12)
        {
            IncreaseIntensity();
        }
        else
        {
            DecreaseIntensity();
        }

        if(hour == 6)
        {
            foreach (GameObject l in lights)
            {
                l.GetComponent<Light>().enabled = false;
            }
            skybox.material = skybox_materials[(int)CYCLE_SKYBOX.MORNING];
        }

        if (hour == 21)
        {
            foreach (GameObject l in lights)
            {
                l.GetComponent<Light>().enabled = true;
            }
            skybox.material = skybox_materials[(int)CYCLE_SKYBOX.AFTERNOON];
        }

        if(hour == 23) skybox.material = skybox_materials[(int)CYCLE_SKYBOX.NIGHT];

    }

    void IncreaseIntensity()
    {
        light_intensity += 0.015f;
    }

    void DecreaseIntensity()
    {
        light_intensity -= 0.015f;
    }

    public int GetHour()
    {
        return hour;
    }

    void Minutes()
    {
       minute++;
    }
}
