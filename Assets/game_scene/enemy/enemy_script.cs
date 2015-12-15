using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: Rearrange data so it can be found!
public class enemy_script : MonoBehaviour
{
	private const float SPEED = 1.0f;
	private const float TEXTURE_SPEED = 7.5f;
	private const float ENEMY_RADIUS = 0.35f;
	
	private Transform model;
	//private Material model_material;
	private _TILEMAP tilemap;
    private radar_script radar;
    private enemy_graphics_script graphics;
    private float dt = 0.0f;
	
	private float x = 0.0f;
	private float y = 0.0f;
	private float speed_x = 0.0f;
	private float speed_y = 0.0f;
    private float direction = 90.0f;
	private int id = -1;

    private const float COOLDOWN_TIME = 3.0f;
    private const float ATTACK_RANGE = 3.0f;
    private const float GOSSIP_RANGE = 1.0f; // Has to be more than enemy radius * 2!
    private const int MAX_HEALTH = 5;
    private const float SEARCH_TIMEOUT = 10.0f;
    
// stuff from tree (TODO: delete useless?)

    public tree_script bt;

    private Vector3 _position;
    private Vector3 player_last_seen;
    private bool player_sighted;

    //public bool friend_close;
    //public bool will_talk;
    //public List<Vector3> friends_last_seen;
    //public Vector3 closest_friend;
//

    public int hits;
    public float end_angle;
    public float current_angle_in_degrees;

    // values defined in leaf nodes
    public float attack_timer = 0;
    private float search_timer = SEARCH_TIMEOUT;
    public float last_gossip;

    // list of open nodes on last tick
    public List<general_node> open_nodes;
    private sequence_memory_data search_sequence;

    // enemy stops moving when it is looking around.
    private bool looking;

    public bool searching { set; get; }
    // access methods for stuff needed in AI
    public void SetLooking(bool is_looking) { looking = is_looking; }
    public void SetDirection(float target_x, float target_y)
    { direction = ENEMY_MATH.get_absolute_angle(target_x - x, target_y - y); }
    public void SetDirectionToPlayer()
    { direction = ENEMY_MATH.get_absolute_angle(player_last_seen.x - x, player_last_seen.y - y); }
    public void SetDirection(float angle) { direction = angle; }
    public float GetDirectionAngle() { return direction; }
    public Vector3 GetPosition() { return _position; }
    public bool is_dead() { return hits >= MAX_HEALTH; }
    public bool GetPlayerSighted() { return radar.seeing_player(); }
    public bool GetPlayerInRange() { return Vector3.Distance(_position, player_last_seen) < ATTACK_RANGE; }
    public Vector3 GetPlayerLastSeen() { return player_last_seen; }
    public List<general_node> GetOpenList() { return open_nodes; }
    public void ResetCooldown() { attack_timer = COOLDOWN_TIME; }
    public void ResetSearchTimer() { search_timer = SEARCH_TIMEOUT; }
    
    // NB we need two separate lists so pointer would be, er, pointless.
    public void SetOpenList(List<general_node> new_list)
    {
        open_nodes.Clear();
        open_nodes.AddRange(new_list);        
    }
    public void shoot()
    {
        graphics.shoot();
    }
    ////////////////////////////////////////////////////////////////////////////// Sequence data methods. identify by int / enum! no id now b/c only one.
    
    public sequence_memory_data get_data(int id)
    {        
        return search_sequence;
    }
    public void set_running_child(int child, int id)
    {
        search_sequence.running_child = child;        
    }
    // this may be useless:
    public void set_current_state(State state, int id)
    {
        search_sequence.current_state = state;
    }
    
    /// ///////////////////////////////////////////////////////////////////////////
    
	void Start()
	{
       
        x = transform.position.x;
		y = transform.position.z;
		
		model = transform.FindChild("enemy_model_parent");
		//model_material = model.gameObject.GetComponent<Renderer>().material;
		tilemap = GameObject.Find("_GLOBAL_SCRIPTS").GetComponent<_TILEMAP>();
        radar = gameObject.GetComponent<radar_script>();
		id = tilemap.add_to_object_list(x, y, ENEMY_RADIUS);
        bt = GameObject.Find("Tree").GetComponent<tree_script>();
        graphics = model.gameObject.GetComponent<enemy_graphics_script>();
        
        search_sequence.running_child = 0;
        search_sequence.current_state = State.FAILURE;

        open_nodes = new List<general_node>();
	}



	void Update()
	{
		dt =_TIMER.deltatime();
        
        if (!looking)
        {
            speed_x = SPEED * Mathf.Cos(direction * 3.14159265f / 180.0f);
            speed_y = SPEED * Mathf.Sin(direction * 3.14159265f / 180.0f);
        }
        else
        {
            
            speed_x = 0;
            speed_y = 0;
        }
        //rotate_model();
        move_enemy();
		//scroll_texture();
		//update_collision_jump();        
		tilemap.update_object_data(id, x, y);
        update_world_status();
        bt.UpdateAI(this);
	}

