using UnityEngine;
using System.Collections;

public class PausemenuScript : MonoBehaviour {


	public static PausemenuScript instance;
	
	private static bool pauseManagerHasSpawned = false;

	public Canvas PauseMenuCanvas;
	public RectTransform PausemenuPanel;
	public RectTransform PauseOptionsPanel;

	void Awake()
	{
		if (pauseManagerHasSpawned == false)
		{
			pauseManagerHasSpawned = true;
			instance = this;
			DontDestroyOnLoad(gameObject);
			
		}
		else
		{
			// This deletes new objects of same type when loading
			// new scenes but keeps the first one
			DestroyImmediate(gameObject);
		}
		
		if (PausemenuPanel)
		{
			PausemenuPanel.gameObject.SetActive(false);
		}
		if (PauseOptionsPanel)
		{
			PauseOptionsPanel.gameObject.SetActive(false);
		}
		
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.M))
		{
			if (Application.loadedLevel != 0 && !PauseOptionsPanel.gameObject.activeSelf) // gameobject.active changed to activeSelf (unity update reasons)
			{
				PauseGame();
			}
		}
	
	}

	public void PauseGame()
	{
		if (!_TIMER.paused())
		{
			_TIMER.set_pause (true);
			PausemenuPanel.gameObject.SetActive(true);
            // set instructions
            MESSAGE.print("Press N for next level", -170, -60, 9, 15, 80);
            MESSAGE.print("Press U to restart", -170, -40, 9, 15, 81);
            MESSAGE.print("B", -170, -20, 9, 15, 82);
            MESSAGE.print("C", -170, 0, 9, 15, 83);
            MESSAGE.print("D", -170, 20, 9, 15, 84);
            MESSAGE.print("E", -170, 40, 9, 15, 85);
			Debug.Log("game is paused");
		}
		else
		{
			_TIMER.set_pause(false);
			PausemenuPanel.gameObject.SetActive(false);
            MESSAGE.print("", -170, -90, 9, 15, 80);
            MESSAGE.print("", -170, -70, 9, 15, 81);
            MESSAGE.print("", -170, -50, 9, 15, 82);
            MESSAGE.print("", -170, -30, 9, 15, 83);
            MESSAGE.print("", -170, -10, 9, 15, 84);
            MESSAGE.print("", -170, 10, 9, 15, 85);
			Debug.Log("game is not paused");
		}
	}

	public void GoToOptions()
	{
		if (PausemenuPanel)
		{
			PausemenuPanel.gameObject.SetActive(false);
		}
		if (PauseOptionsPanel)
		{
			PauseOptionsPanel.gameObject.SetActive(true);
		}

	}

	public void GoBackFromOptions()
	{
		if (PauseOptionsPanel)
		{
			PauseOptionsPanel.gameObject.SetActive(false);
		}
		if (PausemenuPanel)
		{
			PausemenuPanel.gameObject.SetActive(true);
		}
		
	}
	
	public void GoToMainMenu()
	{
		if (PauseOptionsPanel)
		{
			PauseOptionsPanel.gameObject.SetActive(false);
		}
		if (PausemenuPanel)
		{
			PausemenuPanel.gameObject.SetActive(false);
		}

		if (Application.loadedLevel != 0)
		{
			_TIMER.set_pause(false);
			Application.LoadLevel(0);
		}
		else
		{
			Debug.Log("Your current scene is not in the build, add it");
		}
		
	}
}
