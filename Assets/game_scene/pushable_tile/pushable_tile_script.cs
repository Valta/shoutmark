using UnityEngine;
using System.Collections;

public class pushable_tile_script : MonoBehaviour
{
	private _TILEMAP tilemap;
	private int id = -1;
	private Transform model;
	
	private int x = 0;
	private int y = 0;
	private float move_timer = 0.0f;
	private float MOVE_TIME = 0.3f;
	private string move_direction = "";
	private bool starting_to_move = false;
	private Quaternion final_rotation;



	void Start()
	{
		x = (int)transform.position.x;
		y = (int)(-transform.position.z);
		
		tilemap = GameObject.Find("_GLOBAL_SCRIPTS").GetComponent<_TILEMAP>();
		id = tilemap.add_to_pushable_list(x, y, gameObject.GetComponent<pushable_tile_script>());
		model = transform.FindChild("Cube");
	}



	void Update()
	{
		transform.position = new Vector3(x + 0.5f, 0.0f, -y - 0.5f);
		animate_moving();
	}



	private void animate_moving()
	{
		move_timer -= Time.deltaTime;
		float timer = move_timer;
		if (timer < 0.0f)
		{
			timer = 0.0f;
			move_direction = "";
		}
		if (move_direction == "UP")
		{
			if (starting_to_move)
			{
				//model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f), 90.0f);
				starting_to_move = false;
			}
			model.position = transform.position + new Vector3(0.0f, 0.5f, -move_timer / MOVE_TIME);
			model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f), Time.deltaTime * 90.0f / MOVE_TIME);
		}
		if (move_direction == "DOWN")
		{
			model.position = transform.position + new Vector3(0.0f, 0.5f, move_timer / MOVE_TIME);
			model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f), Time.deltaTime * -90.0f / MOVE_TIME);
			Debug.Log("rotation="+model.rotation.x+"  "+model.rotation.y+"  "+model.rotation.z);
		}
		if (move_direction == "LEFT")
		{
			model.position = transform.position + new Vector3(move_timer / MOVE_TIME, 0.5f, 0.0f);
			model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), Time.deltaTime * 90.0f / MOVE_TIME);
			Debug.Log("rotation="+model.rotation.x+"  "+model.rotation.y+"  "+model.rotation.z);
		}
		if (move_direction == "RIGHT")
		{
			model.position = transform.position + new Vector3(-move_timer / MOVE_TIME, 0.5f, 0.0f);
			model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), Time.deltaTime * -90.0f / MOVE_TIME);
			Debug.Log("rotation="+model.rotation.x+"  "+model.rotation.y+"  "+model.rotation.z);
		}
		
		
	}



	public void push_this_tile(string direction)
	{
		if (move_timer <= 0.0f)
		{
			Debug.Log("tile is pushed to direction"+direction);
			if (direction == "DOWN")
			{
				if (tilemap.can_a_tile_be_pushed_here(x, y + 1))
				{
					tilemap.set_tile(x, y + 1, _TILEMAP.TILE_PUSHABLE);
					tilemap.set_tile(x, y, _TILEMAP.TILE_FLOOR);
					y++;
					tilemap.update_pushable_data(id, x, y);
					move_timer = MOVE_TIME;
					move_direction = direction;
					starting_to_move = true;
				}
				else
				Debug.Log("cant push!");
			}
			if (direction == "UP")
			{
				if (tilemap.can_a_tile_be_pushed_here(x, y - 1))
				{
					tilemap.set_tile(x, y - 1, _TILEMAP.TILE_PUSHABLE);
					tilemap.set_tile(x, y, _TILEMAP.TILE_FLOOR);
					y--;
					tilemap.update_pushable_data(id, x, y);
					move_timer = MOVE_TIME;
					move_direction = direction;
					starting_to_move = true;
				}
				else
					Debug.Log("cant push!");
			}
			if (direction == "LEFT")
			{
				if (tilemap.can_a_tile_be_pushed_here(x - 1, y))
				{
					tilemap.set_tile(x - 1, y, _TILEMAP.TILE_PUSHABLE);
					tilemap.set_tile(x, y, _TILEMAP.TILE_FLOOR);
					x--;
					tilemap.update_pushable_data(id, x, y);
					move_timer = MOVE_TIME;
					move_direction = direction;
					starting_to_move = true;
				}
				else
					Debug.Log("cant push!");
			}
			if (direction == "RIGHT")
			{
				if (tilemap.can_a_tile_be_pushed_here(x + 1, y))
				{
					tilemap.set_tile(x + 1, y, _TILEMAP.TILE_PUSHABLE);
					tilemap.set_tile(x, y, _TILEMAP.TILE_FLOOR);
					x++;
					tilemap.update_pushable_data(id, x, y);
					move_timer = MOVE_TIME;
					move_direction = direction;
					starting_to_move = true;
				}
				else
					Debug.Log("cant push!");
			}
		}
		else
		{
			Debug.Log("cant move yet!");
			
		}
	}


}
