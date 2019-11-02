using UnityEngine;
using System.Collections;

[System.Serializable]
public class OAray
{
    public float length;
    public Vector3 direction;
}

public class SteeringObstacleAvoidance : Steering
{

    public LayerMask obstacle_layer;
    public float avoid_distance = 5.0f;
    public OAray[] rays;

    Move move;
    SteeringSeek seek;

    // Use this for initialization
    void Start () {
        move = GetComponent<Move>(); 
        seek = GetComponent<SteeringSeek>();
    }
    
    // Update is called once per frame
    void Update () 
    {
        float angle = Mathf.Atan2(move.current_velocity.x, move.current_velocity.z);
        Quaternion q = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);

        foreach (OAray r in rays)
        {
            RaycastHit hit;

            if (Physics.Raycast(new Vector3(transform.position.x, 1.0f, transform.position.z), q * r.direction.normalized, out hit, r.length, obstacle_layer) == true)
            {
                seek.Steer(new Vector3(hit.point.x, transform.position.y, hit.point.z) + hit.normal * avoid_distance);
                Debug.Log("obstacle hitted by raycast, moving away");
            }
        }
    }

    void OnDrawGizmosSelected() 
    {
        if(move && this.isActiveAndEnabled)
        {
            // Display the explosion radius when selected
            Gizmos.color = Color.red;
            float angle = Mathf.Atan2(move.current_velocity.x, move.current_velocity.z);
            Quaternion q = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);

            foreach(OAray ray in rays)
                Gizmos.DrawLine(transform.position, transform.position + (q * ray.direction.normalized) * ray.length);
        }
    }
}

