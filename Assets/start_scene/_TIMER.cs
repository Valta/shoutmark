using UnityEngine;
using System.Collections;

public class _TIMER : MonoBehaviour
{
	private static bool game_paused = false;
	private static float time_since_start = 0.0f;


	public static float time() {return time_since_start;}
	public static bool paused() {return game_paused;}
	public static void set_pause(bool is_paused_or_not) {game_paused = is_paused_or_not;}



	void Update()
	{
		if (!game_paused)time_since_start += Time.deltaTime;
	}



	public static float deltatime()
	{
		if (!game_paused)
			return Time.deltaTime;
		else
			return 0.0f;
	}


}