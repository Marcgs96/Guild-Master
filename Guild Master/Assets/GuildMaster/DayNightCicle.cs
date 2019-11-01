using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCicle : MonoBehaviour
{
    public Light main_light;
    float light_intensity;
    float hour = 0; // 1 hour equals 7.5 seconds
    float current_cicle_time = 0;
    float total_cicle_time = 180; // seconds
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Sunrise");
    }

    // Update is called once per frame
    void Update()
    { 
        main_light.intensity = light_intensity;
        Debug.Log(hour);
        if(current_cicle_time == 7.5f)
        {
            hour++;
            current_cicle_time = 0;
        }
    }

    IEnumerator Sunrise()
    {
        hour = 0;
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
        hour = 12;
        for (float i = 2; i > 0.2; i -= 0.01f)
        {
            light_intensity = i;
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine("Sunrise");
    }

}
