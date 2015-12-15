using UnityEngine;
using System.Collections;

public class player_script : MonoBehaviour
{
	private const float PLAYER_RADIUS = 0.3f;
	
	private float x = 0.0f;
	private float y = 0.0f;
	private float speed_x = 0.0f;
	private float speed_y = 0.0f;
	private int id = -1;
	
	private _TILEMAP tilemap;
	private camera_script gamecamera;
    private GameObject player_model;
	private particle_script particles;


	void Start()
	{
		x = transform.position.x;
		y = transform.position.z;
		tilemap = GameObject.Find("_GLOBAL_SCRIPTS").GetComponent<_TILEMAP>();
		particles = GameObject.Find("_GLOBAL_SCRIPTS").GetComponent<particle_script>();
		gamecamera = GameObject.Find("Main Camera").GetComponent<camera_script>();
        player_model = transform.FindChild("head").gameObject;

		id = tilemap.add_to_object_list(x, y, PLAYER_RADIUS);
		Debug.Log("kljladjhslöfkjasödlf id="+id);
		tilemap.set_player_id(id);
		gamecamera.set_camera_target(x, y, true);
	}



	void Update()
	{
		float speed = 4.0f * _TIMER.deltatime();
		speed_x = 0.0f;
		speed_y = 0.0f;
		
		if (Input.GetMouseButton(0))
		{
			float touch_x = gamecamera.get_touch_x();
			float touch_y = gamecamera.get_touch_y();
			float distance_to_touch = Mathf.Sqrt((x - touch_x) * (x - touch_x) + (y - touch_y) * (y - touch_y));
			if (Mathf.Abs(touch_x - x) > PLAYER_RADIUS || Mathf.Abs(touch_y - y) > PLAYER_RADIUS)
			{
				speed_x = (touch_x - x) * speed / distance_to_touch;
				speed_y = (touch_y - y) * speed / distance_to_touch;
			}
			//Debug.Log("speedx="+speed_x+"   speedy0"+speed_y);
		}
		
		if (Input.GetKey(KeyCode.UpArrow))
		{
			try_to_push_tile("UP");
			speed_y = speed;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			speed_y = -speed;
			try_to_push_tile("DOWN");
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			try_to_push_tile("LEFT");
			speed_x = -speed;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			try_to_push_tile("RIGHT");
			speed_x = speed;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			MESSAGE.print("test TEST", 0, 50, 15, 5, 77);
			MESSAGE.print("test TEST", 2, 60, 15, 30, 78);
		}
		
		move_player();
		gamecamera.set_camera_target(x, y, false);
		tilemap.update_object_data(id, x, y);
		
		if (Input.GetKeyDown(KeyCode.P)) _TIMER.set_pause(!_TIMER.paused());
	}



	private void move_player()
	{
		
		if (speed_y > 0.0f)
		{
			if (tilemap.can_move(x - PLAYER_RADIUS, y + PLAYER_RADIUS + speed_y) &&
				tilemap.can_move(x + PLAYER_RADIUS, y + PLAYER_RADIUS + speed_y)) y += speed_y;
			else
				speed_y = 0.0f;
		}
		if (speed_y < 0.0f)
		{
			if (tilemap.can_move(x - PLAYER_RADIUS, y - PLAYER_RADIUS + speed_y) &&
				tilemap.can_move(x + PLAYER_RADIUS, y - PLAYER_RADIUS + speed_y)) y += speed_y;
			else
				speed_y = 0.0f;
		}
		if (speed_x < 0.0f)
		{
			if (tilemap.can_move(x - PLAYER_RADIUS + speed_x, y + PLAYER_RADIUS) &&
				tilemap.can_move(x - PLAYER_RADIUS + speed_x, y - PLAYER_RADIUS)) x += speed_x;
			else
				speed_x = 0.0f;
		}
		if (speed_x > 0.0f)
		{
			if (tilemap.can_move(x + PLAYER_RADIUS + speed_x, y + PLAYER_RADIUS) &&
				tilemap.can_move(x + PLAYER_RADIUS + speed_x, y - PLAYER_RADIUS)) x += speed_x;
			else
				speed_x = 0.0f;
		}
		
		transform.position = new Vector3(x, 0.0f, y);
	}



	private void try_to_push_tile(string direction)
	{
		float PUSH_RADIUS = PLAYER_RADIUS - 0.1f;		// a bit smaller that player radius.
		float PUSH_DISTANCE = PLAYER_RADIUS + 0.01f;		// a bit bigger than player radius.
		
		if (direction == "UP")
		{
			int tile_check_1 = tilemap.get_tile((int)(x - PUSH_RADIUS), (int)(-y - PUSH_DISTANCE));
			int tile_check_2 = tilemap.get_tile((int)(x + PUSH_RADIUS), (int)(-y - PUSH_DISTANCE));
			if ((tile_check_1 == _TILEMAP.TILE_PUSHABLE ||
				tile_check_1 == _TILEMAP.TILE_GOAL) &&
				tile_check_2 == tile_check_1)
			{
				Debug.Log("pushing up");
				pushable_tile_script pushable = tilemap.get_pushable_tile_script((int)x, (int)(-y - PUSH_DISTANCE));
				pushable.push_this_tile("UP");
			}
			
		}
		
		if (direction == "DOWN")
		{
			int tile_check_1 = tilemap.get_tile((int)(x - PUSH_RADIUS), (int)(-y + PUSH_DISTANCE));
			int tile_check_2 = tilemap.get_tile((int)(x + PUSH_RADIUS), (int)(-y + PUSH_DISTANCE));
			if ((tile_check_1 == _TILEMAP.TILE_PUSHABLE ||
				tile_check_1 == _TILEMAP.TILE_GOAL) &&
				tile_check_2 == tile_check_1)
			{
				pushable_tile_script pushable = tilemap.get_pushable_tile_script((int)x, (int)(-y + PUSH_DISTANCE));
				pushable.push_this_tile("DOWN");
			}
			
		}
		
		if (direction == "LEFT")
		{
			int tile_check_1 = tilemap.get_tile((int)(x - PUSH_DISTANCE), (int)(-y - PUSH_RADIUS));
			int tile_check_2 = tilemap.get_tile((int)(x - PUSH_DISTANCE), (int)(-y + PUSH_RADIUS));
			if ((tile_check_1 == _TILEMAP.TILE_PUSHABLE ||
				tile_check_1 == _TILEMAP.TILE_GOAL) &&
				tile_check_2 == tile_check_1)
			{
				pushable_tile_script pushable = tilemap.get_pushable_tile_script((int)(x - PUSH_DISTANCE), (int)(-y));
				pushable.push_this_tile("LEFT");
			}
		}
		
		if (direction == "RIGHT")
		{
			int tile_check_1 = tilemap.get_tile((int)(x + PUSH_DISTANCE), (int)(-y - PUSH_RADIUS));
			int tile_check_2 = tilemap.get_tile((int)(x + PUSH_DISTANCE), (int)(-y + PUSH_RADIUS));
			if ((tile_check_1 == _TILEMAP.TILE_PUSHABLE ||
				tile_check_1 == _TILEMAP.TILE_GOAL) &&
				tile_check_2 == tile_check_1)
			{
				Debug.Log("trying to push right. type="+tile_check_1);
				pushable_tile_script pushable = tilemap.get_pushable_tile_script((int)(x + PUSH_DISTANCE), (int)(-y));
				pushable.push_this_tile("RIGHT");
			}
		}
	}



	public void laser_hit()
	{
		particles.start_particle(transform.position, 0, 0);
		player_model.GetComponent<player_animation_script>().damage();
	}
}
