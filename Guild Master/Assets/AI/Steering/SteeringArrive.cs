﻿using UnityEngine;
using System.Collections;

public class SteeringArrive : Steering {

    public float min_distance = 0.1f;
	public float slow_distance = 5.0f;
	public float time_to_target = 0.1f;

	Move move;

	// Use this for initialization
	void Start () { 
		move = GetComponent<Move>();
	}

	// Update is called once per frame
	void Update () 
	{
		Steer(move.target.transform.position);
	}

	public bool Steer(Vector3 target)
	{
        // PROFE
        /* if (!move)
             move = GetComponent<Move>();

          Velocity we are trying to match
         float ideal_speed = 0.0f;
         Vector3 diff = target - transform.position;

         if (diff.magnitude < min_distance)
         {
             move.SetMovementVelocity(Vector3.zero);
             return;
         }

          Decide which would be our ideal velocity
         if (diff.magnitude > slow_distance)
             ideal_speed = move.max_mov_speed;
         else
             ideal_speed = move.max_mov_speed * (diff.magnitude / slow_distance);

          Create a vector that describes the ideal velocity
         Vector3 ideal_movement = diff.normalized * ideal_speed;

          Calculate acceleration needed to match that velocity
         Vector3 acceleration = ideal_movement - move.current_velocity;
         acceleration /= time_to_target;

          Cap acceleration
         if (acceleration.magnitude > move.max_mov_acceleration)
         {
             acceleration = acceleration.normalized * move.max_mov_acceleration;
         }

         move.AccelerateMovement(acceleration);*/


        if (!move)
            move = GetComponent<Move>();

        Vector3 diff = target - transform.position;

        if (diff.magnitude < min_distance)
        {
            move.SetMovementVelocity(Vector3.zero,priority);
            return true;
        }

        Vector3 wanted_velocity = diff.normalized * move.max_mov_velocity;
        if (diff.magnitude <= slow_distance)
        {
            wanted_velocity *= diff.magnitude / slow_distance;
        }

        Vector3 desired_acceleration = (wanted_velocity - move.movement) / time_to_target;
        Vector3 clamped_acceleration = desired_acceleration.normalized * Mathf.Clamp(desired_acceleration.magnitude, 0.0f, move.max_mov_acceleration);
        move.AccelerateMovement(clamped_acceleration, priority);

        return false;
    }

	void OnDrawGizmosSelected() 
	{
		// Display the explosion radius when selected
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, min_distance);

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, slow_distance);
	}
}
