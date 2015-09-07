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



	void Start()
	{
		x = transform.position.x;
		y = transform.position.z;
		tilemap = GameObject.Find("_GLOBAL_SCRIPTS").GetComponent<_TILEMAP>();
		gamecamera = GameObject.Find("Main Camera").GetComponent<camera_script>();
		
		id = tilemap.add_to_object_list(x, y, PLAYER_RADIUS);
		gamecamera.set_camera_target(x, y, true);
	}



	void Update()
	{
		float speed = 4.0f * Time.deltaTime;
		speed_x = 0.0f;
		speed_y = 0.0f;
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
		
		move_player();
		gamecamera.set_camera_target(x, y, false);
		tilemap.update_object_data(id, x, y);
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
		float PUSH_DISTANCE = PLAYER_RADIUS + 0.1f;		// a bit bigger than player radius.
		
		if (direction == "UP")
		{
			int tile_check_1 = tilemap.get_tile((int)(x - PUSH_RADIUS), (int)(-y - PUSH_DISTANCE));
			int tile_check_2 = tilemap.get_tile((int)(x + PUSH_RADIUS), (int)(-y - PUSH_DISTANCE));
			if (tile_check_1 == _TILEMAP.TILE_PUSHABLE && tile_check_2 == _TILEMAP.TILE_PUSHABLE)
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
			if (tile_check_1 == _TILEMAP.TILE_PUSHABLE && tile_check_2 == _TILEMAP.TILE_PUSHABLE)
			{
				pushable_tile_script pushable = tilemap.get_pushable_tile_script((int)x, (int)(-y + PUSH_DISTANCE));
				pushable.push_this_tile("DOWN");
			}
			
		}
		
		if (direction == "LEFT")
		{
			int tile_check_1 = tilemap.get_tile((int)(x - PUSH_DISTANCE), (int)(-y - PUSH_RADIUS));
			int tile_check_2 = tilemap.get_tile((int)(x - PUSH_DISTANCE), (int)(-y + PUSH_RADIUS));
			if (tile_check_1 == _TILEMAP.TILE_PUSHABLE && tile_check_2 == _TILEMAP.TILE_PUSHABLE)
			{
				pushable_tile_script pushable = tilemap.get_pushable_tile_script((int)(x - PUSH_DISTANCE), (int)(-y));
				pushable.push_this_tile("LEFT");
			}
		}
		
		if (direction == "RIGHT")
		{
			int tile_check_1 = tilemap.get_tile((int)(x + PUSH_DISTANCE), (int)(-y - PUSH_RADIUS));
			int tile_check_2 = tilemap.get_tile((int)(x + PUSH_DISTANCE), (int)(-y + PUSH_RADIUS));
			if (tile_check_1 == _TILEMAP.TILE_PUSHABLE && tile_check_2 == _TILEMAP.TILE_PUSHABLE)
			{
				pushable_tile_script pushable = tilemap.get_pushable_tile_script((int)(x + PUSH_DISTANCE), (int)(-y));
				pushable.push_this_tile("RIGHT");
			}
		}
	}
}
