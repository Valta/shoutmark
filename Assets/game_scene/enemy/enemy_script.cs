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
    private radar_script radar;
	private float dt = 0.0f;
	
	private float x = 0.0f;
	private float y = 0.0f;
	private float speed_x = 0.0f;
	private float speed_y = 0.0f;
    private float direction = 90.0f;
	private int id = -1;

    private bool looking;
    public void SetLooking(bool is_looking) { looking = is_looking; }
    public void SetDirection(float target_x, float target_y)
    { direction = ENEMY_MATH.get_absolute_angle(target_x - x, target_y - y); }
    public void SetDirection(float angle) { direction = angle; }
    public float GetDirectionAngle() { return direction; }



	void Start()
	{
       
        x = transform.position.x;
		y = transform.position.z;
		
		model = transform.FindChild("enemy_model_temp");
		model_material = model.gameObject.GetComponent<Renderer>().material;
		tilemap = GameObject.Find("_GLOBAL_SCRIPTS").GetComponent<_TILEMAP>();
        radar = gameObject.GetComponent<radar_script>();
		id = tilemap.add_to_object_list(x, y, ENEMY_RADIUS);
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
        rotate_model();
        move_enemy();
		scroll_texture();
		update_collision_jump();
		
		tilemap.update_object_data(id, x, y);
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
