using System;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using uzAI;
using Random = UnityEngine.Random;

public class raycastfire : MonoBehaviour {

	public float force = 500f;
	public float damage = 50f;
	public float range = 100f;
	
	public LayerMask mask;
	public int projectilecount = 1;
	public float inaccuracy = 0.1f;
	public GameObject lineprefab;
	public Transform currentmuzzle;//leave this empty please
	public GameObject impactnormal;
	public GameObject impactconcrete;
	public GameObject impactwood;
	public GameObject impactblood;
	public GameObject impactwater;
	public GameObject impactmetal;
	public GameObject impactglass;
	public GameObject impactmelee;
	public GameObject impactnodecal;
	public GameObject ZombieBlood;

	RaycastHit hit;
	public void fireMelee ()
	{
		Vector3 fwrd = transform.forward;
		
		Vector3 camUp = transform.up;
		Vector3 camRight = transform.right;
		
		Vector3 wantedvector = fwrd;
		wantedvector += Random.Range( -.1f, .1f ) * camUp + Random.Range( -.1f, .1f ) * camRight;
		
		Ray ray = new Ray (transform.position, wantedvector);
		if (Physics.Raycast(ray,out hit, 3f,mask))
		{   
			
			if(hit.rigidbody) hit.rigidbody.AddForceAtPosition (500 * fwrd , hit.point);
			hit.transform.SendMessageUpwards ("Damage",50f, SendMessageOptions.DontRequireReceiver);
			GameObject decal;
			if (hit.transform.tag  == "flesh") 
			{
				//bodypart bp = hit.transform.GetComponent<bodypart>();
				//if (bp != null) bp.applyBlood( hit.point, ray.direction.normalized);

				decal =Instantiate(impactblood, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject ;
				decal.transform.localRotation = decal.transform.localRotation * Quaternion.Euler(0,Random.Range(-90,90),0);
				decal.transform.parent = hit.transform;
			}
			else
			{
				decal =Instantiate(impactmelee, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject ;
				decal.transform.localRotation = decal.transform.localRotation * Quaternion.Euler(0,Random.Range(-90,90),0);
				decal.transform.parent = hit.transform;
			}
		}
	}

	public void fire () 
	{
		for(int i = 0; i < projectilecount; i++)
		{
			firebullet();
		}
	}

	private void firebullet()
	{

		Vector3 fwrd = transform.forward;

		Vector3 camUp = transform.up;
		Vector3 camRight = transform.right;
		
		Vector3 wantedvector = fwrd;
		wantedvector += Random.Range( -inaccuracy, inaccuracy ) * camUp + Random.Range( -inaccuracy, inaccuracy ) * camRight;
		Ray ray = new Ray (transform.position, wantedvector);
		RaycastHit hit = new RaycastHit();

		if (Physics.Raycast(ray, out hit, range, mask))
		{

			if (hit.rigidbody) hit.rigidbody.AddForceAtPosition(force * fwrd, hit.point);
			// hit.transform.SendMessageUpwards ("Damage",damage, SendMessageOptions.DontRequireReceiver);

			GameObject decal;
			GameObject line;

			//if (PlayerPrefs.GetString("Mode") == "Zombie")
			//{
			//	// cameraShake.Instance.ShakeStart();

			//	if (PlayerPrefs.GetInt("Vibration") == 1)
			//		Handheld.Vibrate();
			//}

			// if (weaponselector.Instance.actionCamActive)
			// {
			// 	
			line = Instantiate(lineprefab, transform.position, transform.rotation) as GameObject;
			LineRenderer linerender = line.GetComponent<LineRenderer>();
			linerender.SetPosition(0, currentmuzzle.transform.position);
			linerender.SetPosition(1, hit.point);
			// 	
			// }

			if (hit.transform.tag == "Untagged")
			{
				decal =
					Instantiate(impactnormal, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as
						GameObject;
				decal.transform.localRotation =
					decal.transform.localRotation * Quaternion.Euler(0, Random.Range(-90, 90), 0);
				decal.transform.parent = hit.transform;
			}
			else if (hit.transform.tag == "concrete")
			{
				decal =
					Instantiate(impactconcrete, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as
						GameObject;
				decal.transform.localRotation =
					decal.transform.localRotation * Quaternion.Euler(0, Random.Range(-90, 90), 0);
				decal.transform.parent = hit.transform;
			}
			else if (hit.transform.tag == "nodecal")
			{
				decal =
					Instantiate(impactnodecal, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as
						GameObject;
				decal.transform.localRotation =
					decal.transform.localRotation * Quaternion.Euler(0, Random.Range(-90, 90), 0);
				decal.transform.parent = hit.transform;
			}
			else if (hit.transform.tag == "metal")
			{
				decal =
					Instantiate(impactmetal, hit.point,
						Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
				decal.transform.localRotation =
					decal.transform.localRotation * Quaternion.Euler(0, Random.Range(-90, 90), 0);
				decal.transform.parent = hit.transform;
			}
			else if (hit.transform.tag == "wood")
			{
				decal =
					Instantiate(impactwood, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
				decal.transform.localRotation =
					decal.transform.localRotation * Quaternion.Euler(0, Random.Range(-90, 90), 0);
				decal.transform.parent = hit.transform;
			}
			else if (hit.transform.tag == "water")
			{
				decal =
					Instantiate(impactwater, hit.point,
						Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
				decal.transform.localRotation =
					decal.transform.localRotation * Quaternion.Euler(0, Random.Range(-90, 90), 0);
				decal.transform.parent = hit.transform;
			}
			else if (hit.transform.tag == "glass")
			{
				decal =
					Instantiate(impactglass, hit.point,
						Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
				decal.transform.localRotation =
					decal.transform.localRotation * Quaternion.Euler(0, Random.Range(-90, 90), 0);
				decal.transform.parent = hit.transform;
			}
			else if (hit.transform.tag == "flesh")
			{
				decal =
					Instantiate(impactblood, hit.point,
						Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
				decal.transform.localRotation =
					decal.transform.localRotation * Quaternion.Euler(0, Random.Range(-90, 90), 0);
				decal.transform.parent = hit.transform;
			}
			else if (hit.collider.gameObject.GetComponent<TacticalAI.HitBox>()) //Tactical AI Line
			{
				if (hit.collider.name == "mixamorig:Head")
				{
					InGameProperties.Instance.isbodyShot = false;
				}
				else
				{
					InGameProperties.Instance.isbodyShot = true;
				}

				hit.transform.SendMessageUpwards("Damage", damage, SendMessageOptions.DontRequireReceiver);
				hit.collider.gameObject.GetComponent<TacticalAI.HitBox>().DamageByPlayer(damage);
			}
			else if (hit.collider.gameObject.GetComponent<explosive>()) //Tactical AI Line
			{
				hit.transform.SendMessageUpwards("Damage", damage, SendMessageOptions.DontRequireReceiver);
				hit.collider.gameObject.GetComponent<explosive>().Damage(damage);
			}
			else if (hit.collider.gameObject.GetComponent<BodyPartDamage>()) //Zombie State Manager
			{
				hit.transform.SendMessageUpwards("Damage", damage, SendMessageOptions.DontRequireReceiver);
				Instantiate(ZombieBlood, hit.point, Quaternion.Euler(-90, 0, 0));
				hit.collider.gameObject.GetComponent<BodyPartDamage>().DealDamage(damage);
				Debug.Log("Zombie Damage State");
			}
		}
	}
}
