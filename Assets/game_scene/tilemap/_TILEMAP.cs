using UnityEngine;
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
	private int current_level = 0;
	private _LEVELS levels;
    private laser_script laser;
    private particle_script particlesys;
    
	
										// counters:
	public int total_goal_blocks = 0;
	public int goal_blocks = 0;
	
										// prefabs:
	public GameObject tile_floor;
	public GameObject tile_floor_var1;
	public GameObject tile_floor_var2;
	public GameObject tile_start_area;
	public GameObject tile_block;
	public GameObject tile_transparentwall;
	public GameObject tile_wireframesprite;
	public GameObject tile_testcube;
	public GameObject tile_pushable;
	public GameObject tile_goal;
	public GameObject player;
	public GameObject enemy;
	
										// map[,] array values:
	public const int TILE_FLOOR = 0;
	public const int TILE_START_AREA = 1;
	public const int TILE_BLOCK = 2;
	public const int TILE_TRANSPARENTWALL = 3;
	public const int TILE_WIREFRAMESPRITE = 4;
	public const int TILE_TESTCUBE = 5;
	public const int TILE_PUSHABLE = 6;
	public const int TILE_GOAL = 7;
	public const int START = 8;
	public const int ENEMY = 9;
	
										// moving objects:
	private const int MAX_NUMBER_OF_OBJECTS = 100;
	private int number_of_objects = 0;
	private float[] object_x = new float[MAX_NUMBER_OF_OBJECTS];
	private float[] object_y = new float[MAX_NUMBER_OF_OBJECTS];
	private float[] object_radius = new float[MAX_NUMBER_OF_OBJECTS];
	private int player_id = -1;

										// pushable objects:
	private const int MAX_NUMBER_OF_PUSHABLES = 100;
	private int number_of_pushables = 0;
	private int[] pushable_x = new int[MAX_NUMBER_OF_PUSHABLES];
	private int[] pushable_y = new int[MAX_NUMBER_OF_PUSHABLES];
	private int[] pushable_tile_type = new int[MAX_NUMBER_OF_PUSHABLES];
	private pushable_tile_script[] pushable_script = new pushable_tile_script[MAX_NUMBER_OF_PUSHABLES];



	void Start()
	{
		levels = gameObject.GetComponent<_LEVELS>();
        laser = gameObject.GetComponent<laser_script>();
        particlesys = gameObject.GetComponent<particle_script>();
	}



	void Update()
	{
		
		if (goal_blocks == total_goal_blocks)
		{
			goal_blocks = 0;
            level_clear();
            // TODO: Add winning animation?
		}
        if (Input.GetKeyDown(KeyCode.N))
        {
            set_level(current_level + 1);
            
            _TIMER.set_pause(false);
            Application.LoadLevel("game_scene");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Application.LoadLevel("game_scene");
        }
        if (current_level >= _LEVELS.TOTAL_LEVELS && Input.GetKeyDown(KeyCode.Z))
        {
            Application.LoadLevel(0);
        }
	}
    void init_text()
    {
        _UI_TEXT_PRINTER text_printer = gameObject.GetComponent<_UI_TEXT_PRINTER>();
        text_printer.instantiate_letter_prefabs();
    }


				// public interface:

	public int get_level() {return current_level;}

	public void set_level(int level) {current_level = level;}

	public void start_game()
	{
        
		levels.load_level(	current_level,
							ref level_width,
							ref level_height,
							map);
		total_goal_blocks = 0;
		goal_blocks = 0;
		instantiate_tilemap();
        init_text();
        laser.initialize_lasers();
        particlesys.particle_init();
        
        MESSAGE.print("", -170, 90, 6, 10, 22);
        if (current_level < _LEVELS.TOTAL_LEVELS)
        {
            MESSAGE.print("", -160, -90, 1, 20, 65);
            MESSAGE.print("FIND " + total_goal_blocks + " GOAL BLOCKS.", -160, -90, 1, 20, 65);
        }
        MESSAGE.print("press M to pause", -170, 90, 6, 10, 22);
	}



	private void instantiate_tilemap()
	{
        // clear old object and pushable lists when starting new level
        objectlist_clear();
		GameObject tilemap_parent_gameobject = (GameObject)Instantiate(new GameObject(), new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
		Transform tilemap_parent = tilemap_parent_gameobject.transform;
		tilemap_parent.name = "_TILEMAP_PARENT";
		
		for (int x = 0; x < level_width; x++)
		{
			for (int y = 0; y < level_height; y++)
			{
				Vector3 tile_position = new Vector3(x + 0.5f, 0.0f, -y - 0.5f);
				Quaternion tile_rotation = Quaternion.identity;
				
				if (map[x, y] == TILE_FLOOR)
				{
					GameObject floor;
					int pattern = (3 * x + y * y + x * (y + 1) + (x + y) % 3) % 4;
					if (pattern > 2) pattern = 0;
					if (pattern == 0)
						floor = (GameObject)Instantiate(tile_floor, tile_position, tile_rotation);
					else if (pattern == 1)
						floor = (GameObject)Instantiate(tile_floor_var1, tile_position, tile_rotation);
					else
						floor = (GameObject)Instantiate(tile_floor_var2, tile_position, tile_rotation);
					floor.transform.SetParent(tilemap_parent);
				}
				if (map[x, y] == TILE_START_AREA)
				{
					GameObject floor = (GameObject)Instantiate(tile_start_area, tile_position, tile_rotation);
					floor.transform.SetParent(tilemap_parent);
				}
				if (map[x, y] == TILE_BLOCK)
				{
					GameObject floor = (GameObject)Instantiate(tile_floor, tile_position, tile_rotation);
					GameObject other = (GameObject)Instantiate(tile_block, tile_position, tile_rotation);
					floor.transform.SetParent(tilemap_parent);
					other.transform.SetParent(tilemap_parent);
				}
				if (map[x, y] == TILE_TRANSPARENTWALL)
				{
					GameObject floor = (GameObject)Instantiate(tile_floor, tile_position, tile_rotation);
					GameObject other = (GameObject)Instantiate(tile_transparentwall, tile_position, tile_rotation);
					floor.transform.SetParent(tilemap_parent);
					other.transform.SetParent(tilemap_parent);
				}
				if (map[x, y] == TILE_WIREFRAMESPRITE)
				{
					GameObject floor = (GameObject)Instantiate(tile_floor, tile_position, tile_rotation);
					GameObject other = (GameObject)Instantiate(tile_wireframesprite, tile_position, tile_rotation);
					floor.transform.SetParent(tilemap_parent);
					other.transform.SetParent(tilemap_parent);
				}
				if (map[x, y] == TILE_TESTCUBE)
				{
					GameObject floor = (GameObject)Instantiate(tile_floor, tile_position, tile_rotation);
					GameObject other = (GameObject)Instantiate(tile_testcube, tile_position, tile_rotation);
					floor.transform.SetParent(tilemap_parent);
					other.transform.SetParent(tilemap_parent);
				}
				if (map[x, y] == TILE_PUSHABLE)
				{
					GameObject floor = (GameObject)Instantiate(tile_floor, tile_position, tile_rotation);
					GameObject other = (GameObject)Instantiate(tile_pushable, tile_position, tile_rotation);
					floor.transform.SetParent(tilemap_parent);
					other.transform.SetParent(tilemap_parent);
				}
				if (map[x, y] == TILE_GOAL)
				{
					GameObject floor = (GameObject)Instantiate(tile_floor, tile_position, tile_rotation);
					GameObject other = (GameObject)Instantiate(tile_goal, tile_position, tile_rotation);
					floor.transform.SetParent(tilemap_parent);
					other.transform.SetParent(tilemap_parent);
					total_goal_blocks++;
				}
				if (map[x, y] == START)
				{
					GameObject floor = (GameObject)Instantiate(tile_start_area, tile_position, tile_rotation);
					GameObject.Instantiate(player, tile_position, tile_rotation);
					floor.transform.SetParent(tilemap_parent);
					map[x, y] = TILE_FLOOR;
				}
				if (map[x, y] == ENEMY)
				{
					GameObject floor = (GameObject)Instantiate(tile_floor, tile_position, tile_rotation);
					GameObject.Instantiate(enemy, tile_position, tile_rotation);
					floor.transform.SetParent(tilemap_parent);
					map[x, y] = TILE_FLOOR;
				}
			}
		}

	}



	public int add_to_object_list(float x, float y, float radius)
	{
        Debug.Log("object nr " + number_of_objects);
		if (number_of_objects < MAX_NUMBER_OF_OBJECTS)
		{
			object_x[number_of_objects] = x;
			object_y[number_of_objects] = y;
			object_radius[number_of_objects] = radius;
			number_of_objects++;
            
			return (number_of_objects - 1); // return the id for an object.
		}
        Debug.Log("objects full!");
		return -1; // the object list is full, return illegal id.
	}



	public void set_player_id(int id)
	{
		player_id = id;
	}



	public void get_player_information(ref float x, ref float y, ref float radius)		//
	{																					//
		if (player_id >= 0)
		{
			x = object_x[player_id];														//
			y = object_y[player_id];														//
			radius = object_radius[player_id];												//
		}
		else
			Debug.Log("ERROR. get_player_information(), player_id="+player_id);				//
	}																					//



	public void update_object_data(int id, float new_x, float new_y)
	{
		object_x[id] = new_x;
		object_y[id] = new_y;
	}



	public int add_to_pushable_list(int x, int y, int type, pushable_tile_script script)
	{
		if (number_of_pushables < MAX_NUMBER_OF_PUSHABLES)
		{
			pushable_x[number_of_pushables] = x;
			pushable_y[number_of_pushables] = y;
			pushable_tile_type[number_of_pushables] = type;
			pushable_script[number_of_pushables] = script;
			number_of_pushables++;
			return (number_of_pushables - 1); // return the id for the pushable tile.
		}
        Debug.Log("pushables full!");
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
			//Debug.Log("pushable coordinates:"+pushable_x[a]+","+pushable_y[a]);
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
		
		if (map[xx, yy] == TILE_FLOOR ||
			map[xx, yy] == TILE_START_AREA)
			return true; // free tile?
		
		return false; // something else is on the way.
	}



	public bool floor(float x, float y)
	{
		int xx = (int)x;			// now [xx, yy] are the
		int yy = (int)(-y);			// map array coordinates.
		
		if (x < 0 || y > 0 || xx >= level_width || yy >= level_height) return false; // outside the map?
		if (map[xx, yy] == TILE_FLOOR ||
			map[xx, yy] == TILE_START_AREA)
			return true; // floor tile?
		
		return false; // some other tile.
	}



	public bool can_a_tile_be_pushed_here(int x, int y)
	{
		if (map[x, y] == TILE_FLOOR || map[x, y] == TILE_START_AREA) return true; else return false;
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

    private void level_clear()
    {
        if (current_level < _LEVELS.TOTAL_LEVELS)
        {
            MESSAGE.print("LEVEL CLEAR!", -160, 0, 1, 20, 65);
            MESSAGE.print("", -160, 20, 5, 15, 66);
            MESSAGE.print("press n to continue", -160, 20, 5, 15, 66);
        }
        MESSAGE.print("", -170, 90, 6, 10, 22);
        // TODO: pause, next button, animation?
        _TIMER.set_pause(true);
    }
    private void objectlist_clear()
    {
	    for (int i = 0; i < MAX_NUMBER_OF_OBJECTS; ++i)
        {
            object_x[i] = 0;
            object_y[i] = 0;
            object_radius[i] = 0;
        }
	    number_of_objects = 0;

        for (int i = 0; i < MAX_NUMBER_OF_PUSHABLES; ++i)
        {
            pushable_x[i] = 0;
            pushable_y[i] = 0;
            pushable_tile_type[i] = 0;
            pushable_script[i] = null;
        }
        number_of_pushables = 0;
    }
}
