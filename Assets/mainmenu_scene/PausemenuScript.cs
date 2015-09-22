using UnityEngine;
using System.Collections;

public class PausemenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.M)) {
			PauseGame();
		}
	
	}

	public void PauseGame()
	{
		if (!_TIMER.paused())
		{
			_TIMER.set_pause (true);
			Debug.Log("game is paused");
		}
		else
		{
			_TIMER.set_pause(false);
			Debug.Log("game is not paused");
		}
	}
}
