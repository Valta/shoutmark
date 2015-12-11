using UnityEngine;
using System.Collections;

public class enemy_graphics_script : MonoBehaviour
{
	private Transform body;
	private Transform head;
	private Transform left_leg;
	private Transform right_leg;
	private Transform left_gun;
	private Transform right_gun;
	
	private float timer = 0.0f;
	private Vector3 previous_position;
	private float current_speed;
	private float current_direction;
	private bool running;


	void Start()
	{
		body = transform;
		head = transform.FindChild("head");
		left_leg = transform.FindChild("left_leg_parent");
		right_leg = transform.FindChild("right_leg_parent");
		left_gun = transform.FindChild("gun_l");
		right_gun = transform.FindChild("gun_r");
		
		previous_position = transform.position;
		current_speed = 0.0f;
		current_direction = 0.0f;
		running = false;
	}



	void Update()
	{
		timer += Time.deltaTime;
		
		calculate_speed();
		calculate_direction();
		set_previous_position();
		animate_bodyparts();
		//animate_idle();
		//Debug.Log("ENEMY= speed="+current_speed);
		
		if (Input.GetKeyDown(KeyCode.S)) shoot();
		
		transform.localRotation = Quaternion.Euler(0.0f, -current_direction, 0.0f);
	}



	public void shoot()
	{
		Debug.Log("SHOOT");
		head.localPosition = new Vector3(0.0f, 3.0f, 0.0f);
	}


	private void calculate_speed()
	{
		float x = transform.position.x;
		float z = transform.position.z;
		float speed = Mathf.Sqrt((previous_position.x - x) * (previous_position.x - x) +
			(previous_position.z - z) * (previous_position.z - z)) / Mathf.Max(Time.deltaTime, 0.001f);
		current_speed = 0.8f * current_speed + 0.2f * speed;
		
		if (current_speed < 0.2f)
		{
			if (running)
			{
				timer = 0.0f;
				running = false;
			}
		}
		else
		{
			if (!running)
			{
				timer = 0.0f;
				running = true;
			}
		}
		
	}
	
	
	
	private void calculate_direction()
	{
		float x = transform.position.x;
		float z = transform.position.z;
		float direction = ENEMY_MATH.get_absolute_angle(x - previous_position.x, z - previous_position.z);
		
		if (current_speed > 0.7f)
		{
			if (direction > 180.0f) direction -= 360.0f;
			float cd = current_direction - direction;
			while (cd > 180.0f) cd -= 360.0f;
			while (cd < -180.0f) cd += 360.0f;
			float increment = 200.0f * Time.deltaTime;
			
			if (cd > 0.0f) increment *= -1.0f;
			if (cd > 80.0f || cd < -80.0f) increment *= 2.0f;
			if (cd > -10.0f && cd < 10.0f) increment *= cd * 0.1f;
			current_direction += increment;
		}
	}



	private void set_previous_position()
	{
		previous_position = transform.position;
	}



	private void animate_bodyparts()
	{
		const float SPEED = 9.0f;
		
		head.localPosition = new Vector3(0.0f, -0.12f + 0.20f * Mathf.Abs(Mathf.Sin(timer * SPEED)), 0.0f);
		head.localRotation = Quaternion.Euler(0.0f, 180.0f + 16.0f * Mathf.Sin(timer * SPEED), 0.0f);
		left_gun.localRotation = Quaternion.Euler(-8.1f, 180.0f + 8.0f * Mathf.Sin(timer * SPEED), 90.0f);
		left_gun.localPosition = new Vector3(0.143f, -0.5f + 0.17f * Mathf.Abs(Mathf.Sin(timer * SPEED - 1.0f)), -0.3f);
		right_gun.localRotation = Quaternion.Euler(-8.1f, 180.0f + 8.0f * Mathf.Sin(timer * SPEED), 90.0f);
		right_gun.localPosition = new Vector3(0.143f, -0.5f + 0.17f * Mathf.Abs(Mathf.Sin(timer * SPEED - 1.0f)), 0.3f);
		
		left_leg.localRotation = Quaternion.Euler(0.0f, 0.0f, 80.0f * Mathf.Sin(timer * SPEED));
		right_leg.localRotation = Quaternion.Euler(0.0f, 0.0f, -80.0f * Mathf.Sin(timer * SPEED));
	}



	/*
	private void animate_idle()
	{
		if (!running)
		{
			//body//.transform.localScale = 
		}
	}
	*/
}
