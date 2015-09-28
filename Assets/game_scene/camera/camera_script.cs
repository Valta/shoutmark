﻿using UnityEngine;
using System.Collections;

public class camera_script : MonoBehaviour
{
	private const float CAMERA_TARGET_RADIUS = 6.0f;
	
	public GameObject TEST_CUBE;
	public GameObject TEST_QUAD;
	public GameObject TEST_UI_IMAGE;
	
	private Vector3 camera_target;
	private float screen_width = 0;
	private float screen_height = 0;
	private float touch_x = 0.0f;
	private float touch_y = 0.0f;



	void Start()
	{
		camera_target = transform.position;
		get_screen_size();
	}



	void Update()
	{
		move_towards_target();
		
				if (Input.GetKeyDown(KeyCode.Q)) MESSAGE.print("a"+(char)16+"b c d e f", -160, 80, 2, 66);
				if (Input.GetKeyDown(KeyCode.W)) MESSAGE.print("abcdefhilkjmop123", -160, 80, 2, 66);
				if (Input.GetKeyDown(KeyCode.E)) MESSAGE.print("ABCDEF", 30, 30, 2, 67);
				if (Input.GetKeyDown(KeyCode.R)) MESSAGE.print("ABCDEFGHIJKL", 30, 30, 2, 67);
				if (Input.GetKeyDown(KeyCode.T)) MESSAGE.report("säätänän päskä", 7);
				if (Input.GetKeyDown(KeyCode.Y)) MESSAGE.report("PELAA NES JUO ES \"!#¤%&&/\\", 9);
		
		{
			calculate_touch_position();
			TEST_QUAD.transform.position = new Vector3(Mathf.Floor(touch_x) + 0.5f, TEST_QUAD.transform.position.y, Mathf.Floor(touch_y) + 0.5f);
			TEST_CUBE.transform.position = new Vector3(touch_x, TEST_CUBE.transform.position.y, touch_y);
		}
	}



	private void get_screen_size()
	{
		screen_width = Screen.width;
		screen_height = Screen.height;
	}



	private void calculate_touch_position()
	{
		float angle_in_radius = (transform.rotation.eulerAngles.y + 0.0f) * 3.14159f / 180.0f;
		
		float CAMERA_CENTER_X = transform.position.x + Mathf.Sin(angle_in_radius) * (CAMERA_TARGET_RADIUS + 0.3f);
		float CAMERA_CENTER_Y = transform.position.z + Mathf.Cos(angle_in_radius) * (CAMERA_TARGET_RADIUS + 0.3f);
		float CAMERA_SIZE = transform.GetComponent<Camera>().orthographicSize;
		
		float x_on_screen = (Input.mousePosition.x - screen_width * 0.5f);
		float y_on_screen = (Input.mousePosition.y - screen_height * 0.5f);
		
		x_on_screen = x_on_screen / (screen_height * 0.5f);
		y_on_screen = y_on_screen / (screen_height * 0.5f);
		x_on_screen *= CAMERA_SIZE;
		y_on_screen *= CAMERA_SIZE * 1.6429f;
		
		touch_x = CAMERA_CENTER_X + (x_on_screen) * Mathf.Cos(-angle_in_radius) + (-y_on_screen) * Mathf.Sin(-angle_in_radius);
		touch_y = CAMERA_CENTER_Y + (y_on_screen) * Mathf.Cos(-angle_in_radius) + (x_on_screen) * Mathf.Sin(-angle_in_radius);
	}



	public void set_camera_target(float x, float y, bool set_instantly)
	{
		float angle_in_radius = (transform.rotation.eulerAngles.y + 0.0f) * 3.14159f / 180.0f;
		
		camera_target.x = x - Mathf.Sin(angle_in_radius) * CAMERA_TARGET_RADIUS;
		camera_target.z = y - Mathf.Cos(angle_in_radius) * CAMERA_TARGET_RADIUS;
		
		if (set_instantly) transform.position = camera_target;
	}



	private void move_towards_target()
	{
		float SPEED = _TIMER.deltatime() * 2.0f;
		transform.position = transform.position + (camera_target - transform.position) * SPEED;
	}
}
