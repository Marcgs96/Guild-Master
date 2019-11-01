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
    NavMeshAgent agent;

    public bool arrived;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<Move>();
        arrive = GetComponent<SteeringArrive>();
        seek = GetComponent<SteeringSeek>();
        align = GetComponent<SteeringAlign>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {    
        if (agent.hasPath)
        {
            if (agent.path.corners.Length > 2)
            {
                seek.Steer(agent.path.corners[1]);
            }
            else
            {
                if (arrive.Steer(agent.path.corners[1]))
                    arrived = true;
            }

            align.Steer(agent.path.corners[1]);
        }
    }

    public void CreatePath(Vector3 pos)
    {
        arrived = false;
        agent.ResetPath();
        agent.SetDestination(pos);
    }
}
