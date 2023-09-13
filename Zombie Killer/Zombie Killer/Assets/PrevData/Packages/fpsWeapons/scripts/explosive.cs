using UnityEngine;
using System.Collections;

public class explosive : MonoBehaviour {
	
	public float hitpoints = 100f;
	public GameObject explosion;
	// public Transform spawnObject;
	public float power = 100.0f;
	
	void Update () 
	{
		if (hitpoints <= 0)
		{
			// Instantiate(spawnObject, transform.position, transform.rotation);
			Instantiate(explosion, transform.position, Quaternion.identity);
			Vector3 explosionPos = transform.position;
			Destroy(this.gameObject);
		}
	}
	
	public void Damage (float damage) 
	{
		hitpoints = hitpoints - damage;
	}
}
