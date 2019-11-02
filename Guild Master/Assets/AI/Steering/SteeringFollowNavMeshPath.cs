using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;


public class SteeringFollowNavMeshPath : Steering
{
    Move move;
    SteeringArrive arrive;
    SteeringSeek seek;
    SteeringAlign align;
    NavMeshPath path;

    public float min_distance;
    int current_point = 1;


    public delegate void ReachAction();
    public event ReachAction OnReachEnd;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<Move>();
        arrive = GetComponent<SteeringArrive>();
        seek = GetComponent<SteeringSeek>();
        align = GetComponent<SteeringAlign>();

        path = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {    
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            align.Steer(path.corners[current_point]);

            if (current_point != path.corners.Length - 1)
            {
                seek.Steer(path.corners[current_point]);
                if (Vector3.Distance(transform.position, path.corners[current_point]) < min_distance)
                    current_point++;
            }
            else
            {
                if (arrive.Steer(path.corners[current_point]))
                {
                    current_point = 1;
                    OnReachEnd();
                    path.ClearCorners();
                }
            }
        }
    }

    public void CreatePath(Vector3 pos)
    {
        path.ClearCorners();
        NavMesh.CalculatePath(transform.position, pos, 1 << NavMesh.GetAreaFromName("Walkable"), path);
    }
}
