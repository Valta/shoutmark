using UnityEngine;
using System.Collections;

public class TextureMover : MonoBehaviour {

    private float offSetX = 0.0f;
    private float offSetY = 0.0f;

    private Renderer renderer;

	// Use this for initialization
	void Start () {

        renderer = GetComponent<Renderer>();
	
	}
	
	// Update is called once per frame
	void Update () {

        renderer.material.mainTextureOffset = new Vector2(offSetX, offSetY);
        offSetX -= 0.1f * Time.deltaTime;
        offSetY -= 0.05f * Time.deltaTime;
	
	}
}
