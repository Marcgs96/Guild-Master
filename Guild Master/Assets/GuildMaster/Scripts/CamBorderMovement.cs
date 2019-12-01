using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBorderMovement : MonoBehaviour
{
    public float movement_speed;
    public float offset;

    public float max_z = 270.0f;
    public float min_z = 110.0f;
    public float max_x = 140.0f;
    public float min_x = -10.0f;


    // Update is called once per frame
    void Update()
    {
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;

        //MOVE
        if (x <= 0 + offset)
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + movement_speed * Time.deltaTime);      
        else if (x >= Screen.width - offset)
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - movement_speed * Time.deltaTime);

        if (y <= 0 + offset) 
            transform.position = new Vector3(transform.position.x - movement_speed * Time.deltaTime, transform.position.y, transform.position.z);
        else if (y >= Screen.height - offset)
            transform.position = new Vector3(transform.position.x + movement_speed * Time.deltaTime, transform.position.y, transform.position.z);
       
        //LIMIT
        if(transform.position.z > max_z)
            transform.position = new Vector3(transform.position.x, transform.position.y, max_z);
        else if (transform.position.z < min_z)
            transform.position = new Vector3(transform.position.x, transform.position.y, min_z);

        if (transform.position.x > max_x)
            transform.position = new Vector3(max_x, transform.position.y, transform.position.z);
        else if (transform.position.x < min_x)
            transform.position = new Vector3(min_x, transform.position.y, transform.position.z);
    }
}
