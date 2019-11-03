using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBorderMovement : MonoBehaviour
{
    public float movement_speed;
    public float offset;

    // Update is called once per frame
    void Update()
    {
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;

        if (x <= 0 + offset)
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + movement_speed * Time.deltaTime);      
        if (x >= Screen.width - offset)
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - movement_speed * Time.deltaTime);
        if (y <= 0 + offset) 
            transform.position = new Vector3(transform.position.x - movement_speed * Time.deltaTime, transform.position.y, transform.position.z);
        if (y >= Screen.height - offset)
            transform.position = new Vector3(transform.position.x + movement_speed * Time.deltaTime, transform.position.y, transform.position.z);
       
    }
}
