using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour {

	public Canvas Canvas;
	public RectTransform[] Panels;

	// Use this for initialization
	void Start () {
		Canvas.gameObject.SetActive(true);
		for (int i = 0; i < Panels.Length; i++)
		{
			Panels[i].gameObject.SetActive(false);
		}
		Panels[0].gameObject.SetActive(true);
		//EventSystem.current.SetSelectedGameObject(GameObject.Find("StartButton"));
	
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
		if (levelNumber < EditorBuildSettings.scenes.Length)
		{
			//SwitchToPanel(3); //loading screen panel if we have one
			Application.LoadLevel(levelNumber);
		}
		
	}

	public void ExitGame()
	{
		Application.Quit(); // doesnt work in editor, only in build
	}

}
