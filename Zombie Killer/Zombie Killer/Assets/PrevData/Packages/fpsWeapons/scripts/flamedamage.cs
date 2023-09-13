using UnityEngine;
using System.Collections;
using TacticalAI;
using uzAI;

public class flamedamage : MonoBehaviour {
	
	private void OnParticleCollision (GameObject other )
	{

		if (other.layer == 14)
		{
		 other.SendMessageUpwards("Damage", 5f,SendMessageOptions.DontRequireReceiver);
		}
			
		if (other.gameObject.GetComponent<uzAIZombieStateManager>())   //Tactical AI Line
		{
			other.gameObject.GetComponent<uzAIZombieStateManager>().ZombieHealthStats.onDamage(5f);
		}
		
	}
}
