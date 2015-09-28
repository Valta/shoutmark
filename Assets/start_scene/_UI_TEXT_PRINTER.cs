using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class MESSAGE
{
	private const int MAX_NUMBER_OF_MESSAGES = 20;
	
	private static string[] message = new string[MAX_NUMBER_OF_MESSAGES];
	private static int[] message_x = new int[MAX_NUMBER_OF_MESSAGES];
	private static int[] message_y = new int[MAX_NUMBER_OF_MESSAGES];
	private static int[] message_color = new int[MAX_NUMBER_OF_MESSAGES];
	private static int[] message_id = new int[MAX_NUMBER_OF_MESSAGES];
	private static bool[] message_update = new bool[MAX_NUMBER_OF_MESSAGES];
	
	private static bool[] will_be_reported = new bool[MAX_NUMBER_OF_MESSAGES];
	private static int next_report_id = -2000000000;



	public static void print(string new_message, int x, int y, int color, int id)
	{
		//Debug.Log("PRINT: prosessoidaan "+new_message+" x="+x+" y="+y+" id="+id+" color="+color);
		bool new_id = true;
		
		for (int a = 0; a < MAX_NUMBER_OF_MESSAGES; a++)
		{
			if (message_id[a] == id)
			{
				new_id = false;
				
				if (message[a] != new_message) message_update[a] = true;
				message[a] = new_message;
				message_x[a] = x;
				message_y[a] = y;
				message_color[a] = color;
				//Debug.Log("PRINT: new message="+new_message+"  update="+message_update[a]);
			}
		}
		
		if (new_id)
		{
			int index = MAX_NUMBER_OF_MESSAGES - 1;
			for (int a = MAX_NUMBER_OF_MESSAGES - 2; a >= 0; a--)
			{
				if (message_id[a] == 0) index = a;
			}
			message[index] = new_message;
			message_x[index] = x;
			message_y[index] = y;
			message_color[index] = color;
			message_id[index] = id;
			message_update[index] = true;
			//Debug.Log("PRINT: new message="+new_message+"  NEW ID update="+message_update[index]);
		}
	}



	public static void report(string new_report, int color)
	{
		Debug.Log("report: prosessoidaan "+new_report);
		
		int index = MAX_NUMBER_OF_MESSAGES - 1;
		for (int a = MAX_NUMBER_OF_MESSAGES - 2; a >= 0; a--)
		{
			if (message_id[a] == 0) index = a;
		}
		message[index] = new_report;
		message_x[index] = 100;
		message_y[index] = 70;
		message_color[index] = color;
		message_id[index] = next_report_id;
		will_be_reported[index] = true;
		message_update[index] = false;
		Debug.Log("report: new message="+new_report+"  REPORT ID="+message_id[index]);
		
		next_report_id++;
	}



	public static int number_of_updated_messages()
	{
		int count = 0;
		for (int a = 0; a < MAX_NUMBER_OF_MESSAGES; a++)
		{
			if (message_update[a]) count++;
		}
		return count;
	}



	public static void get_updated_message(ref string new_message, ref int x, ref int y, ref int color, ref int id)
	{
		int index = MAX_NUMBER_OF_MESSAGES - 1;
		for (int a = MAX_NUMBER_OF_MESSAGES - 2; a >= 0; a--)
		{
			if (message_update[a]) index = a;
		}
		message_update[index] = false;
		new_message = message[index];
		x = message_x[index];
		y = message_y[index];
		color = message_color[index];
		id = message_id[index];
	}



	public static bool is_there_a_report()
	{
		for (int a = 0; a < MAX_NUMBER_OF_MESSAGES; a++)
		{
			if (will_be_reported[a]) return true;
		}
		return false;
	}



	public static void get_next_report(ref string report, ref int x, ref int y, ref int color, ref int id)
	{
		int min_value_index = 0;
		int min_value = 0;
		for (int a = 0; a < MAX_NUMBER_OF_MESSAGES; a++)
		{
			if (will_be_reported[a])
			{
				if (min_value > message_id[a])
				{
					min_value_index = a;
					min_value = message_id[a];
				}
			}
		}
		report = message[min_value_index];
		x = message_x[min_value_index];
		y = message_y[min_value_index];
		color = message_color[min_value_index];
		id = message_id[min_value_index];
		
		message_id[min_value_index] = 0;
		will_be_reported[min_value_index] = false;
	}
}



