using UnityEngine;
using System.Collections;

public class pushable_tile_script : MonoBehaviour
{
	private _TILEMAP tilemap;
	private int id = -1;
	private int type = -1;
	private Transform model;
	
	private int x = 0;
	private int y = 0;
	private int tile_under = _TILEMAP.TILE_FLOOR;
	private float move_timer = 0.0f;
	private float MOVE_TIME = 0.3f;
	private string move_direction = "";
	private bool starting_to_move = false;
	private Quaternion final_rotation;



	void Start()
	{
		x = (int)transform.position.x;
		y = (int)(-transform.position.z);
		get_type_of_this_tile();
		
		tilemap = GameObject.Find("_GLOBAL_SCRIPTS").GetComponent<_TILEMAP>();
		id = tilemap.add_to_pushable_list(x, y, type, gameObject.GetComponent<pushable_tile_script>());
		model = transform.FindChild("Cube");
	}



	private void get_type_of_this_tile()
	{
		if (transform.name == "tile_pushable(Clone)") type = _TILEMAP.TILE_PUSHABLE;
		if (transform.name == "tile_goal(Clone)") type = _TILEMAP.TILE_GOAL;
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
		float current_y = 0.5f + 0.2f * Mathf.Sin((move_timer / MOVE_TIME) * 3.14159f);
		
		if (move_direction == "UP")
		{
			if (starting_to_move)
			{
				model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f), 90.0f);
				final_rotation = model.localRotation;
				model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f), -90.0f);
				starting_to_move = false;
			}
			model.position = transform.position + new Vector3(0.0f, current_y, -move_timer / MOVE_TIME);
			model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f), Time.deltaTime * 90.0f / MOVE_TIME);
			if (timer <= 0.0f)
			{
				model.rotation = final_rotation;
				model.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
			}
		}
		if (move_direction == "DOWN")
		{
			if (starting_to_move)
			{
				model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f), -90.0f);
				final_rotation = model.localRotation;
				model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f), 90.0f);
				starting_to_move = false;
			}
			model.position = transform.position + new Vector3(0.0f, current_y, move_timer / MOVE_TIME);
			model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f), Time.deltaTime * -90.0f / MOVE_TIME);
			if (timer <= 0.0f)
			{
				model.rotation = final_rotation;
				model.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
			}
		}
		if (move_direction == "LEFT")
		{
			if (starting_to_move)
			{
				model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), 90.0f);
				final_rotation = model.localRotation;
				model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), -90.0f);
				starting_to_move = false;
			}
			model.position = transform.position + new Vector3(move_timer / MOVE_TIME, current_y, 0.0f);
			model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), Time.deltaTime * 90.0f / MOVE_TIME);
			if (timer <= 0.0f)
			{
				model.rotation = final_rotation;
				model.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
			}
		}
		if (move_direction == "RIGHT")
		{
			if (starting_to_move)
			{
				model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), -90.0f);
				final_rotation = model.localRotation;
				model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), 90.0f);
				starting_to_move = false;
			}
			model.position = transform.position + new Vector3(-move_timer / MOVE_TIME, current_y, 0.0f);
			model.RotateAround(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), Time.deltaTime * -90.0f / MOVE_TIME);
			if (timer <= 0.0f)
			{
				model.rotation = final_rotation;
				model.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
			}
		}
		
		if (timer < 0.0f)
		{
			timer = 0.0f;
			move_direction = "";
		}
		
	}



	public void push_this_tile(string direction)
	{
		int previous_tile_under = tile_under;
		
		if (move_timer <= 0.0f)
		{
			Debug.Log("tile is pushed to direction"+direction);
			if (direction == "DOWN")
			{
				if (tilemap.can_a_tile_be_pushed_here(x, y + 1))
				{
					tile_under = tilemap.get_tile(x, y + 1);
					tilemap.set_tile(x, y + 1, type);
					tilemap.set_tile(x, y, previous_tile_under);
					y++;
					tilemap.update_pushable_data(id, x, y);
					move_timer = MOVE_TIME;
					move_direction = direction;
					starting_to_move = true;
					
					if (tile_under == _TILEMAP.TILE_START_AREA && previous_tile_under != tile_under)
						tilemap.goal_blocks++;
					if (previous_tile_under == _TILEMAP.TILE_START_AREA && previous_tile_under != tile_under)
						tilemap.goal_blocks--;
				}
				else
				Debug.Log("cant push!");
			}
			if (direction == "UP")
			{
				if (tilemap.can_a_tile_be_pushed_here(x, y - 1))
				{
					tile_under = tilemap.get_tile(x, y - 1);
					tilemap.set_tile(x, y - 1, type);
					tilemap.set_tile(x, y, previous_tile_under);
					y--;
					tilemap.update_pushable_data(id, x, y);
					move_timer = MOVE_TIME;
					move_direction = direction;
					starting_to_move = true;
					
					if (tile_under == _TILEMAP.TILE_START_AREA && previous_tile_under != tile_under)
						tilemap.goal_blocks++;
					if (previous_tile_under == _TILEMAP.TILE_START_AREA && previous_tile_under != tile_under)
						tilemap.goal_blocks--;
				}
				else
					Debug.Log("cant push!");
			}
			if (direction == "LEFT")
			{
				if (tilemap.can_a_tile_be_pushed_here(x - 1, y))
				{
					tile_under = tilemap.get_tile(x - 1, y);
					tilemap.set_tile(x - 1, y, type);
					tilemap.set_tile(x, y, previous_tile_under);
					x--;
					tilemap.update_pushable_data(id, x, y);
					move_timer = MOVE_TIME;
					move_direction = direction;
					starting_to_move = true;
					
					if (tile_under == _TILEMAP.TILE_START_AREA && previous_tile_under != tile_under)
						tilemap.goal_blocks++;
					if (previous_tile_under == _TILEMAP.TILE_START_AREA && previous_tile_under != tile_under)
						tilemap.goal_blocks--;
				}
				else
					Debug.Log("cant push!");
			}
			if (direction == "RIGHT")
			{
				if (tilemap.can_a_tile_be_pushed_here(x + 1, y))
				{
					tile_under = tilemap.get_tile(x + 1, y);
					tilemap.set_tile(x + 1, y, type);
					tilemap.set_tile(x, y, previous_tile_under);
					x++;
					tilemap.update_pushable_data(id, x, y);
					move_timer = MOVE_TIME;
					move_direction = direction;
					starting_to_move = true;
					
					if (tile_under == _TILEMAP.TILE_START_AREA && previous_tile_under != tile_under)
						tilemap.goal_blocks++;
					if (previous_tile_under == _TILEMAP.TILE_START_AREA && previous_tile_under != tile_under)
						tilemap.goal_blocks--;
				}
				else
					Debug.Log("cant push!");
			}
		}
		else
		{
			Debug.Log("cant move yet!");
			
		}
        MESSAGE.print("GOAL BLOCKS FOUND: " + tilemap.goal_blocks, -160, -90, 1, 65);
	}


}
