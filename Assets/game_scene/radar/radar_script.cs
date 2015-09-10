using UnityEngine;
using System.Collections;

public class radar_script : MonoBehaviour
{
	private _TILEMAP tilemap;
	public GameObject radar_line;
	
	private const int NUMBER_OF_RAYS = 30;
	private float direction = 45.0f;
	private Vector3[] line_end = new Vector3[NUMBER_OF_RAYS];
	private bool[] max_distance_reached = new bool[NUMBER_OF_RAYS];
	private bool[] player_detected = new bool[NUMBER_OF_RAYS];
	
	private LineRenderer[] lines = new LineRenderer[NUMBER_OF_RAYS + 1];


	void Start()
	{
		tilemap = GameObject.Find("_GLOBAL_SCRIPTS").GetComponent<_TILEMAP>();
		
		for (int a = 0; a < NUMBER_OF_RAYS + 1; a++)
		{
			GameObject line = (GameObject)Instantiate(radar_line, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
			line.transform.SetParent(transform);
			lines[a] = line.GetComponent<LineRenderer>();
			lines[a].SetPosition(0, new Vector3(0.0f, 0.0f, 0.5f));
			lines[a].SetPosition(1, new Vector3(0.0f, 0.0f, 0.8f));
		}
	}



	void Update()
	{
		
		raycast_radar(transform.position.x, transform.position.z, direction, 3.0f, 90.0f);
		direction += Time.deltaTime * 90.0f;
	}



	private void raycast_radar(float center_x, float center_y, float direction, float length, float width_in_degrees)
	{
		float HEIGHT = 0.8f;
		//bool player_detected = false;
		for (int a = 0; a < NUMBER_OF_RAYS; a++) player_detected[a] = false;
		
		//shoot_rays(center_x, center_y, direction, length, width_in_degrees, HEIGHT, ref player_detected);
		shoot_rays(center_x, center_y, direction, length, width_in_degrees, HEIGHT, player_detected);
		smooth_radar_edges();
		set_line_positions(center_x, center_y, HEIGHT);
		
		/*
		if (player_detected)
			set_line_colors(new Color(1.0f, 0.0f, 0.0f, 0.9f));
		else
			set_line_colors(new Color(0.0f, 1.0f, 0.0f, 0.9f));
		*/
		set_line_colors(new Color(0.0f, 1.0f, 0.0f, 1.0f));
	}



	private void shoot_rays(float center_x,
							float center_y,
							float center_direction,
							float max_length,
							float width_in_degrees,
							float height,
							//ref bool player_detected)
							bool[] player_detected)
		{
		float STEP = 0.1f;
		int MAX_STEPS = (int)(max_length / STEP);
		
		float angle_resolution = width_in_degrees / (NUMBER_OF_RAYS - 1.0f);
		center_direction = (int)(center_direction / angle_resolution) * angle_resolution;
		
		float player_x = 0.0f;
		float player_y = 0.0f;
		float player_radius = 0.0f;
		tilemap.get_player_information(ref player_x, ref player_y, ref player_radius);
		
		for (int a = 0; a < NUMBER_OF_RAYS; a++)
		{
			line_end[a] = new Vector3(center_x, height, center_y);
			max_distance_reached[a] = false;
			float x = center_x;
			float y = center_y;
			int steps = 0;
			float dir = center_direction + width_in_degrees * ((float)a - (NUMBER_OF_RAYS - 1.0f) * 0.5f) / (float)(NUMBER_OF_RAYS - 1);
			float step_x = Mathf.Cos(dir * 3.14159f / 180.0f) * STEP;
			float step_y = Mathf.Sin(dir * 3.14159f / 180.0f) * STEP;
			
			bool raycasting = true;
			while (raycasting)
			{
				if (tilemap.get_tile((int)(x), (int)(-y)) == _TILEMAP.TILE_FLOOR)
				{
					x += step_x;
					y += step_y;
					steps++;
				}
				else
				{
					raycasting = false;
					if (steps > 1)
					{
						x -= step_x ;		// one step back.
						y -= step_y ;		//
					}
				}
				if (x > player_x - player_radius && x < player_x + player_radius &&
					y > player_y - player_radius && y < player_y + player_radius)
				{
					x -= step_x ;		// one step back.
					y -= step_y ;		//
					raycasting = false;
					//player_detected = true;
					player_detected[a] = true;
				}
				if (steps > MAX_STEPS)
				{
					raycasting = false;
					max_distance_reached[a] = true;
				}
			}
			line_end[a] = new Vector3(x, height, y);
		}
	}



	private void smooth_radar_edges()
	{
		float SMOOTH_EDGE_DISTANCE = 0.06f;
		
		for (int a = 0; a < NUMBER_OF_RAYS; a++)
		{
			if (!max_distance_reached[a])
			{
				float X = Mathf.Floor(line_end[a].x);
				float Y = Mathf.Floor(line_end[a].z);
				float x = line_end[a].x - X;
				float y = line_end[a].z - Y;
				if (x > 0.0f && x < SMOOTH_EDGE_DISTANCE * 2.0f) line_end[a].x = X + SMOOTH_EDGE_DISTANCE;
				if (y > 0.0f && y < SMOOTH_EDGE_DISTANCE * 2.0f) line_end[a].z = Y + SMOOTH_EDGE_DISTANCE;
				if (x < 1.0f && x > 1.0f - SMOOTH_EDGE_DISTANCE * 2.0f) line_end[a].x = X + 1.0f - SMOOTH_EDGE_DISTANCE;
				if (y < 1.0f && y > 1.0f - SMOOTH_EDGE_DISTANCE * 2.0f) line_end[a].z = Y + 1.0f - SMOOTH_EDGE_DISTANCE;
			}
		}
	}



	private void set_line_positions(float center_x, float center_y, float height)
	{
		/*
		for (int a = 0; a < NUMBER_OF_RAYS + 1; a++)
		{
			if (a == 0)
				lines[a].SetPosition(0, new Vector3(center_x, height, center_y));
			else
				lines[a].SetPosition(0, line_end[a - 1]);
			
			if (a == NUMBER_OF_RAYS)
				lines[a].SetPosition(1, new Vector3(center_x, height, center_y));
			else
				lines[a].SetPosition(1, line_end[a]);
		}
		*/
		for (int a = 0; a < NUMBER_OF_RAYS; a++)
		{
			lines[a].SetPosition(0, new Vector3(center_x, height, center_y));
			lines[a].SetPosition(1, line_end[a]);
		}
		
	}



	private void set_line_colors(Color color)
	{
		for (int a = 0; a < NUMBER_OF_RAYS + 0; a++)
		{
			if (player_detected[a])
				lines[a].material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
			else
				lines[a].material.color = new Color(color.r, color.g, color.b, color.a * a / NUMBER_OF_RAYS);
		}
	}
	



}
