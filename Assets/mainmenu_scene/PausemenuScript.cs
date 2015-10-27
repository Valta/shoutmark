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
			Debug.Log("game is paused");
		}
		else
		{
			_TIMER.set_pause(false);
			PausemenuPanel.gameObject.SetActive(false);
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
