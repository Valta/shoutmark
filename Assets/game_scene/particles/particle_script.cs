using UnityEngine;
using System.Collections;

public class particle_script : MonoBehaviour
{
	private const int MAX_NUMBER_OF_PARTICLES = 10;
	
	public GameObject particle_prefab;
	private GameObject[] particles = new GameObject[MAX_NUMBER_OF_PARTICLES];
	private ParticleSystem[] particle_systems = new ParticleSystem[MAX_NUMBER_OF_PARTICLES];



	void Start()
	{
		for (int a = 0; a < MAX_NUMBER_OF_PARTICLES; a++)
		{
			particles[a] = (GameObject)Instantiate(particle_prefab, new Vector3(3 + a*3, 1, -8), Quaternion.identity);
			particles[a].transform.rotation = Quaternion.Euler(0, 180, 90);
			particle_systems[a] = particles[a].GetComponent<ParticleSystem>();
		}
	}

    public void particle_init()
    {
        particles = new GameObject[MAX_NUMBER_OF_PARTICLES];
	    particle_systems = new ParticleSystem[MAX_NUMBER_OF_PARTICLES];

        for (int a = 0; a < MAX_NUMBER_OF_PARTICLES; a++)
        {
            particles[a] = (GameObject)Instantiate(particle_prefab, new Vector3(3 + a * 3, 1, -8), Quaternion.identity);
            particles[a].transform.rotation = Quaternion.Euler(0, 180, 90);
            particle_systems[a] = particles[a].GetComponent<ParticleSystem>();
        }
    }


	void Update()
	{
		/*
		if (Input.GetKey(KeyCode.Alpha2))
		{
			ParticleSystem p = particles[0].GetComponent<ParticleSystem>();
			p.Play();
		}
		*/
	}



	public void start_particle(Vector3 position, float direction, int type)
	{
		int index = -1;
		for (int a = 0; a < MAX_NUMBER_OF_PARTICLES; a++)
		{
			if (!particle_systems[a].isPlaying) index = a;
		}
		
		if (index >= 0)
		{
			if (type == 0)
			{
				particles[index].transform.position = position;
				particles[index].transform.rotation = Quaternion.Euler(0, 0, 0);
				particle_systems[index].startColor = new Color(1, 0, 0);
				particle_systems[index].startLifetime = 1.0f;
				particle_systems[index].startSize = 1.0f;
				particle_systems[index].Play();
			}
			if (type == 1)
			{
				particles[index].transform.position = position;
				particles[index].transform.rotation = Quaternion.Euler(90.0f, direction + 90, 0.0f);
				particle_systems[index].startColor = new Color(1, 1, 1);
				particle_systems[index].startLifetime = 0.3f;
				particle_systems[index].startSize = 0.5f;
				particle_systems[index].Play();
			}
		}
	}



}
