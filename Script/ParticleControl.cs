using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControl : MonoBehaviour {

	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (GetComponent<ParticleSystem>().isStopped && !GetComponent<AudioSource>().isPlaying)
        {
            Destroy(gameObject);
        }
	}
}
