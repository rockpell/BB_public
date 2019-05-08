using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OneMoreShootChance : MonoBehaviour
{
	public GameObject endParticle;

	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponent<ShootBullet>().AddShootChance(1);
			Instantiate(endParticle, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}
