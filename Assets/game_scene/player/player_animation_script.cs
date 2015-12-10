using UnityEngine;
using System.Collections;

public class player_animation_script : MonoBehaviour
{
	private const float SPEED = 10.0f;
	private Transform body;
	private Transform ear1;
	private Transform ear2;
	private Transform ass_ball;
	
	private Vector3 previous_position;
	private float current_speed;
	private float current_direction;
	private float timer = 0.0f;



	void Start()
	{
		body = transform;
		ear1 = transform.FindChild("bunny_ear_parent_1");
		ear2 = transform.FindChild("bunny_ear_parent_2");
		ass_ball = transform.FindChild("tail");
		previous_position = transform.position;
		current_speed = 0.0f;
		current_direction = 0.0f;
		timer = 0.0f;
	}



	void Update()
	{
		calculate_speed();
		calculate_direction();
		set_previous_position();
		animate_bodyparts();
		
		transform.localRotation = Quaternion.Euler(0.0f, -current_direction, 0.0f);
	}



	private void calculate_speed()
	{
		float x = transform.position.x;
		float z = transform.position.z;
		float speed = Mathf.Sqrt((previous_position.x - x) * (previous_position.x - x) +
			(previous_position.z - z) * (previous_position.z - z)) / Mathf.Max(Time.deltaTime, 0.001f);
		current_speed = 0.8f * current_speed + 0.2f * speed;
		if (current_speed < 0.2f) timer = 0.0f; else timer += Time.deltaTime;
	}



	private void calculate_direction()
	{
		float x = transform.position.x;
		float z = transform.position.z;
		float direction = ENEMY_MATH.get_absolute_angle(x - previous_position.x, z - previous_position.z);

		if (current_speed > 2.0f)
		{
			if (direction > 180.0f) direction -= 360.0f;
			float cd = current_direction - direction;
			while (cd > 180.0f) cd -= 360.0f;
			while (cd < -180.0f) cd += 360.0f;
			float increment = 200.0f * Time.deltaTime;
			
			if (cd > 0.0f) increment *= -1.0f;
			if (cd > 50.0f || cd < -50.0f) increment *= 4.0f;
			if (cd > -3.0f && cd < 3.0f) increment = 0.0f;
			current_direction += increment;
		}
	}



	private void set_previous_position()
	{
		previous_position = transform.position;
	}



	private void animate_bodyparts()
	{
		body.localPosition = new Vector3(	0.0f,
											0.5f + 0.6f * Mathf.Abs(Mathf.Sin(timer * SPEED)),
											0.0f);
		ass_ball.localPosition = new Vector3(	-0.72f,
												-0.44f + 0.6f * Mathf.Abs(Mathf.Sin(timer * SPEED - 1.0f)),
												0.0f);
		ear1.localPosition = new Vector3(	0.11f,
											0.8f + 0.3f * Mathf.Abs(Mathf.Sin(timer * SPEED - 0.6f)),
											-0.27f);
		ear2.localPosition = new Vector3(	0.1f,
											0.8f + 0.35f * Mathf.Abs(Mathf.Sin(timer * SPEED - 0.6f)),
											0.31f);
	}
}
