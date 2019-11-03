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
    bool once = false;

    public delegate void ReachAction();
    public event ReachAction OnReachEnd;


    void Awake()
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
                    if (!once)
                    {
                        OnReachEnd();
                        once = true;
                    }
                }
            }
        }
    }

    public void CreatePath(Vector3 pos)
    {
        current_point = 1;
        once = false;
        NavMesh.CalculatePath(transform.position, pos, (1 << NavMesh.GetAreaFromName("Walkable")) | (1 << NavMesh.GetAreaFromName("OffRoad")), path);
    }

    public Vector3 GetPathPosition()
    {
        if(path.status == NavMeshPathStatus.PathComplete)
        {
            return path.corners[current_point];
        }

        return Vector3.zero;
    }
}
