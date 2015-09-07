﻿using UnityEngine;
using System.Collections;



	// kameran kallistuskulmat: (38, 12, 0)
	// kameran koko (ortografinen): noin 2.8
	// tilekuvan position, rotation, scale= (0.08, 0.95, 0.37), (0, 12, 0), (1.4, 3.55, 0)



public class _TILEMAP : MonoBehaviour
{
	private const int MAX_LEVEL_SIZE = 50;
	private int[,] map = new int[MAX_LEVEL_SIZE, MAX_LEVEL_SIZE];
	private int level_width = 0;
	private int level_height = 0;
	private int current_level = 1;
	private _LEVELS levels;
	
										// prefabs:
	public GameObject tile_floor;
	public GameObject tile_block;
	public GameObject tile_transparentwall;
	public GameObject tile_wireframesprite;
	public GameObject tile_testcube;
	public GameObject tile_pushable;
	public GameObject player;
	public GameObject enemy;
	
										// map[,] array values:
	public const int TILE_FLOOR = 0;
	public const int TILE_BLOCK = 1;
	public const int TILE_TRANSPARENTWALL = 2;
	public const int TILE_WIREFRAMESPRITE = 3;
	public const int TILE_TESTCUBE = 4;
	public const int TILE_PUSHABLE = 5;
	public const int START = 6;
	public const int ENEMY = 7;
	
										// moving objects:
	private const int MAX_NUMBER_OF_OBJECTS = 100;
	private int number_of_objects = 0;
	private float[] object_x = new float[MAX_NUMBER_OF_OBJECTS];
	private float[] object_y = new float[MAX_NUMBER_OF_OBJECTS];
	private float[] object_radius = new float[MAX_NUMBER_OF_OBJECTS];

	// moving objects:
	private const int MAX_NUMBER_OF_PUSHABLES = 100;
	private int number_of_pushables = 0;
	private int[] pushable_x = new int[MAX_NUMBER_OF_OBJECTS];
	private int[] pushable_y = new int[MAX_NUMBER_OF_OBJECTS];
	private pushable_tile_script[] pushable_script = new pushable_tile_script[MAX_NUMBER_OF_OBJECTS];



	void Start()
	{
		levels = GameObject.Find("_GLOBAL_SCRIPTS").GetComponent<_LEVELS>();
		
		levels.load_level(	current_level,
							ref level_width,
							ref level_height,
							map);
		Debug.Log("w,h="+level_width+","+level_height);
		instantiate_tilemap();
	}



	void Update()
	{
	
	}



	public void instantiate_tilemap()
	{
		for (int x = 0; x < level_width; x++)
		{
			for (int y = 0; y < level_height; y++)
			{
				Vector3 tile_position = new Vector3(x + 0.5f, 0.0f, -y - 0.5f);
				Quaternion tile_rotation = Quaternion.identity;
				
				if (map[x, y] == TILE_FLOOR)
				{
					GameObject.Instantiate(tile_floor, tile_position, tile_rotation);
				}
				if (map[x, y] == TILE_BLOCK)
				{
					GameObject.Instantiate(tile_floor, tile_position, tile_rotation);
					GameObject.Instantiate(tile_block, tile_position, tile_rotation);
				}
				if (map[x, y] == TILE_TRANSPARENTWALL)
				{
					GameObject.Instantiate(tile_floor, tile_position, tile_rotation);
					GameObject.Instantiate(tile_transparentwall, tile_position, tile_rotation);
				}
				if (map[x, y] == TILE_WIREFRAMESPRITE)
				{
					GameObject.Instantiate(tile_floor, tile_position, tile_rotation);
					GameObject.Instantiate(tile_wireframesprite, tile_position, tile_rotation);
				}
				if (map[x, y] == TILE_TESTCUBE)
				{
					GameObject.Instantiate(tile_floor, tile_position, tile_rotation);
					GameObject.Instantiate(tile_testcube, tile_position, tile_rotation);
				}
				if (map[x, y] == TILE_PUSHABLE)
				{
					GameObject.Instantiate(tile_floor, tile_position, tile_rotation);
					GameObject.Instantiate(tile_pushable, tile_position, tile_rotation);
				}
				if (map[x, y] == START)
				{
					GameObject.Instantiate(tile_floor, tile_position, tile_rotation);
					GameObject.Instantiate(player, tile_position, tile_rotation);
					map[x, y] = TILE_FLOOR;
				}
				if (map[x, y] == ENEMY)
				{
					GameObject.Instantiate(tile_floor, tile_position, tile_rotation);
					GameObject.Instantiate(enemy, tile_position, tile_rotation);
					map[x, y] = TILE_FLOOR;
				}
			}
		}
	}



