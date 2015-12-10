using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using UnityEditor;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour {

	public Canvas Canvas;
	public RectTransform[] Panels;

	public int LevelsUnlocked = 1; // will be moved to GameManager
	public Button[] LevelButtons;

    private _TILEMAP tilemap;

	// Use this for initialization
	void Start () {
		Canvas.gameObject.SetActive(true);
		for (int i = 0; i < Panels.Length; i++)
		{
			Panels[i].gameObject.SetActive(false);
		}
		Panels[0].gameObject.SetActive(true);
		//EventSystem.current.SetSelectedGameObject(GameObject.Find("StartButton"));

		for (int i = 0; i < LevelButtons.Length; i++)
		{
			LevelButtons[i].interactable = false;//gameObject.SetActive(false);
		}
		UnlockLevels (LevelsUnlocked);

        tilemap = GameObject.Find("_GLOBAL_SCRIPTS").GetComponent<_TILEMAP>();

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void SwitchToPanel(int panelNumber)
	{
		for (int i = 0; i < Panels.Length; i++)
		{
			Panels[i].gameObject.SetActive(false);
		}
		Panels[panelNumber].gameObject.SetActive(true);
		
	}
	
	public void Loadscene(int levelNumber)
	{
        tilemap.set_level(levelNumber-1); // levels start from 0 in code

		Application.LoadLevel(1); // gamescene
		
	}

	public void SetUnlockedLevel(int unlockedLevelNumber) // will be moved to GameManager
	{
		if (unlockedLevelNumber > LevelsUnlocked)
		{
			LevelsUnlocked = unlockedLevelNumber;
		}
	}

	public void UnlockLevels(int unlockedLevelnumber)
	{
		for (int i = 0; i < LevelsUnlocked; i++)
		{
			LevelButtons[i].interactable = true;//gameObject.SetActive(false);
		}

	}

	public void CheatUnlockLevels()
	{
		if (LevelsUnlocked < 9) // increase this number if there are more levels in the future
		{
			LevelsUnlocked++;
		}
		UnlockLevels (LevelsUnlocked);
	}

	public void ExitGame()
	{
		Application.Quit(); // doesnt work in editor, only in build
	}

}
