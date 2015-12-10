using UnityEngine;
using System.Collections;

public class laser_script : MonoBehaviour {

    private const int MAX_BEAMS = 20; // TODO: estimate of how many beams are needed at one time
    public GameObject laserbeam; // Set prefab in editor
    private float beam_length = 2.0f;
    private float[] speed = new float[MAX_BEAMS + 1];
    private _TILEMAP tilemap;

    private Vector3[] beam_point = new Vector3[MAX_BEAMS + 1]; // front point of the beam, can be used for detecting hits
    private Vector3[] beam_direction = new Vector3[MAX_BEAMS + 1]; // beam goes this way
    private LineRenderer[] beams = new LineRenderer[MAX_BEAMS + 1];

    private Vector3 storage = new Vector3(0.0f, 10.0f, 0.0f);
    private Vector3 storage_dir = new Vector3(1.0f, 10.0f, 0.0f);

    public void shoot_beam(Vector3 gunpoint, Vector3 shooter)
    {
        // get first free beam
        int id = get_unused_beam_id();
        if (id >= 0)
        {
            speed[id] = 2.0f;
            beam_point[id] = shooter;
            beam_direction[id] = Vector3.Normalize(gunpoint - shooter); // deviation?
            Debug.Log("using beam " + id + " from point " + shooter);
        }
        else Debug.Log("Error: All lasers in use! More ammo needed!");
    }
    int get_unused_beam_id()
    {
        int id = -1;
        for (int i = 0; i < MAX_BEAMS + 1; ++i)
        {
            if (speed[i] == 0)
                return i;
        }
        return id;
    }
	// Use this for initialization
	void Start () 
    {
        tilemap = GameObject.Find("_GLOBAL_SCRIPTS").GetComponent<_TILEMAP>();
        for (int i = 0; i < MAX_BEAMS + 1; ++i)
        {
            GameObject beam = (GameObject)Instantiate(laserbeam, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            beam.transform.SetParent(transform);
            beam.transform.localPosition = transform.position; //new Vector3(0.0f, 0.0f, 0.0f);
            beams[i] = beam.GetComponent<LineRenderer>();
            beam_point[i] = storage;
            beam_direction[i] = storage_dir;
            beams[i].SetPosition(0, storage);
            beams[i].SetPosition(1, storage_dir);
            speed[i] = 0.0f;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        update_beam_positions();
        //check_beam_collisions();
        draw_beams();
	}
    
    void draw_beams()
    {
        for (int i = 0; i < MAX_BEAMS + 1; ++i)
        {
            beams[i].SetPosition(0, beam_point[i]);
            beams[i].SetPosition(1, trail_direction(beam_direction[i]));
        }
    }
    void update_beam_positions()
    {
        for (int i = 0; i < MAX_BEAMS + 1; ++i)
        {
            beam_point[i] += beam_direction[i] * _TIMER.deltatime() * speed[i];
        }
    }
    void check_beam_collisions()
    {
        for (int i = 0; i < MAX_BEAMS + 1; ++i)
        {
            if (!tilemap.floor(beam_point[i].x, -beam_point[i].z))
            {
                beam_point[i] = storage;
                beam_direction[i] = storage_dir;
                speed[i] = 0;
            }
        }
    }
// TODO: right coordinates! dammit!
    Vector3 trail_direction(Vector3 direction)
    {
        //Vector3 endpoint = new Vector3(-direction.x, 0, direction.y);
        return Vector3.Normalize(direction) * beam_length;
    }
}


