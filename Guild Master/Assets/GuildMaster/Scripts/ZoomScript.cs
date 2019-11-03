using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomScript : MonoBehaviour
{
    public float zoom_speed;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y -= Input.mouseScrollDelta.y * zoom_speed * Time.deltaTime;
        pos.x += Input.mouseScrollDelta.y * zoom_speed * Time.deltaTime;
        transform.position = pos;
    }
}
