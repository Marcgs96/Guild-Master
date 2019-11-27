using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class SteeringWander : Steering
{

	public Vector3 offset = Vector3.zero;
	public float radius = 1.0f;

    SteeringFollowNavMeshPath steer;
	Vector3 random_point;

	// Use this for initialization
	void Start () {
        steer = GetComponent<SteeringFollowNavMeshPath>();
	}

    // Update is called once per frame
    public void GeneratePoint() 
	{
        while (true)
        {
            random_point = Random.insideUnitSphere;
            random_point *= radius;
            random_point += transform.position + offset;
            random_point.y = transform.position.y;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(random_point, out hit, float.PositiveInfinity, (1 << NavMesh.GetAreaFromName("OffRoad"))))
            {
                if(steer.CreatePath(hit.position))
                    return;
            }
        }
    }

	void OnDrawGizmosSelected() 
	{
		if(this.isActiveAndEnabled)
		{
			// Display the explosion radius when selected
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.TransformPoint(offset), radius);
		
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(random_point, 0.2f);
		}
	}
}
