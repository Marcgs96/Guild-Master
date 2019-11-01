using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;


public class SteeringFollowNavMeshPath : Steering
{
    public Transform location;

    Move move;
    SteeringArrive arrive;
    SteeringSeek seek;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<Move>();
        arrive = GetComponent<SteeringArrive>();
        seek = GetComponent<SteeringSeek>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            MoveTo(location.position);
        }
       
        if (agent.hasPath)
        {
            Debug.Log(agent.path.corners.Length);
            if (agent.path.corners.Length > 2)
            {
                Debug.Log("seek");
                seek.Steer(agent.path.corners[1]);
            }
            else
            {
                Debug.Log("arrive");
                arrive.Steer(agent.path.corners[1]);
            }
        }
    }

    void MoveTo(Vector3 pos)
    {
        Debug.Log("Setting path");
        agent.ResetPath();
        agent.SetDestination(pos);
    }
}
