using UnityEngine;
using System.Collections;

public class SteeringAlign : Steering
{

	public float min_angle = 0.01f;
	public float slow_angle = 0.1f;
	public float time_to_accel = 0.1f;

	Move move;

	// Use this for initialization
	void Start () {
		move = GetComponent<Move>();
	}

	// Update is called once per frame
	void Update () 
	{
    }

    public void Steer(Vector3 target)
    {
        // Orientation we are trying to match
        /*float delta_angle = Vector3.SignedAngle(transform.forward, target - transform.position, new Vector3(0.0f, 1.0f, 0.0f));

        float diff_absolute = Mathf.Abs(delta_angle);

        if(diff_absolute < min_angle)
        {
            move.SetRotationVelocity(0.0f);
            return;
        }

        float ideal_rotation_speed = move.max_rot_speed;

        if (diff_absolute < slow_angle)
            ideal_rotation_speed *= (diff_absolute / slow_angle);

        float angular_acceleration = ideal_rotation_speed / time_to_accel;

        //Invert rotation direction if the angle is negative
        if(delta_angle < 0)
            angular_acceleration = -angular_acceleration;

        move.AccelerateRotation(Mathf.Clamp(angular_acceleration, -move.max_rot_acceleration, move.max_rot_acceleration), priority);*/


        float angle = Vector3.SignedAngle(transform.forward, target - transform.position, Vector3.up);

        if (Mathf.Abs(angle) < min_angle)
        {
            move.SetRotationVelocity(0);
            return;
        }

        float angular_acceleration = move.max_rot_speed / time_to_accel;

        if (angle < 0.0f)
            angular_acceleration = -angular_acceleration;

        float clamped_acceleration = Mathf.Clamp(angular_acceleration, -move.max_rot_acceleration, move.max_rot_acceleration);
        move.AccelerateRotation(clamped_acceleration, priority);
    }
}
