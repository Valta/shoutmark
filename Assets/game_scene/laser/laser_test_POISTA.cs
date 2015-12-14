using UnityEngine;
using System.Collections;

public class laser_test_POISTA : MonoBehaviour
{


	void Start()
	{
	
	}


	void Update()
	{
		transform.RotateAround(transform.position, new Vector3(0.0f, 1.0f, 0.0f), _TIMER.deltatime() * 20.0f);
		if (Input.GetKeyDown(KeyCode.Y))
		{
			laser_script laser = gameObject.GetComponent<laser_script>();
			laser.shoot(transform.position, transform.rotation.eulerAngles.y);
		}
	}
	
	
}
