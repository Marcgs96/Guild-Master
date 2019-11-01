using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class Move : MonoBehaviour {

	public GameObject target;
	public GameObject aim;
//	public Slider arrow;
	public float max_mov_speed = 5.0f;
	public float max_mov_acceleration = 0.1f;
	public float max_rot_speed = 10.0f; // in degrees / second
	public float max_rot_acceleration = 0.1f; // in degrees

    public Vector3[] movement_velocity;
    public float[] rotation_velocity;

    [Header("-------- Read Only --------")]
	public Vector3 current_velocity = Vector3.zero;
	public float current_rotation_speed = 0.0f; // degrees

    void Start()
    {
        movement_velocity = new Vector3[SteeringConf.num_priorities];
        rotation_velocity = new float[SteeringConf.num_priorities];
    }
	// Methods for behaviours to set / add velocities
	public void SetMovementVelocity (Vector3 velocity) 
	{
        current_velocity = velocity;
	}

	public void AccelerateMovement (Vector3 acceleration, int prio) 
	{
        movement_velocity[prio] += acceleration;
	}

	public void SetRotationVelocity (float rotation_speed) 
	{
        current_rotation_speed = rotation_speed;
	}

	public void AccelerateRotation (float rotation_acceleration, int prio) 
	{
        rotation_velocity[prio] += rotation_acceleration;
	}
	
	// Update is called once per frame
	void Update () 
	{
        for(int i = SteeringConf.num_priorities-1; i>=0; i--)
        {
            if(movement_velocity[i].magnitude > 0.0f)
            {
                current_velocity += movement_velocity[i];
                break;
            }
        }

        for (int i = SteeringConf.num_priorities - 1; i >= 0; i--)
        {
            if (rotation_velocity[i] != 0.0f)
            {
                current_rotation_speed += rotation_velocity[i];
                break;
            }
        }

        // cap velocity
        if (current_velocity.magnitude > max_mov_speed)
		{
            current_velocity = current_velocity.normalized * max_mov_speed;
		}

        // cap rotation
        current_rotation_speed = Mathf.Clamp(current_rotation_speed, -max_rot_speed, max_rot_speed);

		// rotate the arrow
		float angle = Mathf.Atan2(current_velocity.x, current_velocity.z);
        //aim.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);

        // strech it
        //arrow.value = current_velocity.magnitude * 4;

        // final rotate
        Debug.Log("rotation speed " + current_rotation_speed);
		transform.rotation *= Quaternion.AngleAxis(current_rotation_speed * Time.deltaTime, Vector3.up);

		// finally move
		transform.position += current_velocity * Time.deltaTime;


        for (int i = SteeringConf.num_priorities - 1; i > 0; i--)
        {
            movement_velocity[i] = Vector3.zero;
        }
        for (int i = SteeringConf.num_priorities - 1; i >= 0; i--)
        {
            rotation_velocity[i] = 0.0f;
        }
    }
}