    private void update_world_status()
    {
        _position.x = x;
        _position.y = y;
        player_last_seen = radar.last_player_position();
        if (searching)
        {
            search_timer -= _TIMER.deltatime();
            if (search_timer < 0)
            {
                //Debug.Log("search timeout");
                search_timer = SEARCH_TIMEOUT;
                searching = false;
            }
        }
    }
    
	private void move_enemy()
	{
        bool este = false;

		if (speed_y > 0.0f)
		{
			if (tilemap.can_move(x - ENEMY_RADIUS, y + ENEMY_RADIUS + speed_y * dt) &&
				tilemap.can_move(x, y + ENEMY_RADIUS + speed_y * dt) &&
				tilemap.can_move(x + ENEMY_RADIUS, y + ENEMY_RADIUS + speed_y * dt)) y += speed_y * dt;
			else
                este = true;
		}
		if (speed_y < 0.0f)
		{
			if (tilemap.can_move(x - ENEMY_RADIUS, y - ENEMY_RADIUS + speed_y * dt) &&
				tilemap.can_move(x, y - ENEMY_RADIUS + speed_y * dt) &&
				tilemap.can_move(x + ENEMY_RADIUS, y - ENEMY_RADIUS + speed_y * dt)) y += speed_y * dt;
			else
                este = true;
		}
		if (speed_x < 0.0f)
		{
			if (tilemap.can_move(x - ENEMY_RADIUS + speed_x * dt, y + ENEMY_RADIUS) &&
				tilemap.can_move(x - ENEMY_RADIUS + speed_x * dt, y) &&
				tilemap.can_move(x - ENEMY_RADIUS + speed_x * dt, y - ENEMY_RADIUS)) x += speed_x * dt;
			else
                este = true;
		}
		if (speed_x > 0.0f)
		{
			if (tilemap.can_move(x + ENEMY_RADIUS + speed_x * dt, y + ENEMY_RADIUS) &&
				tilemap.can_move(x + ENEMY_RADIUS + speed_x * dt, y) &&
				tilemap.can_move(x + ENEMY_RADIUS + speed_x * dt, y - ENEMY_RADIUS)) x += speed_x * dt;
			else
                este = true;
		}
		if (este)
        {
            speed_x = 0;
            speed_y = 0;
            direction += _TIMER.deltatime() * 180.0f;
        }
        radar.set_direction(direction);
		transform.position = new Vector3(x, transform.position.y, y);
        // update position used by BT nodes (if necessary?)



	}



	private void rotate_model()
	{
		float rotation_speed = 360.0f / 3.14f;
		
		model.RotateAround(	transform.position + new Vector3(0.0f, 0.5f, 0.0f),
							new Vector3(0.0f, 0.0f, 1.0f), rotation_speed * -speed_x * dt);
		model.RotateAround(	transform.position + new Vector3(0.0f, 0.5f, 0.0f),
							new Vector3(1.0f, 0.0f, 0.0f), rotation_speed * speed_y * dt);
	}



	private void scroll_texture()
	{
		int step = (int)Mathf.Abs((int)(_TIMER.time() * TEXTURE_SPEED + id * 0.5f) % 36 - 12);
		//model_material.SetTextureOffset("_MainTex", new Vector2(0.0f, step / 12.0f));
	}



	private float jump_timer = 999.0f;
	private void update_collision_jump()
	{

		
		float FIRST_JUMP = 0.4f;
		float SECOND_JUMP = 0.7f;
		
		float FIRST_JUMP_HEIGHT = 0.4f;
		float SECOND_JUMP_HEIGHT = 0.2f;
		
		float new_y = 0.0f;

		jump_timer += Time.deltaTime;
		if (speed_x == 0.0f && speed_y == 0.0f && jump_timer > SECOND_JUMP) jump_timer = 0.0f;

		if (jump_timer < FIRST_JUMP)
			new_y = Mathf.Sin(jump_timer * 3.14159f / FIRST_JUMP) * FIRST_JUMP_HEIGHT;
		else if (jump_timer >= FIRST_JUMP && jump_timer < SECOND_JUMP)
			new_y = Mathf.Sin((jump_timer - FIRST_JUMP) * 3.14159f / (SECOND_JUMP - FIRST_JUMP)) * SECOND_JUMP_HEIGHT;
		
		transform.position = new Vector3(transform.position.x, new_y, transform.position.z);
		
	}

    public bool isnotnear()
    {
        bool near = (Mathf.Abs(_position.x - player_last_seen.x) < 0.2f &&
                     Mathf.Abs(_position.y - player_last_seen.y) < 0.2f);
        // if (near) Debug.Log("search destination reached" + player_last_seen + " from pos " + _position);
        return !near;
    }
}


public class ENEMY_MATH
{
    public static float get_absolute_angle(float x, float y)
    {
        float angle = 0.0f;

        if (x > 0.0f)
        {
            angle = Mathf.Atan(y / x);
        }
        else if (x < 0.0f)
        {
            angle = Mathf.Atan(y / x) + 3.14159f;
        }
        else
        {
            if (y > 0.0f) angle = 3.14159f * 0.5f; else angle = 3.14159f * 1.5f;
        }
        if (angle < 0.0f) angle += 2.0f * 3.14159f;
        return angle * 180.0f / 3.14159f;
    }


}