public class _UI_TEXT_PRINTER : MonoBehaviour
{
	public GameObject letter_prefab;
	
	private const int MAX_NUMBER_OF_LETTERS = 100;
	private const int LETTERSHEET_WIDTH = 736;
	private const int LETTERSHEET_HEIGHT = 880;
	
	private RawImage[] letter_rawimage = new RawImage[MAX_NUMBER_OF_LETTERS];
	private RectTransform[] letter = new RectTransform[MAX_NUMBER_OF_LETTERS];
	private bool[] letter_free = new bool[MAX_NUMBER_OF_LETTERS];
	private int[] letter_id = new int[MAX_NUMBER_OF_LETTERS];
	private int letter_check_index = 0;
	
	private float report_timer = 0.0f;
	private int current_report_id = 0;
	private bool[] report_letter = new bool[MAX_NUMBER_OF_LETTERS];
	
	private Rect[] letter_source_rectangle = new Rect[256];
	
	private float screen_width = 0.0f;
	private float screen_height = 0.0f;
	private float letter_width = 0.0f;
	private float screen_top = 0.0f;
	private float screen_left = 0.0f;
	
	private Color[] letter_colors = new Color[16];



	void Start()
	{
		letter_colors[0] = new Color(0.0f, 0.0f, 0.0f);
		letter_colors[1] = new Color(0.0f, 0.0f, 0.67f);
		letter_colors[2] = new Color(0.0f, 0.67f, 0.0f);
		letter_colors[3] = new Color(0.0f, 0.67f, 0.0f);
		letter_colors[4] = new Color(0.0f, 0.67f, 0.0f);
		letter_colors[5] = new Color(0.0f, 0.67f, 0.0f);
		letter_colors[6] = new Color(0.0f, 0.67f, 0.0f);
		letter_colors[7] = new Color(0.67f, 0.67f, 0.67f);
		letter_colors[8] = new Color(0.0f, 0.67f, 0.0f);
		letter_colors[9] = new Color(0.33f, 0.33f, 1.0f);
		
		
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
			
			hide_letter(a);
		}
	}



	private void hide_letter(int index)
	{
		letter[index].sizeDelta = new Vector2(0.0f, 0.0f);
		letter[index].anchoredPosition = new Vector2(1000.0f, 1000.0f);
		
		letter[index].localScale = new Vector3(1.0f, 1.0f, 1.0f);
		letter_rawimage[index].uvRect = letter_source_rectangle[0];
		letter_rawimage[index].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		letter_free[index] = true;
		letter_id[index] = 0;
	}



	private void get_screen_size()
	{
		screen_width = Screen.width;
		screen_height = Screen.height;
		screen_top = screen_height * 0.5f;
		screen_left = -screen_width * 0.5f;
	}



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



	void Update()
	{
		int number_of_messages = MESSAGE.number_of_updated_messages();
		if (number_of_messages > 0) Debug.Log("number="+number_of_messages);
		for (int a = 0; a < number_of_messages; a++)
		{
			string message = "";
			int x = 0;
			int y = 0;
			int color = 0;
			int id = 0;
			MESSAGE.get_updated_message(ref message, ref x, ref y, ref color, ref id);
			display_text(message, x, y, 20, letter_colors[color], id, false);
		}
		if (report_timer == 0.0f && MESSAGE.is_there_a_report())
		{
			string message = "";
			int x = 0;
			int y = 0;
			int color = 0;
			int id = 0;
			MESSAGE.get_next_report(ref message, ref x, ref y, ref color, ref id);
			Debug.Log("REPORT  ID="+id);
			display_text(message, x, y, 15, letter_colors[color], id, true);
			
			report_timer = 3.0f;
			current_report_id = id;
		}
		
		if (report_timer > 0.0f) scroll_report();
	}



	private void scroll_report()
	{
		report_timer -= _TIMER.deltatime();
		
		float SPEED = screen_height * 0.5f;
		float VISIBLE_LIMIT = 0.9f * screen_height / 2.0f;
		float HIDE_LIMIT = screen_height / 2.0f; 
		
		for(int a = 0; a < MAX_NUMBER_OF_LETTERS; a++)
		{
			if (letter_id[a] == current_report_id)
			{
				letter[a].anchoredPosition -= new Vector2(SPEED * _TIMER.deltatime(), 0.0f);
				float x = letter[a].anchoredPosition.x;
				Color color = letter_rawimage[a].color;
				color.a = 1.0f;
				if (x > HIDE_LIMIT || x < -HIDE_LIMIT) color.a = 0.0f;
				if (x > VISIBLE_LIMIT && x <= HIDE_LIMIT) color.a = 1.0f - (x - VISIBLE_LIMIT) / (HIDE_LIMIT - VISIBLE_LIMIT);
				if (x > -HIDE_LIMIT && x <= -VISIBLE_LIMIT) color.a = 1.0f - (-x - VISIBLE_LIMIT) / (HIDE_LIMIT - VISIBLE_LIMIT);
				letter_rawimage[a].color = color;
			}
		}
		
		if (report_timer < 0.0f)
		{
			delete_text_with_id(current_report_id);
			current_report_id = 0;
			report_timer = 0.0f;
		}
	}



	private void display_text(string text, int x, int y, int font_size, Color font_color, int id, bool report)
	{
				Debug.Log("UI_TEXT_PRINTER: displaying text="+text);
				string aaa = "";
				for (int a = 0; a < MAX_NUMBER_OF_LETTERS; a++)
				{
				if (letter_free[a]) aaa+="-"; else aaa+=(char)(48 + (letter_id[a]+2000000000) % 40);
				}
				Debug.Log(aaa+" aluksi");
		
		get_screen_size();
		
		float next_letter_size_y = screen_height * font_size * 0.005f;
		float next_letter_size_x = next_letter_size_y * (LETTERSHEET_WIDTH - 32.0f) / (LETTERSHEET_HEIGHT - 32.0f);
		
		float next_letter_x = screen_height * x * 0.005f + next_letter_size_x * 0.5f;
		float next_letter_y = screen_height * y * -0.005f - next_letter_size_y * 0.5f;
		
		delete_text_with_id(id);
		
		bool overwrite_old_text = false;
		int overwrite_counter = 0;
		
		for (int a = 0; a < text.Length; a++)
		{
			if (overwrite_old_text)
			{
				letter_check_index++;
				if (letter_check_index >= MAX_NUMBER_OF_LETTERS) letter_check_index = 0;
			}
			else
			{
				while (!overwrite_old_text && !letter_free[letter_check_index])
				{
					letter_check_index++;
					if (letter_check_index >= MAX_NUMBER_OF_LETTERS) letter_check_index = 0;
					overwrite_counter++;
					if (overwrite_counter >= MAX_NUMBER_OF_LETTERS - 1)
					{
						overwrite_old_text = true;
						Debug.Log("overwrite");
					}
				}
			}
			
			letter[letter_check_index].sizeDelta = new Vector2(next_letter_size_x, next_letter_size_y);
			letter[letter_check_index].anchoredPosition = new Vector2(next_letter_x, next_letter_y);
			letter_rawimage[letter_check_index].uvRect = letter_source_rectangle[text[a]];
			letter_rawimage[letter_check_index].color = font_color;
			letter_free[letter_check_index] = false;
			letter_id[letter_check_index] = id;
			report_letter[letter_check_index] = report;
			
			if (text[a] == 'M' || text[a] == 'm' || text[a] == 'W' || text[a] == 'w')
				next_letter_x += next_letter_size_x * 1.0f;
			else
				next_letter_x += next_letter_size_x * 0.9f;
			
		}
		aaa = "";
		for (int a = 0; a < MAX_NUMBER_OF_LETTERS; a++)
		{
			if (letter_free[a]) aaa+="-"; else aaa+=(char)(48 + (letter_id[a]+2000000000) % 40);
		}
		Debug.Log(aaa+" lopuksi");
		
	}



	private void delete_text_with_id(int id)
	{
		Debug.Log("DELETE:"+id);
		for (int a = 0; a < MAX_NUMBER_OF_LETTERS; a++)
			if (letter_id[a] == id) hide_letter(a);
		
		string aaa = "";
		for (int a = 0; a < MAX_NUMBER_OF_LETTERS; a++)
		{
			if (letter_free[a]) aaa+="-"; else aaa+=(char)(48 + (letter_id[a]+2000000000) % 40);
		}
		Debug.Log(aaa+" poiston jälkeen");
	}

}
