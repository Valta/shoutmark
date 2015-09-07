using UnityEngine;
using System.Collections;

public class radar_script : MonoBehaviour
{
	private _TILEMAP tilemap;
	public GameObject radar_line;
	
	private const int NUMBER_OF_LINES = 20;
	private float direction = 45.0f;
	private LineRenderer[] lines = new LineRenderer[NUMBER_OF_LINES];
	private Vector3[] line_end = new Vector3[NUMBER_OF_LINES - 1];



	void Start()
	{
		tilemap = GameObject.Find("_GLOBAL_SCRIPTS").GetComponent<_TILEMAP>();
		
		for (int a = 0; a < NUMBER_OF_LINES; a++)
		{
			GameObject line = (GameObject)Instantiate(radar_line, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
			line.transform.SetParent(transform);
			lines[a] = line.GetComponent<LineRenderer>();
			lines[a].SetPosition(0, new Vector3(0.0f, 0.0f, 0.0f));
			lines[a].SetPosition(1, new Vector3(0.0f, 0.0f, 0.0f));
		}
	}


	void Update()
	{
		
		raycast_radar(transform.position.x, transform.position.z, direction, 90.0f);
		direction += 90.0f / (NUMBER_OF_LINES - 1);


	}



	private void raycast_radar(float center_x, float center_y, float center_direction, float width)
	{
		float HEIGHT = 0.8f;
		float STEP = 0.1f;
		float MAX_STEPS = 40;
		
		for (int a = 0; a < NUMBER_OF_LINES - 1; a++)
		{
			float x = center_x;
			float y = center_y;
			int steps = 0;
			float dir = center_direction + width * ((float)a - (NUMBER_OF_LINES - 1.0f) * 0.5f) / (float)(NUMBER_OF_LINES - 1);
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
					if (steps > 0)			// the ray is inside a wall,
					{						// take it back one step.
						x -= step_x;		//
						y -= step_y;		//
					}						//
				}
				if (steps > MAX_STEPS) raycasting = false;
			}
			line_end[a] = new Vector3(x, HEIGHT, y);
		}
		
		for (int a = 0; a < NUMBER_OF_LINES; a++)
		{
			if (a == 0)
				lines[a].SetPosition(0, new Vector3(center_x, HEIGHT, center_y));
			else
				lines[a].SetPosition(0, line_end[a - 1]);
			if (a == NUMBER_OF_LINES - 1)
				lines[a].SetPosition(1, new Vector3(center_x, HEIGHT, center_y));
			else
				lines[a].SetPosition(1, line_end[a]);
		}
	}
}
