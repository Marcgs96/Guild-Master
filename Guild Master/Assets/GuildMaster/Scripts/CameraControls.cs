using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float movement_speed;
    public float offset;
    public float zoom_speed;

    public float min_y = 5.0f;
    public float max_y = 55.0f;
    public float max_z = 270.0f;
    public float min_z = 110.0f;
    public float max_x = 140.0f;
    public float min_x = -10.0f;
    public float center_offset = -10.0f;
    public float center_zoom = 20.0f;

    GameObject focus = null;


    // Update is called once per frame
    void Update()
    {
        if(focus)
        {
            CenterCamera(focus.transform.position);
        }

        Vector3 pos = transform.position;
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;

        //MOVE
        if (x <= 0 + offset)
            pos.z += movement_speed * Time.deltaTime;
        else if (x >= Screen.width - offset)
            pos.z -= movement_speed * Time.deltaTime;

        if (y <= 0 + offset)
            pos.x -= movement_speed * Time.deltaTime;
        else if (y >= Screen.height - offset)
            pos.x += movement_speed * Time.deltaTime;

        //LIMIT
        if (pos.z > max_z)
            pos.z = max_z;
        else if (pos.z < min_z)
            pos.z = min_z;
        if (pos.x > max_x)
            pos.x = max_x;
        else if (pos.x < min_x)
            pos.x = min_x;

        Vector3 temp_pos = pos;
        temp_pos.y -= Input.mouseScrollDelta.y * zoom_speed * Time.deltaTime;
        temp_pos.x += Input.mouseScrollDelta.y * zoom_speed * Time.deltaTime;

        if (temp_pos.y < max_y && temp_pos.y > min_y)
            pos = temp_pos;

        if (transform.position != pos)
            focus = null;

        transform.position = pos;
    }

    void CenterCamera(Vector3 position)
    {
        transform.position = new Vector3(position.x + center_offset, center_zoom, position.z);
    }

    public void SetFocus(GameObject obj)
    {
        focus = obj;
    }
}
