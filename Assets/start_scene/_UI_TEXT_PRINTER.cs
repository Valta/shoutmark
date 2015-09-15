using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class PRINT
{
	private static string next_message = "";
	
	
	
	public static void report(string message)
	{
		next_message = message;
	}
	
	
	public static string ui_text_printer_get_message()
	{
		return next_message;
	}
	
	
	
	public static void ui_text_printer_clear_message()
	{
		next_message = "";
	}
}



public class _UI_TEXT_PRINTER : MonoBehaviour
{
	public GameObject letter_prefab;
	
	private const int MAX_NUMBER_OF_LETTERS = 1000;
	private RawImage[] letter_rawimage = new RawImage[MAX_NUMBER_OF_LETTERS];
	private RectTransform[] letter = new RectTransform[MAX_NUMBER_OF_LETTERS];
	private Rect[] letter_source_rectangle = new Rect[256];
	
	private float screen_width = 0.0f;
	private float screen_height = 0.0f;
	private float letter_width = 0.0f;
	private float letter_height = 0.0f;
	private float screen_top = 0.0f;
	private float screen_left = 0.0f;



	void Start()
	{
		calculate_letter_rectangles();
		instantiate_letter_prefabs();
	}



	private void instantiate_letter_prefabs()
	{
		get_screen_size();
		Transform letter_parent = GameObject.Find("Canvas").transform.FindChild("message_texts");
		
		for (int a = 0; a < MAX_NUMBER_OF_LETTERS; a++)
		{
			GameObject new_letter = (GameObject)Instantiate(letter_prefab,
															new Vector3(0.0f, 0.0f, 0.0f),
															Quaternion.identity);
			new_letter.transform.SetParent(letter_parent);
			
			letter_rawimage[a] = new_letter.GetComponent<RawImage>();
			letter[a] = new_letter.GetComponent<RectTransform>();
			
			
			letter[a].sizeDelta = new Vector2(letter_width, letter_height);
			letter[a].anchoredPosition = new Vector2(screen_left + (a % 12 + 0.5f) * letter_width, screen_top - ((int)(a / 12) + 0.5f) * letter_height);
			
			letter[a].localScale = new Vector3(1.0f, 1.0f, 1.0f);
			letter_rawimage[a].uvRect = letter_source_rectangle[(a + 48) % 128];
			letter_rawimage[a].color = new Color(	1.0f,
													Mathf.Repeat(a * 0.3f, 0.5f) + 0.5f,
													Mathf.Repeat(a * 0.13f, 0.5f) + 0.5f,
													Mathf.Repeat(a * 0.1f, 1));
			//Debug.Log("asldkj:"+letter_rawimage[a].uvRect);
		}
	}



	private void get_screen_size()
	{
		screen_width = Screen.width;
		screen_height = Screen.height;
		letter_height = screen_height * 0.1f;
		letter_width = letter_height * (24.0f / 32.0f);
		screen_top = screen_height * 0.5f;
		screen_left = -screen_width * 0.5f;
	}



	private void calculate_letter_rectangles()
	{
		float indent = 0.0005f;
		for (int x = 0; x < 16; x++)
			for (int y = 0; y < 8; y++)
				letter_source_rectangle[(7 - y) * 16 + x] =
					new Rect(	x / 16.0f + indent, 
								y / 8.0f + indent,
								1.0f / 16.0f - 2.0f * indent,
								1.0f / 8.0f - 2.0f * indent);
	}



	void Update()
	{
		if (PRINT.ui_text_printer_get_message() != "")
		{
			Debug.Log("THERE IS MESSAGE:"+PRINT.ui_text_printer_get_message());
			PRINT.ui_text_printer_clear_message();
		}
	}



}
