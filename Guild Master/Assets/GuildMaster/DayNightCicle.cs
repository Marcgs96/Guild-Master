using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCicle : MonoBehaviour
{
    public Light main_light;
    public Text hours_text;
    float light_intensity;
    int hour = 0; // 1 hour equals 7.5 seconds
    float current_cicle_time = 0;
    float total_cicle_time = 180; // seconds

    public delegate void HourAction();
    public static event HourAction OnHourChange;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Sunrise");
    }

    // Update is called once per frame
    void Update()
    { 
        main_light.intensity = light_intensity;
        if(current_cicle_time == 7.0f)
        {
            hour++;
            OnHourChange?.Invoke();
            current_cicle_time = 0;
        }

        hours_text.text = "Current hour: " + hour;
    }

    IEnumerator Sunrise()
    {
        hour = 7;
        for (float i = 0.2f; i < 2; i += 0.01f)
        {
            light_intensity = i;
            current_cicle_time += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine("NightFall");
    }

    IEnumerator NightFall()
    {
        for (float i = 2; i > 0.2; i -= 0.01f)
        {
            light_intensity = i;
            current_cicle_time += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine("Sunrise");
    }

    public int GetHour()
    {
        return hour;
    }

}
