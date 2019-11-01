using UnityEngine;
using System.Collections;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;

public class SteeringFollowPath : Steering
{
	Move move;
	SteeringSeek seek;
    public BGCcMath path;
    Vector3 closest_point;

    public float ratio_increment = 0.1f;
    public float min_distance = 1.0f;
    float current_ratio = 0.0f;

	// Use this for initialization
	void Start () {
		move = GetComponent<Move>();
		seek = GetComponent<SteeringSeek>();

        // TODO 1: Calculate the closest point from the tank to the curve
        float distance;
        closest_point = path.CalcPositionByClosestPoint(transform.position, out distance);
        current_ratio = distance / path.Curve.Points.Length;
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (Vector3.Distance(transform.position, closest_point) <= min_distance)
        {
            current_ratio += ratio_increment;
            if (current_ratio > 1)
                current_ratio = 0;
            closest_point = path.CalcPositionByDistanceRatio(current_ratio);
        }

        seek.Steer(closest_point);
        // TODO 2: Check if the tank is close enough to the desired point
        // If so, create a new point further ahead in the path
    }

	void OnDrawGizmosSelected() 
	{

		if(isActiveAndEnabled)
		{
			// Display the explosion radius when selected
			Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(closest_point, 5);
            // Useful if you draw a sphere were on the closest point to the path
        }

	}
}
