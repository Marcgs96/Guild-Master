﻿using UnityEngine;
using System.Collections;

public class SteeringFlee : Steering {

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

	public void Steer(Vector3 target)
	{
        Vector3 diff = target - transform.position;
        diff.Normalize();
        diff *= move.max_mov_acceleration;

        move.AccelerateMovement(-diff, priority);
	}
}
