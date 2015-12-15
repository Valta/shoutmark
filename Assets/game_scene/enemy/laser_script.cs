using UnityEngine;
using System.Collections;

public class laser_script : MonoBehaviour
{
	private const int MAX_NUMBER_OF_LASERS = 20;
	private const float SPEED = 4.0f;
	private const float DISTANCE = 0.6f;
	
	public GameObject laser;
	private Transform[] lasers = new Transform[MAX_NUMBER_OF_LASERS];
	private bool[] laser_active = new bool[MAX_NUMBER_OF_LASERS];
	private float[] laser_direction = new float[MAX_NUMBER_OF_LASERS];
	private _TILEMAP tilemap;



	void Start()
	{
		tilemap = GameObject.Find("_GLOBAL_SCRIPTS").GetComponent<_TILEMAP>();
		initialize_lasers();
	}



	public void initialize_lasers()
	{
		/*
		bool destroying_lasers = true;
		while (destroying_lasers)
		{
			destroying_lasers = false;
			GameObject old_lasers = GameObject.Find("laser_parent(Clone)");
			if (old_lasers)
			{
				Destroy(old_lasers);
				//destroing_lasers = true;
			}
		}
		*/
		
		for (int a = 0; a < MAX_NUMBER_OF_LASERS; a++)
		{
			GameObject new_laser = (GameObject)Instantiate(	laser,
															new Vector3(1000.0f, 1000.0f, 1000.0f),
															Quaternion.identity);
			lasers[a] = new_laser.transform;
			laser_active[a] = false;
		}
	}



	public void shoot(Vector3 position, float direction)
	{
		for (int b = 0; b < 2; b++)
		{
			int index = -1;
			for (int a = 0; a < MAX_NUMBER_OF_LASERS; a++)
			{
				if (!laser_active[a]) index = a;
			}
			if (index >= 0)
			{
				laser_active[index] = true;
				laser_direction[index] = direction;
				lasers[index].position =
					new Vector3(position.x + (b - 0.5f) * DISTANCE * Mathf.Cos((direction + 90.0f) * 3.14159f / 180.0f),
								0.0f,
								position.z - (b - 0.5f) * DISTANCE * Mathf.Sin((direction + 90.0f) * 3.14159f / 180.0f));
				lasers[index].rotation = Quaternion.Euler(0.0f, laser_direction[index], 0.0f);
			}
			else
			{
				Debug.Log("OUT OF LASERS!");
			}
		}
	}



	private void destroy_laser(int index)
	{
		laser_active[index] = false;
		lasers[index].position = new Vector3(1000.0f, 1000.0f, 1000.0f);
	}



	void Update()
	{
		float dt = _TIMER.deltatime();
		for (int a = 0; a < MAX_NUMBER_OF_LASERS; a++)
		{
			if (laser_active[a])
			{
				lasers[a].position += new Vector3(	SPEED * dt * Mathf.Cos(laser_direction[a] * 3.14159f / 180.0f),
													0.0f,
													-SPEED * dt * Mathf.Sin(laser_direction[a] * 3.14159f / 180.0f));
				if (!tilemap.can_a_tile_be_pushed_here((int)lasers[a].position.x, (int)(-lasers[a].position.z)))
				{
					destroy_laser(a);
				}
				float player_x = 0;
				float player_y = 0;
				float player_radius = 0;
				tilemap.get_player_information(ref player_x, ref player_y, ref player_radius);
				if (lasers[a].position.x > player_x - player_radius &&
					lasers[a].position.x < player_x + player_radius &&
					lasers[a].position.z > player_y - player_radius &&
					lasers[a].position.z < player_y + player_radius)
				{
					player_script player = GameObject.Find("player(Clone)").GetComponent<player_script>();
					player.laser_hit();
					destroy_laser(a);
				}
			}
		}
	}



}