	public int add_to_object_list(float x, float y, float radius)
	{
		if (number_of_objects < MAX_NUMBER_OF_OBJECTS)
		{
			object_x[number_of_objects] = x;
			object_y[number_of_objects] = y;
			object_radius[number_of_objects] = radius;
			number_of_objects++;
			return (number_of_objects - 1); // return the id for an object.
		}
		return -1; // the object list is full, return illegal id.
	}



	public void update_object_data(int id, float new_x, float new_y)
	{
		object_x[id] = new_x;
		object_y[id] = new_y;
	}



	public int add_to_pushable_list(int x, int y, pushable_tile_script script)
	{
		if (number_of_pushables < MAX_NUMBER_OF_PUSHABLES)
		{
			pushable_x[number_of_pushables] = x;
			pushable_y[number_of_pushables] = y;
			pushable_script[number_of_pushables] = script;
			number_of_pushables++;
			return (number_of_pushables - 1); // return the id for the pushable tile.
		}
		return -1; // the pushable list is full, return illegal id.
	}



	public void update_pushable_data(int id, int new_x, int new_y)
	{
		pushable_x[id] = new_x;
		pushable_y[id] = new_y;
	}



	public pushable_tile_script get_pushable_tile_script(int x, int y)
	{
		for (int a = 0; a < number_of_pushables; a++)
		{
			Debug.Log("pushable coordinates:"+pushable_x[a]+","+pushable_y[a]);
			if (pushable_x[a] == x && pushable_y[a] == y)
				return pushable_script[a];
		}
		Debug.Log("_TILEMAP: get_pushable_tile_script. bad coordinates! (" + x + ", " + y + ")");
		return null; // bad coordinates, return null.
	}



	public bool can_move(float x, float y) // collision check.
	{
		int xx = (int)x;			// now [xx, yy] are the
		int yy = (int)(-y);			// map array coordinates.
		
		if (x < 0 || y > 0 || xx >= level_width || yy >= level_height) return false; // outside the map?
		
		for (int a = 0; a < number_of_objects; a++)						// collision with
		{																// another object?
			if (x > object_x[a] - object_radius[a] &&					//
				x < object_x[a] + object_radius[a] &&					//
				y > object_y[a] - object_radius[a] &&					//
				y < object_y[a] + object_radius[a]) return false;		//
		}																//
		
		if (map[xx, yy] == TILE_FLOOR) return true; // free tile?
		
		return false; // something else is on the way.
	}



	public bool can_a_tile_be_pushed_here(int x, int y)
	{
		if (map[x, y] == TILE_FLOOR) return true; else return false;
	}



	public void set_tile(int x, int y, int tile)
	{
		if (x >= 0 && y >= 0 && x < level_width && y < level_height)
			map[x, y] = tile;
		else
			Debug.Log("TILEMAP: set_tile. bad coordinates! (" + x + ", " + y + ")");
	}



	public int get_tile(int x, int y)
	{
		if (x >= 0 && y >= 0 && x < level_width && y < level_height)
			return map[x, y];
		else
			return TILE_BLOCK; // if coordinates are outside the level, return any non-movable wall tile.
	}
}