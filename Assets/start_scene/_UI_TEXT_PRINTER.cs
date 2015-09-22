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
	private const int LETTERSHEET_WIDTH = 736;
	private const int LETTERSHEET_HEIGHT = 880;
	
	private RawImage[] letter_rawimage = new RawImage[MAX_NUMBER_OF_LETTERS];
	private RectTransform[] letter = new RectTransform[MAX_NUMBER_OF_LETTERS];
	private bool[] letter_free = new bool[MAX_NUMBER_OF_LETTERS];
	private int letter_check_index = 0;
	
	private Rect[] letter_source_rectangle = new Rect[256];
	
	private float screen_width = 0.0f;
	private float screen_height = 0.0f;
	private float letter_width = 0.0f;
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
			
			letter[a].sizeDelta = new Vector2(0.0f, 0.0f);
			letter[a].anchoredPosition = new Vector2(1000.0f, 1000.0f);
			
			letter[a].localScale = new Vector3(1.0f, 1.0f, 1.0f);
			letter_rawimage[a].uvRect = letter_source_rectangle[0];
			letter_rawimage[a].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			letter_free[a] = true;
		}
	}



	private void get_screen_size()
	{
		screen_width = Screen.width;
		screen_height = Screen.height;
		screen_top = screen_height * 0.5f;
		screen_left = -screen_width * 0.5f;
	}


	// 736*880 -> 46*55

	private void calculate_letter_rectangles()
	{
		for (int x = 0; x < 16; x++)
			for (int y = 0; y < 16; y++)
				letter_source_rectangle[(15 - y) * 16 + x] =
					new Rect(	(x * (LETTERSHEET_WIDTH / 16.0f) + 1.0f) / LETTERSHEET_WIDTH, 
								(y * (LETTERSHEET_HEIGHT / 16.0f) + 1.0f) / LETTERSHEET_HEIGHT,
								((LETTERSHEET_WIDTH / 16.0f) - 2.0f) / LETTERSHEET_WIDTH,
								((LETTERSHEET_HEIGHT / 16.0f) - 2.0f) / LETTERSHEET_HEIGHT);
	}


	int TEMP_text_y = -100;
	void Update()
	{
		if (PRINT.ui_text_printer_get_message() != "")
		{
			display_text(PRINT.ui_text_printer_get_message(), -160, TEMP_text_y, 15,
						new Color(Random.Range(0.5f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.5f, 1.0f)));
			TEMP_text_y += 15;
			if (TEMP_text_y > 90) TEMP_text_y -= 193;
			PRINT.ui_text_printer_clear_message();
		}
	}



	private void display_text(string text, int x, int y, int font_size, Color font_color)
	{
		get_screen_size();
		
		float next_letter_size_y = screen_height * font_size * 0.005f;
		float next_letter_size_x = next_letter_size_y * (LETTERSHEET_WIDTH - 32.0f) / (LETTERSHEET_HEIGHT - 32.0f);
		
		float next_letter_x = screen_height * x * 0.005f + next_letter_size_x * 0.5f;
		float next_letter_y = screen_height * y * -0.005f - next_letter_size_y * 0.5f;
		
		for (int a = 0; a < text.Length; a++)
		{
			letter[letter_check_index].sizeDelta = new Vector2(next_letter_size_x, next_letter_size_y);
			letter[letter_check_index].anchoredPosition = new Vector2(next_letter_x, next_letter_y);
			letter_rawimage[letter_check_index].uvRect = letter_source_rectangle[text[a]];
			letter_rawimage[letter_check_index].color = font_color;
			
			if (text[a] == 'M' || text[a] == 'm' || text[a] == 'W'|| text[a] == 'w')
				next_letter_x += next_letter_size_x * 1.0f;
			else
				next_letter_x += next_letter_size_x * 0.9f;
			
			letter_check_index++;
			if (letter_check_index >= MAX_NUMBER_OF_LETTERS) letter_check_index = 0;
		}
	}


}
