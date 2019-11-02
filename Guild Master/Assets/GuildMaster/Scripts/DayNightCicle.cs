using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCicle : MonoBehaviour
{
    public Light main_light;
    public Text hours_text;
    float light_intensity;
    int hour = 0; // 1 hour equals 15 seconds
    float hour_count = 0.0f;

    public delegate void HourAction();
    public static event HourAction OnHourChange;

    // Start is called before the first frame update
    void Start()
    {
        light_intensity = main_light.intensity;
        hour = 5;
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
        hour_count += 0.1f;
        if(Mathf.Floor(hour_count) >= 1.0f)
        {
            hour++;
            if (hour == 24)
                hour = 0;
            OnHourChange?.Invoke();
            hour_count = 0;
        }
        if(hour <= 12)
        {
            IncreaseIntensity();
        }
        else
        {
            DecreaseIntensity();
        }
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
}
