using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomScript : MonoBehaviour
{
    public float zoom_speed;

    public float min_y = 5.0f;
    public float max_y = 55.0f;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y -= Input.mouseScrollDelta.y * zoom_speed * Time.deltaTime;
        pos.x += Input.mouseScrollDelta.y * zoom_speed * Time.deltaTime;


        if (pos.y < max_y && pos.y > min_y)
            transform.position = pos;
    }
}
