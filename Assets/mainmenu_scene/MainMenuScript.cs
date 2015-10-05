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
//		if (levelNumber < EditorBuildSettings.scenes.Length) // this may not work outside unity editor and may cause problems
//		{
//			//SwitchToPanel(3); //loading screen panel if we have one
//			Application.LoadLevel(levelNumber);
//		}
//		else
//		{
//			Debug.Log("Selected level is not in the build, add it");
//		}
		Application.LoadLevel(levelNumber);
		
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
