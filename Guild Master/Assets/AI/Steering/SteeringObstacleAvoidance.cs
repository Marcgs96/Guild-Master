//using UnityEngine;
//using System.Collections;

//[System.Serializable]
//public class my_ray
//{
//    public float length = 2.0f;
//    public Vector3 direction = Vector3.forward;
//}

//public class SteeringObstacleAvoidance : Steering
//{

//    public LayerMask mask;
//    public float avoid_distance = 5.0f;
//    public my_ray[] rays;

//    Move move;
//    SteeringSeek seek;

//    // Use this for initialization
//    void Start () {
//        move = GetComponent<Move>(); 
//        seek = GetComponent<SteeringSeek>();
//    }
    
//    // Update is called once per frame
//    void Update () 
//    {
//        float angle = Mathf.Atan2(move.current_velocity.x, move.current_velocity.z);
//        Quaternion q = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);

//        foreach (my_ray ray in rays)
//        {
//            RaycastHit hit;

//            if (Physics.Raycast(new Vector3(transform.position.x, 1.0f, transform.position.z), q * ray.direction.normalized, out hit, ray.length, mask) == true)
//            {
//                seek.Steer(new Vector3(hit.point.x, transform.position.y, hit.point.z) + hit.normal * avoid_distance);
//                Debug.Log("obstacle hitted by raycast, moving away");
//            }
//        }
//    }

//    void OnDrawGizmosSelected() 
//    {
//        if(move && this.isActiveAndEnabled)
//        {
//            // Display the explosion radius when selected
//            Gizmos.color = Color.red;
//            float angle = Mathf.Atan2(move.current_velocity.x, move.current_velocity.z);
//            Quaternion q = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);

//            foreach(my_ray ray in rays)
//                Gizmos.DrawLine(transform.position, transform.position + (q * ray.direction.normalized) * ray.length);
//        }
//    }
//}

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
    void Start()
    {
        move = GetComponent<Move>();
        seek = GetComponent<SteeringSeek>();
    }

    // Update is called once per frame
    void Update()
    {
        /* foreach (OAray r in rays)
         {
             RaycastHit hit;

             if (Physics.Raycast(transform.position, transform.rotation * r.direction.normalized, out hit, r.length, obstacle_layer) == true)
             {
                 seek.Steer(new Vector3(hit.point.x, transform.position.y, hit.point.z) + hit.normal * avoid_distance);
             }
         }*/

        Collider[] colliders = Physics.OverlapSphere(transform.position, avoid_distance, obstacle_layer);
        //Vector3 final = Vector3.zero;

        // collision data
        GameObject target = null;
        float target_shortest_time = float.PositiveInfinity;
        float target_min_separation = 0.0f;
        float target_distance = 0.0f;
        Vector3 target_relative_pos = Vector3.zero;
        Vector3 target_relative_vel = Vector3.zero;

        foreach (Collider col in colliders)
        {
            GameObject go = col.gameObject;

            if (go == gameObject)
                continue;

            Move target_move = go.GetComponent<Move>();

            if (target_move == null)
                continue;

            // calculate time to collision
            Vector3 relative_pos = go.transform.position - transform.position;
            Vector3 relative_vel = target_move.movement - move.movement;
            float relative_speed = relative_vel.magnitude;
            float time_to_collision = Vector3.Dot(relative_pos, relative_vel) / relative_speed * relative_speed;

            // make sure there is a collision at all
            float distance = relative_pos.magnitude;
            float min_separation = distance - relative_speed * time_to_collision;
            if (min_separation > 2.0f * avoid_distance)
                continue;

            if (time_to_collision > target_shortest_time)
                continue;

            Debug.Log("Avoiding " + go.name);
            target = go;
            target_shortest_time = time_to_collision;
            target_min_separation = min_separation;
            target_distance = distance;
            target_relative_pos = relative_pos;
            target_relative_vel = relative_vel;
        }

        //if we have a target, avoid collision
        if (target != null)
        {
            Vector3 escape_pos;
            if (target_min_separation <= 0.0f || target_distance < avoid_distance * 2.0f)
                escape_pos = target.transform.position - transform.position;
            else
                escape_pos = target_relative_pos + target_relative_vel * target_shortest_time;

            move.AccelerateMovement(-(escape_pos.normalized * move.max_mov_acceleration), priority);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (move && this.isActiveAndEnabled)
        {
            // Display the explosion radius when selected
            Gizmos.color = Color.red;
            float angle = Mathf.Atan2(move.movement.x, move.movement.z);
            Quaternion q = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);

            foreach (OAray r in rays)
                Gizmos.DrawLine(transform.position, transform.position + (transform.rotation * r.direction.normalized) * r.length);
        }
    }
}


