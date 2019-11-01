using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBehaviour : MonoBehaviour
{
    bool training = false;
    DayNightCicle time;


    // Start is called before the first frame update
    void Start()
    {
        time = GameObject.Find("GameManager").GetComponent<DayNightCicle>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
