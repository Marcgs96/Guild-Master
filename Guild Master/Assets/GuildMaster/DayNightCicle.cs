using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCicle : MonoBehaviour
{
    public Light main_light;
    public Text hours_text;
    float light_intensity = 1.4f;
    float hour = 0; // 1 hour equals 7.5 seconds
    float current_cicle_time = 0;
    float total_cicle_time = 180; // seconds
    // Start is called before the first frame update
    void Start()
    {
        hour = 8;
        InvokeRepeating("IncreaseTime", 1.50f, 1.50f);
    }

    // Update is called once per frame
    void Update()
    {
        main_light.intensity = light_intensity;

        hours_text.text = "Current hour: " + Mathf.FloorToInt(hour);
    }

    void IncreaseTime()
    {
        hour+=0.1f;
        if (hour % 2 == 0)
        {

        }
        if(hour <= 12)
        {
            IncreaseIntensity();
        }
        else
        {
            DecreaseIntensity();
        }
        if (hour == 24) hour = 0;
    }

    void IncreaseIntensity()
    {
        light_intensity += 0.015f;
    }

    void DecreaseIntensity()
    {
        light_intensity -= 0.015f;
    }

}
