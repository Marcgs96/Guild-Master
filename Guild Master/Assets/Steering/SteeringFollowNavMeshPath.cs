using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;


public class SteeringFollowNavMeshPath : Steering
{
    CurveManager curve_manager;

    Move move;
    SteeringArrive arrive;
    SteeringSeek seek;
    NavMeshAgent agent;

    GameObject curve_obj;
    BGCcMath path;
    BGCurve curve;

    Vector3 closest_point;

    public float ratio_increment = 0.1f;
    public float min_distance = 1.0f;
    float current_ratio = 0.0f;
    bool new_path = false;

    int current_point = 0;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<Move>();
        arrive = GetComponent<SteeringArrive>();
        seek = GetComponent<SteeringSeek>();
        agent = GetComponent<NavMeshAgent>();

        curve_manager = GameObject.Find("Curve Manager").GetComponent<CurveManager>();
        curve_obj = curve_manager.CreateCurve();

        curve = curve_obj.GetComponent<BGCurve>();
        path = curve_obj.GetComponent<BGCcMath>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            MoveTo(move.target.transform.position);
        }

        /*if(agent.hasPath && new_path)
        {
              curve_manager.SetCurve(curve, agent.path, transform.position);


              float distance;
              closest_point = path.CalcPositionByClosestPoint(transform.position, out distance);
              current_ratio = distance / path.Curve.Points.Length;
              new_path = false;
        }
        if (curve.PointsCount > 0)
        {
            if (closest_point != curve.Points[curve.PointsCount - 1].PositionWorld)
            {
                if (Vector3.Distance(transform.position, closest_point) <= min_distance)
                {
                    current_ratio += ratio_increment;
                    if (current_ratio > 1)
                        current_ratio = 0;
                    closest_point = path.CalcPositionByDistanceRatio(current_ratio);
                }
                seek.Steer(closest_point);
            }
            else
            {
                Debug.Log("ENTRO");
                arrive.Steer(closest_point);
            }        

        }*/
       
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
        new_path = true;
    }
}
