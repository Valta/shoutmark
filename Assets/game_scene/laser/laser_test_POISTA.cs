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

/*
			float touch_x = gamecamera.get_touch_x();
			float touch_y = gamecamera.get_touch_y();
			float distance_to_touch = Mathf.Sqrt((x - touch_x) * (x - touch_x) + (y - touch_x) * (y - touch_x));
			if (Mathf.Abs(touch_x - x) > PLAYER_RADIUS && Mathf.Abs(touch_y - y) > PLAYER_RADIUS)
			{
				speed_x = (touch_x - x) * speed * distance_to_touch;
				speed_y = (touch_y - y) * speed * distance_to_touch;
			}
			//Debug.Log("speedx="+speed_x+"   speedy0"+speed_y);

*/
