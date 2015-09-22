using UnityEngine;
using System.Collections;

public class enemy_script : MonoBehaviour
{
	private const float SPEED = 1.0f;
	private const float TEXTURE_SPEED = 7.5f;
	private const float ENEMY_RADIUS = 0.45f;
	
	private Transform model;
	private Material model_material;
	private _TILEMAP tilemap;
	private float dt = 0.0f;
	
	private float x = 0.0f;
	private float y = 0.0f;
	private float speed_x = 0.0f;
	private float speed_y = 0.0f;
	private int id = -1;

    private bool roaming;
    public void SetRoaming(bool is_roaming) { roaming = is_roaming; }

	void Start()
	{
		x = transform.position.x;
		y = transform.position.z;
		
		model = transform.FindChild("enemy_model_temp");
		model_material = model.gameObject.GetComponent<Renderer>().material;
		tilemap = GameObject.Find("_GLOBAL_SCRIPTS").GetComponent<_TILEMAP>();
		
		id = tilemap.add_to_object_list(x, y, ENEMY_RADIUS);
	}



	void Update()
	{
		dt =Time.deltaTime;
		
		//apply_numpad_input();
		if (roaming)
            apply_ai();
		
		move_enemy();
		rotate_model();
		scroll_texture();
		update_collision_jump();
		
		tilemap.update_object_data(id, x, y);
	}



	private void apply_numpad_input()
	{
		speed_x = 0.0f;
		speed_y = 0.0f;
		if (Input.GetKey(KeyCode.Keypad8)) speed_y = SPEED;
		if (Input.GetKey(KeyCode.Keypad5)) speed_y = -SPEED;
		if (Input.GetKey(KeyCode.Keypad4)) speed_x = -SPEED;
		if (Input.GetKey(KeyCode.Keypad6)) speed_x = SPEED;
	}



	private int ai_direction = 0;
	private void apply_ai()
	{
		if (speed_x == 0.0f && speed_y == 0.0f)
		{
			ai_direction++;
			if (ai_direction > 3) ai_direction = 0;
			
			if (ai_direction == 0)
			{
				speed_x = 0.0f;
				speed_y = SPEED;
			}
			if (ai_direction == 1)
			{
				speed_x = SPEED;;
				speed_y = 0.0f;
			}
			if (ai_direction == 2)
			{
				speed_x = 0.0f;
				speed_y = -SPEED;
			}
			if (ai_direction == 3)
			{
				speed_x = -SPEED;;
				speed_y = 0.0f;
			}
		}
	}



	private void move_enemy()
	{
		if (speed_y > 0.0f)
		{
			if (tilemap.can_move(x - ENEMY_RADIUS, y + ENEMY_RADIUS + speed_y * dt) &&
				tilemap.can_move(x, y + ENEMY_RADIUS + speed_y * dt) &&
				tilemap.can_move(x + ENEMY_RADIUS, y + ENEMY_RADIUS + speed_y * dt)) y += speed_y * dt;
			else
				speed_y = 0.0f;
		}
		if (speed_y < 0.0f)
		{
			if (tilemap.can_move(x - ENEMY_RADIUS, y - ENEMY_RADIUS + speed_y * dt) &&
				tilemap.can_move(x, y - ENEMY_RADIUS + speed_y * dt) &&
				tilemap.can_move(x + ENEMY_RADIUS, y - ENEMY_RADIUS + speed_y * dt)) y += speed_y * dt;
			else
				speed_y = 0.0f;
		}
		if (speed_x < 0.0f)
		{
			if (tilemap.can_move(x - ENEMY_RADIUS + speed_x * dt, y + ENEMY_RADIUS) &&
				tilemap.can_move(x - ENEMY_RADIUS + speed_x * dt, y) &&
				tilemap.can_move(x - ENEMY_RADIUS + speed_x * dt, y - ENEMY_RADIUS)) x += speed_x * dt;
			else
				speed_x = 0.0f;
		}
		if (speed_x > 0.0f)
		{
			if (tilemap.can_move(x + ENEMY_RADIUS + speed_x * dt, y + ENEMY_RADIUS) &&
				tilemap.can_move(x + ENEMY_RADIUS + speed_x * dt, y) &&
				tilemap.can_move(x + ENEMY_RADIUS + speed_x * dt, y - ENEMY_RADIUS)) x += speed_x * dt;
			else
				speed_x = 0.0f;
		}
		
		transform.position = new Vector3(x, transform.position.y, y);
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
		model_material.SetTextureOffset("_MainTex", new Vector2(0.0f, step / 12.0f));
	}



	private float jump_timer = 999.0f;
	private void update_collision_jump()
	{
		jump_timer += Time.deltaTime;
		if (speed_x == 0.0f && speed_y == 0.0f) jump_timer = 0.0f;
		
		float FIRST_JUMP = 0.4f;
		float SECOND_JUMP = 0.7f;
		
		float FIRST_JUMP_HEIGHT = 0.4f;
		float SECOND_JUMP_HEIGHT = 0.2f;
		
		float new_y = 0.0f;
		
		if (jump_timer < FIRST_JUMP)
			new_y = Mathf.Sin(jump_timer * 3.14159f / FIRST_JUMP) * FIRST_JUMP_HEIGHT;
		else if (jump_timer >= FIRST_JUMP && jump_timer < SECOND_JUMP)
			new_y = Mathf.Sin((jump_timer - FIRST_JUMP) * 3.14159f / (SECOND_JUMP - FIRST_JUMP)) * SECOND_JUMP_HEIGHT;
		
		transform.position = new Vector3(transform.position.x, new_y, transform.position.z);
		
	}


}
