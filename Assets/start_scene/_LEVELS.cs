using UnityEngine;
using System.Collections;

public class _LEVELS : MonoBehaviour
{
    public const int TOTAL_LEVELS = 3;

	void Start()
	{
		get_level_data(0);
	}



	public void load_level(int level_index, ref int level_width, ref int level_height, int[,] map)
	{
		string[] data = get_level_data(level_index);
		
		level_width = data[0].Length;				// read the width and
		level_height = data.Length;					// height of the map.
		
		for (int x = 0; x < level_width; x++)		// this is the map parser.
		{
			for (int y = 0; y < level_height; y++)
			{
				map[x, y] = _TILEMAP.TILE_FLOOR;
				if (data[y][x] == ' ') map[x, y] = _TILEMAP.TILE_START_AREA;
				if (data[y][x] == 'B') map[x, y] = _TILEMAP.TILE_BLOCK;
				if (data[y][x] == 'T') map[x, y] = _TILEMAP.TILE_TRANSPARENTWALL;
				if (data[y][x] == 'W') map[x, y] = _TILEMAP.TILE_WIREFRAMESPRITE;
				if (data[y][x] == 't') map[x, y] = _TILEMAP.TILE_TESTCUBE;
				if (data[y][x] == 'P') map[x, y] = _TILEMAP.TILE_PUSHABLE;
				if (data[y][x] == 'G') map[x, y] = _TILEMAP.TILE_GOAL;
				if (data[y][x] == 's') map[x, y] = _TILEMAP.START;
				if (data[y][x] == 'e') map[x, y] = _TILEMAP.ENEMY;
			}
		}
	}



	private string[] get_level_data(int level_index)
	{
        switch (level_index)
        {
            case (0):
                {
                        string[] level =
			        {
				        "TTTTTTTTTTTTTT",
				        "T------------T",
				        "T---------G--T",
				        "Tt-e-TTTT----T",
				        "T-------t----T",
				        "T-------tt---T",
				        "TTTTTt---te--T",
				        "T    t---t---T",
				        "Ts   t---t---T",
				        "T    --G-----T",
				        "T    --------T",
				        "TTTTTTTTTTTTTT"
			        };
                    return level;
                }
            case(1):
                {
                    string[] level =
			        {
				        "TTTTTTTTTTTTTTT",
                        "T-------------T",
				        "T-------------T",
				        "T--t---t------T",
				        "T--t---tBG----T",
				        "Te---------B--T",
				        "T--t  st--G---T",
				        "T--t   t------T",
				        "T---ttt---e-B-T",
				        "T---G---------T",
                        "T-------------T",
				        "TTTTTTTTTTTTTTT"
			        };
                    return level;
                }
            case(2):
                {
                    string[] level =
			        {
				        "TTTTTTTTTTTTTT",
                        "T------------T",
				        "T------------T",
				        "T--t---t-----T",
				        "T--t---tBG---T",
				        "T--e--    -B-T",
				        "T--t-G s  P--T",
				        "T--te-    ---T",
				        "T---ttt---e-BT",
                        "T------------T",
				        "T------------T",
				        "TTTTTTTTTTTTTT"
			        };
                    return level;
                }
            default:
                {
                    string[] level =
                    {
                        "TTTTT",
                        "T---T",
                        "T-s-T",
                        "T---T",
                        "TTTTT"
                    };
                    return level;
                    //Debug.Log("error! bad level index: " + level_index);
                    //string[] invalid_level_index = {""};
                    //return invalid_level_index;                    
                }
        }


		

	}
	
}
