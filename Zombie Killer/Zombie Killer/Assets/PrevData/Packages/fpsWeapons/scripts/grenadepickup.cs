using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class grenadepickup : MonoBehaviour {
	

	weaponselector inventory;
	public GameObject player;
	public int destroyTime;
	
	void Start () {
		if(player==null) player=GameObject.Find("playerController");
		inventory = player.GetComponent<weaponselector>();
		
		Destroy(this.gameObject,destroyTime);
	}

	void OnTriggerEnter (Collider other) 
	{
		if  (other.tag == "Player")
		{
			inventory.grenade +=1;
			Destroy(gameObject);
			Debug.Log("Grenades Pickup");
		}
	}
}
