using UnityEngine;
using System.Collections;

public class camera_script : MonoBehaviour
{
	private Vector3 camera_target;



	void Start()
	{
		camera_target = transform.position;
	}



	void Update()
	{
		move_towards_target();
		apply_keyboard_input(); // for testing purposes!
	}



	private void apply_keyboard_input()
	{
				if (Input.GetKeyDown(KeyCode.Q)) PRINT.report("message abc ABC 123");
		
		bool write_info = false;
		float speed = Time.deltaTime * 10.0f;
		
		if (Input.GetKey(KeyCode.I))
		{
			write_info = true;
			transform.position += new Vector3(0.0f, 0.0f, speed);
		}
		if (Input.GetKey(KeyCode.K))
		{
			write_info = true;
			transform.position += new Vector3(0.0f, 0.0f, -speed);
		}
		if (Input.GetKey(KeyCode.J))
		{
			write_info = true;
			transform.position += new Vector3(-speed, 0.0f, 0.0f);
		}
		if (Input.GetKey(KeyCode.L))
		{
			write_info = true;
			transform.position += new Vector3(speed, 0.0f, 0.0f);
		}
		if (Input.GetKey(KeyCode.A))
		{
			write_info = true;
			transform.GetComponent<Camera>().orthographicSize -= speed * 0.2f;
		}
		if (Input.GetKey(KeyCode.Z))
		{
			write_info = true;
			transform.GetComponent<Camera>().orthographicSize += speed * 0.2f;
		}
		
		if (write_info) Debug.Log("CAMERA POSITION="+transform.position+"  SIZE="+transform.GetComponent<Camera>().orthographicSize);
	}



	public void set_camera_target(float x, float y, bool set_instantly)
	{
		float angle_in_radius = (transform.rotation.eulerAngles.y + 0.0f) * 3.14159f / 180.0f;
		float TARGET_RADIUS = 6.0f;
		
		camera_target.x = x - Mathf.Sin(angle_in_radius) * TARGET_RADIUS;
		camera_target.z = y - Mathf.Cos(angle_in_radius) * TARGET_RADIUS;
		
		if (set_instantly) transform.position = camera_target;
	}



	private void move_towards_target()
	{
		float SPEED = Time.deltaTime * 2.0f;
		transform.position = transform.position + (camera_target - transform.position) * SPEED;
	}
}
