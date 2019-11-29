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
    SteeringSeparation separation;
    NavMeshPath path;

    public float min_distance;
    int current_point = 1;
    bool reached = false;

    public delegate void ReachAction();
    public event ReachAction OnReachEnd;


    void Awake()
    {
        move = GetComponent<Move>();
        arrive = GetComponent<SteeringArrive>();
        seek = GetComponent<SteeringSeek>();
        align = GetComponent<SteeringAlign>();
        separation = GetComponent<SteeringSeparation>();

        path = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (path.corners.Length > 0 && !reached)
        {
            align.Steer(path.corners[current_point]);
            separation.Steer();

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
                    reached = true;
                }
            }
        }
    }

    public bool CreatePath(Vector3 pos)
    {
        Vector3 distance = pos - transform.position;

        if (distance.magnitude < min_distance)
        {
            reached = true;
            return false;
        }

        current_point = 1;
        reached = false;
        NavMesh.CalculatePath(transform.position, pos, (1 << NavMesh.GetAreaFromName("Walkable")) | (1 << NavMesh.GetAreaFromName("OffRoad")), path);

        return true;
    }

    public Vector3 GetPathPosition()
    {
        if(path.status == NavMeshPathStatus.PathComplete)
        {
            return path.corners[current_point];
        }

        return Vector3.zero;
    }

    public bool ReachedDestination()
    {
        return reached;
    }

    public void ClearPath()
    {
        move.SetMovementVelocity(new Vector3(0, 0, 0));
        path.ClearCorners();
    }
}
