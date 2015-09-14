using UnityEngine;
using System.Collections;


public class ui_test : MonoBehaviour
{
	public GameObject test_piece;
	private RectTransform test;
	private float x = 0.0f;
	private float y = 0.0f;

	private const int MAX_NUMBER_OF_LETTERS = 10;
	private RectTransform[] letter = new RectTransform[MAX_NUMBER_OF_LETTERS];



	private float screen_width = 0.0f;
	private float screen_height = 0.0f;
	private float letter_size = 0.0f;
	private float screen_top = 0.0f;
	private float screen_left = 0.0f;



	void Start()
	{
		get_screen_size();
		
		test = GameObject.Find("Canvas").transform.FindChild("RawImage").GetComponent<RectTransform>();
		
		Transform canvas = GameObject.Find("Canvas").transform;
		for (int a = 0; a < MAX_NUMBER_OF_LETTERS; a++)
		{
			GameObject new_letter = (GameObject)Instantiate(test_piece, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
			new_letter.transform.SetParent(canvas);
			letter[a] = new_letter.GetComponent<RectTransform>();
			letter[a].localScale = new Vector3(1.0f, 1.0f, 1.0f);
			letter[a].anchoredPosition = new Vector2(screen_left + (a % 3) * letter_size, screen_top - (int)(a / 3) * letter_size);
			letter[a].sizeDelta = new Vector2(letter_size, letter_size);
		}
		
		/*
		aaaa.transform.SetParent(GameObject.Find("Canvas").transform);
		RectTransform bbbb = aaaa.GetComponent<RectTransform>();
		bbbb.anchoredPosition = new Vector2(-200, 100);
		
		test.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		bbbb.localScale = new Vector3(1.0f, 1.0f, 1.0f); // scale ykköseksi niin sizedelta määrää pikselikoon.
		test.sizeDelta = new Vector2(200, 200);
		*/
	}



	private void get_screen_size()
	{
		screen_width = Screen.width;
		screen_height = Screen.height;
		letter_size = screen_height * 0.1f;
		screen_top = screen_height * 0.5f;
		screen_left = -screen_width * 0.5f;
	}



	void Update()
	{
		//Debug.Log("position:"+test.position+ "offset:"+test.offsetMin+","+test.offsetMax);
		//Debug.Log("(x, y)=("+x+", "+y+")");
		string aaa = "abcd ABCD";
		//for(int a = 0; a < 9; a++) Debug.Log("merkki="+aaa[a]+"  numero="+(int)aaa[a]);
		
		apply_keyboard_input();
		
		float x_on_screen = x * Screen.width * 0.5f;
		float y_on_screen = y * Screen.height * 0.5f;
		
		test.anchoredPosition = new Vector2(x_on_screen, y_on_screen);
	}



	private void apply_keyboard_input()
	{
		if (Input.GetKey(KeyCode.Keypad8)) y += 0.01f;
		if (Input.GetKey(KeyCode.Keypad5)) y -= 0.01f;
		if (Input.GetKey(KeyCode.Keypad4)) x -= 0.01f;
		if (Input.GetKey(KeyCode.Keypad6)) x += 0.01f;
	}
}
