using UnityEngine;
using System.Collections;

public class particle_script : MonoBehaviour
{
	private const int MAX_NUMBER_OF_PARTICLES = 2;
	
	public GameObject particle_prefab;
	private GameObject[] particles = new GameObject[MAX_NUMBER_OF_PARTICLES];



	void Start()
	{
		for (int a = 0; a < MAX_NUMBER_OF_PARTICLES; a++)
		{
			particles[a] = (GameObject)Instantiate(particle_prefab, new Vector3(3 + a*3, 1, -8), Quaternion.identity);
			particles[a].transform.rotation = Quaternion.Euler(90, 180, 0);
		}
	}



	void Update()
	{
		if (Input.GetKey(KeyCode.Alpha2))
		{
			ParticleSystem p = particles[0].GetComponent<ParticleSystem>();
			p.Play();
		}
	}


	public void start_particle(Vector3 position, float direction, int type)
	{
		
	}



}
