using UnityEngine;
using System.Collections;

public class background_image_script : MonoBehaviour
{
	private const float BACKGROUND_SPEED = 0.4f;
	
	private GameObject gamecamera;
	private Vector3 starting_position;
	private Vector3 camera_starting_position;



	void Start()
	{
		gamecamera = GameObject.Find("Main Camera");
		starting_position = transform.position;
		camera_starting_position = gamecamera.transform.position;
	}


	void Update()
	{
		// make the background image follow the camera:
		transform.position =
			starting_position + (gamecamera.transform.position - camera_starting_position) * BACKGROUND_SPEED;
	}
}
