using UnityEngine;
using System.Collections;

public class instantiate_level : MonoBehaviour
{
	private _TILEMAP tilemap;


	void Start()
	{
		tilemap = GameObject.Find("_GLOBAL_SCRIPTS").GetComponent<_TILEMAP>();
		tilemap.start_game();
		Destroy(gameObject);
	}
}
