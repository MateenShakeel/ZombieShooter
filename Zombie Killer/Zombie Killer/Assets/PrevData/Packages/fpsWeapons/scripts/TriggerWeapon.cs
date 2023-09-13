using UnityEngine;
using System.Collections;

public class TriggerWeapon : MonoBehaviour {
	
	public int weaponNumber;
	weaponselector inventory;
	public GameObject player;

	public float DestoryTime;
	public Vector3 WeaponRotate;
	public float RotationSpeed;
	
	void Start () {
		
		if (player == null)
		{
		   player = GameObject.Find("PlayerController");
		}
		
		inventory = player.GetComponent<weaponselector>();
		Destroy(gameObject,DestoryTime);
		
	}

	private void LateUpdate()
	{
		transform.Rotate(WeaponRotate,RotationSpeed);
	}

	// private void OnTriggerEnter (Collider other) 
	// {
	// 	if  (other.CompareTag("Player") && !InGameProperties.Instance.isReloadTime)
	// 	{
	// 		inventory.SwitchWeapon(weaponNumber);
	// 		Destroy(gameObject);
	// 	}
	// }

}
