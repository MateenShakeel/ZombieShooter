using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchHit : MonoBehaviour
{

	public Vector3 normalposition;
	public float speed = 2f;
	public Transform player;
	
	public AudioSource myAudioSource;
	
	public AudioSource fireAudioSource;

	public AnimationClip fireAnimsA;
	public AnimationClip hideA;
	
	public float normalFOV  = 65f;
	public float weaponnormalFOV = 32f;
	
	private bool isA = true;
	public float fireAnimSpeed = 1.1f;
	public float inaccuracy = 0.02f;
	public float force  = 500f;
	public float damage = 50f;
	public float range = 2f;


	
	public Vector3 retractPos;
	
	private bool retract = false;	

	public Transform rayfirer;
	public Transform grenadethrower;
	private Vector3 wantedrotation;
	public float runXrotation = 20f;
	public float runYrotation = 0f;
	public Vector3 runposition = Vector3.zero;
	raycastfire weaponfirer;
	weaponselector inventory;
	public Camera weaponcamera;
	playercontroller playercontrol ;
	Animation myanimation;
	public static PunchHit Instance;
	
	void Awake()
	{
		if (Instance == null)
		{
			Instance= this;
		}
		weaponfirer = rayfirer.GetComponent<raycastfire>();
		playercontrol = player.GetComponent<playercontroller>();
		myanimation = GetComponent<Animation>();

	}
	
	void Start () 
	{
		myanimation.Stop();
		onstart();
	}

	void Update () 
	{
		
		float step = speed * Time.deltaTime;

		inventory.currentammo = 0;

		float newField = Mathf.Lerp(Camera.main.fieldOfView, normalFOV, Time.deltaTime * 2);
		float newfieldweapon = Mathf.Lerp(weaponcamera.fieldOfView, weaponnormalFOV, Time.deltaTime * 2);
		Camera.main.fieldOfView = newField;
		weaponcamera.fieldOfView = newfieldweapon;
		
		if (ControlFreak2.CF2Input.GetButton("ThrowGrenade") && !myanimation.isPlaying && inventory.grenade>0)
		{
			if(Time.timeSinceLevelLoad>(inventory.lastGrenade+1)){
				inventory.lastGrenade=Time.timeSinceLevelLoad;			
				StartCoroutine(setThrowGrenade());
			}
		}
		if (retract) 
		{
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, retractPos,step *2f);
		}



		else if (playercontrol.running)
		{

			transform.localPosition = Vector3.MoveTowards(transform.localPosition,runposition, step);
			wantedrotation = new Vector3(runXrotation,runYrotation,0f);

		}
		else
		{
			wantedrotation = Vector3.zero;
			
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, normalposition, step);
			
		}

		transform.localRotation = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(wantedrotation),step * 3f );

		// if (ControlFreak2.CF2Input.GetButton("Aim") || 	ControlFreak2.CF2Input.GetAxis("Aim") > 0.1)
		// {
		// 	doswitch();
		// }
		
		if (ControlFreak2.CF2Input.GetButton("Knife") || ControlFreak2.CF2Input.GetAxis ("Knife")>0.1 )
		{
			fire();
		}
	}
	
	
	public void onstart()
	{
		myAudioSource.Stop();
		fireAudioSource.Stop();
		retract = false;

		myanimation.Stop();

		if(weaponfirer==null) weaponfirer = rayfirer.GetComponent<raycastfire>();
		weaponfirer.inaccuracy = inaccuracy;
		weaponfirer.damage = damage;
		weaponfirer.range = range;
		weaponfirer.force = force;
		weaponfirer.projectilecount = 1;

		if(inventory==null){ 
			inventory = player.GetComponent<weaponselector>();
			//Init the Current Weapon with ammo value
			// inventory.InitCurrentWeaponAmmo(-1);
		}
		inventory.ShowAim(false);

		myAudioSource.loop = false;

		myAudioSource.Play ();
		
		isA = true;
	}
	
	void fire()
	{
		if (!myanimation.isPlaying && isA)
		{
			fireAudioSource.pitch = 0.98f + 0.1f *Random.value;
			fireAudioSource.Play();
			myanimation.clip = fireAnimsA;
			myanimation.Play();  
			StartCoroutine(firedelayed(0.3f));
		}
	}
	

	
	void doRetract()
	{
		if( isA)
		{
			myanimation.Play(hideA.name);
		}
	}
	void doNormal()
	{
		retract = false;
		onstart();
	}
	
	IEnumerator firedelayed(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		weaponfirer.fireMelee();
	}
	
	IEnumerator setThrowGrenade()
	{
		retract = true;
		grenadethrower.gameObject.SetActive(true);
		grenadethrower.gameObject.BroadcastMessage("throwstuff");
		yield return new WaitForSeconds(grenadethrower.GetComponent<Animation>()["throwAnim"].length);
		retract = false;
		grenadethrower.gameObject.SetActive(false);
	}
}
