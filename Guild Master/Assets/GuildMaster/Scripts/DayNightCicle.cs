using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCicle : MonoBehaviour
{
    enum CYCLE_SKYBOX { MORNING, AFTERNOON, NIGHT};
    public Light main_light;
    public GameObject[] lights;
    public Text hours_text;
    public Text day_text;
    float light_intensity;
    public int hour = 0; // 1 hour equals 15 seconds
    int minute = -1;
    float hour_count = 0.0f;
    public int day = 1;
    public Material[] skybox_materials;
    Skybox skybox;

    public delegate void DayAction(bool night);
    public event DayAction OnDayCycleChange;
    public event DayAction OnDayChange;

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


        hours_text.text = "Hour: " + Mathf.FloorToInt(hour) + ":" + minute;
    }

    public float InGameHoursToSeconds(float hour)
    {
        return 15.0f * hour;
    }

    public void AdvanceDay()
    {
        day++;
        hour = 0;
        day_text.text = "Day: " + day.ToString();
        OnDayChange?.Invoke(true);
    }

    void IncreaseTime()
    {
        hour_count += 0.1f;
        if(Mathf.Floor(hour_count) >= 1.0f)
        {
            hour++;
            if (hour == 24)
                AdvanceDay();

            hour_count = 0;
            minute = -1;
        }
        if(hour < 12)
        {
            IncreaseIntensity();
        }
        else if (hour > 12)
        {
            DecreaseIntensity();
        }

        if(hour == 6)
        {
            OnDayCycleChange?.Invoke(false);
            foreach (GameObject l in lights)
            {
                l.GetComponent<Light>().enabled = false;
            }
            skybox.material = skybox_materials[(int)CYCLE_SKYBOX.MORNING];
        }

        if (hour == 18)
        {
            foreach (GameObject l in lights)
            {
                l.GetComponent<Light>().enabled = true;
            }
            skybox.material = skybox_materials[(int)CYCLE_SKYBOX.AFTERNOON];
        }

        if (hour == 22)
        {
            OnDayCycleChange?.Invoke(true);
            skybox.material = skybox_materials[(int)CYCLE_SKYBOX.NIGHT];
        }
    }

    void IncreaseIntensity()
    {
        light_intensity += 0.010f;
    }

    void DecreaseIntensity()
    {
        light_intensity -= 0.010f;
    }

    public uint GetHour()
    {
        return (uint)hour;
    }

    void Minutes()
    {
       minute++;
    }
}
