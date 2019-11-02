﻿using UnityEngine;
using System.Collections;

public class SteeringSeparation : Steering {

	public LayerMask mask;
	public float search_radius = 5.0f;
    public AnimationCurve strength;

    Move move;

	// Use this for initialization
	void Start () {
		move = GetComponent<Move>();
	}

	// Update is called once per frame
    void Update () 
    {
        //PROFE
        /*Collider[] colliders = Physics.OverlapSphere(transform.position, search_radius, mask);
        Vector3 final = Vector3.zero;

        foreach(Collider col in colliders)
        {
            GameObject go = col.gameObject;

            if(go == gameObject) 
                continue;

            Vector3 diff = transform.position - go.transform.position;
            float distance = diff.magnitude;
            float acceleration = (1.0f - falloff.Evaluate(distance / search_radius)) * move.max_mov_acceleration;

            final += diff.normalized * acceleration;
        }

        float final_strength = final.magnitude;
        if(final_strength > 0.0f)
        {
            if(final_strength > move.max_mov_acceleration)
                final = final.normalized * move.max_mov_acceleration;
            move.AccelerateMovement(final, priority);
        }*/


        Collider[] hit_agents = Physics.OverlapSphere(transform.position, search_radius, mask.value);
        Vector3 sum_vector = Vector3.zero;
      
        foreach (Collider c in hit_agents)
        {
            if (c.gameObject != gameObject)
            {
                Vector3 direction = -Vector3.Normalize(c.transform.position - transform.position);
                sum_vector += direction * strength.Evaluate(direction.magnitude/search_radius);
            }
        }
        Vector3 result = Vector3.Normalize(sum_vector) * (Mathf.Clamp(sum_vector.magnitude, 0, move.max_mov_acceleration));
        move.AccelerateMovement(result, priority);

    }

	void OnDrawGizmosSelected() 
	{
		// Display the explosion radius when selected
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, search_radius);
	}
}
