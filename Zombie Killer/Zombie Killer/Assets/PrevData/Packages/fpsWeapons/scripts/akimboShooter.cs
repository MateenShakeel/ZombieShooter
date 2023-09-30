
using UnityEngine;
using System.Collections;
using TacticalAI;


public class akimboShooter : MonoBehaviour {

	public Vector3 normalposition;
	public Vector3 aimposition;	
	public Vector3 retractPos;

	public float aimFOV = 45f;
	public float normalFOV  = 65f;
	public float weaponnormalFOV = 32f;
	public float weaponaimFOV  = 20f;
	public float speed = 1f;

	public AudioSource myAudioSource;

	public AudioSource fireAudioSource1;
	public AudioSource fireAudioSource2;
	public AudioClip emptySound;
	public AudioClip fireSound;

	public AudioClip readySound;
	public AudioClip reloadSound;

	//public int ammoToReload = 20;

	public int projectilecount = 1;
	private float inaccuracy = 0.02f;
	public float spreadNormal = 0.08f;
	public float spreadAim = 0.02f;
	public float force  = 500f;
	public float damage = 50f;
	public float range = 100f;
	public float smoothdamping  = 2f;
	public float recoil = 0f;
	public float maxrecoil = 0f;


	public AnimationClip fireAnim1;
	public AnimationClip fireAnim2;
	public float fireAnimSpeed = 1.1f;

	public AnimationClip reloadAnim;
	public AnimationClip readyAnim;

	public AnimationClip hideAnim;
	public GameObject shell;

	public Transform shellPos1;
	public Transform shellPos2;
	public float shellejectdelay = 0;
	public int ammo = 200;
	public int currentammo= 20;
	public int clipSize = 20;

	public Transform muzzle1;
	public Transform muzzlesmoke1;
	public Transform muzzle2;
	public Transform muzzlesmoke2;
	public Camera weaponcamera;
	public Transform recoilCamera;
	public float runXrotation = 20f;
	public float runYrotation = 0f;
	public Vector3 runposition = Vector3.zero;
	private float nextField;
	private float weaponnextfield;


	private Vector3 wantedrotation;
	private bool canaim = true;

	private bool canfire = true;
	private bool canreload = true;
	private bool retract = false;	
	private bool isreloading  = false;
	
	public Transform grenadethrower;
	public Transform meleeweapon;
	public Transform rayfirer;
	public Transform player;
	private bool fireleft = false;

	raycastfire weaponfirer;
	playercontroller playercontrol ;
	weaponselector inventory;
	camerarotate cameracontroller;
	Animation myanimation;
	private float autoShootDistance;
	
	private void Awake()
	{
		weaponfirer = rayfirer.GetComponent<raycastfire>();
		playercontrol = player.GetComponent<playercontroller>();
		myanimation = GetComponent<Animation>();
		cameracontroller = recoilCamera.GetComponent<camerarotate>();
	}
	
	private void Start()
	{
        
        clipSize =currentammo;
		weaponselector.Instance.CurrentClipSize(clipSize);
		PlayerPrefs.SetInt("Totalammo",clipSize);
		nextField = normalFOV ;
		weaponnextfield = weaponnormalFOV;
		myanimation.Stop();
		onstart();
		
		autoShootDistance = PlayerPrefs.GetString("Mode") == "Zombie" ? 7f : Mathf.Infinity;


	}
	
	RaycastHit hit;
	private Outline enemy;
	private bool isOutline = true;
	
	private void Update () 
	{
		if(ControlFreak2.CF2Input.GetButton("Reload") ){
			if (currentammo !=clipSize && ammo >0)
			{
				reload();
			}
		}

		if (Physics.Raycast(muzzle1.position, transform.TransformDirection(Vector3.forward), out hit, autoShootDistance))
		{
			//AutoShoot and Aim Color Changed

			//if (PlayerPrefs.GetInt("AutoShoot") == 1)
			//{
			//	if (hit.collider.CompareTag("HitBox")&& hit.collider.gameObject.layer == 14 || hit.collider.CompareTag("uzAIZombie"))
			//	{
			//		if (currentammo == 0)
			//		{
			//			reload();
			//		}
			//		else
			//		{
			//			fire();
			//			// CrosshairColor.Instance.CrossAssistColor();
			//			InGameProperties.Instance.CrossAssistColor();
			//		}
			//	}
			//	else
			//	{
			//		// CrosshairColor.Instance.CrossAimColor();
			//		InGameProperties.Instance.CrossAimColor();
			//	}

			//}

			if (hit.collider.CompareTag("HitBox") && hit.collider.gameObject.layer == 14)
			{
				enemy = hit.transform.GetComponent<HitBox>().myScript.transform.GetComponent<HealthScript>()
					.EnemyMeshRender.GetComponent<Outline>();
				playerrotate.Instance.Pointer_Aim_Assist(); //Enemy OutLine Show 
				enemy.enabled = true;
			}
			else
			{
				if (enemy != null)
				{
					enemy.enabled = false;
					if (PlayerPrefs.GetString("Mode") != "MultiPlayer")
					{
						LevelsController.Instance.EnemiesInList();
					}

					enemy = null;
				}
				playerrotate.Instance.Normel_Aim_Assist();
			}
		}

		float step = speed * Time.deltaTime;
		weaponfirer.inaccuracy = inaccuracy;
		float newField = Mathf.Lerp(Camera.main.fieldOfView, nextField, Time.deltaTime * 2);
		float newfieldweapon = Mathf.Lerp(weaponcamera.fieldOfView, weaponnextfield, Time.deltaTime * 2);
		Camera.main.fieldOfView = newField;
		weaponcamera.fieldOfView = newfieldweapon;

		inventory.currentammo = currentammo;
		//inventory.totalammo = ammo;
		if (ControlFreak2.CF2Input.GetButton("ThrowGrenade") && !myanimation.isPlaying && inventory.grenade>0 && canfire)
		{
			if(Time.timeSinceLevelLoad>(inventory.lastGrenade+1)){
				inventory.lastGrenade=Time.timeSinceLevelLoad;			
				StartCoroutine(setThrowGrenade());
			}
		}
		if (ControlFreak2.CF2Input.GetButton("Melee") && !myanimation.isPlaying && canfire) 
		{
			StartCoroutine(setMelee());
		}
		if (retract)
		{
			canfire = false;
			canaim = false;
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, retractPos, step);
			weaponnextfield = weaponnormalFOV;
			nextField = normalFOV;
		}


		else if (playercontrol.running && !retract)
		{
			canfire = false;
			transform.localPosition = Vector3.MoveTowards(transform.localPosition,runposition, step);
			wantedrotation = new Vector3(runXrotation,runYrotation,0f);
			weaponnextfield = weaponnormalFOV;
			nextField = normalFOV;

		}
		else
		{
			canfire = true;
			wantedrotation = Vector3.zero;
			if (((ControlFreak2.CF2Input.GetButton("Fire2") || ControlFreak2.CF2Input.GetAxis("Aim") > 0.1)) && canaim && !playercontrol.running)
			{						// check zoom is enabled
				inaccuracy = spreadAim;
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, aimposition, step);
				weaponnextfield = weaponaimFOV;
				nextField = aimFOV;
			}
			else
			{								// check zoom is enabled
				inaccuracy = spreadNormal;
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, normalposition, step);
				weaponnextfield = weaponnormalFOV;
				nextField = normalFOV;
			}

		}

		transform.localRotation = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(wantedrotation),step * 3f);

		if (currentammo == 0 || currentammo  <= 0 )
		{	
			if (ammo <= 0)
			{
				canfire = false;
				canreload = false;
				if ((ControlFreak2.CF2Input.GetButton("Fire2") || ControlFreak2.CF2Input.GetAxis ("Fire1")>0.1) && !myAudioSource.isPlaying)
				{
                    //UIHandler.Instance.OpenWatchVideoPanelTime(1f);
                    myAudioSource.PlayOneShot(emptySound);
				}
				else
				{
					canreload = true;
				}
			}
			else 
			{
				reload();
			}
		}


		if(!isreloading && canfire){
			inventory.ShowAim(true);
			if ((ControlFreak2.CF2Input.GetButton("Fire2")  || ControlFreak2.CF2Input.GetAxis ("Fire1")>0.1)  )
			{
				fire();
			}
		}else{
			inventory.ShowAim(true);
		}
	}

	void doRetract()
	{
		myanimation.Play(hideAnim.name);
		weaponnextfield = weaponnormalFOV;
		nextField = normalFOV; 
	}

	void onstart()
	{
		myAudioSource.Stop();
		fireAudioSource1.Stop();

		fireAudioSource2.Stop();
		if(weaponfirer==null) weaponfirer = rayfirer.GetComponent<raycastfire>();
		weaponfirer.inaccuracy = inaccuracy;
		weaponfirer.damage = damage;
		weaponfirer.range = range;
		weaponfirer.force = force;
		weaponfirer.projectilecount = projectilecount;

		if(inventory==null){ 
			inventory = player.GetComponent<weaponselector>();
			//Init the Current Weapon with ammo value
		}
		inventory.ShowAim(false);

		myanimation.Stop();
		if (isreloading) {
			reload ();
		} 
		else 
		{
			myAudioSource.clip = readySound;
			myAudioSource.loop = false;
			myAudioSource.volume = 1;
			myAudioSource.Play ();

			myanimation.Play (readyAnim.name);
			canaim = true;
			canfire = true;
		}

		
		
			UIHandler.Instance.UpdateAmmoText();

		
		

    }

	void fire()
	{

		if (!myanimation.isPlaying)
		{
			if (!fireleft)
			{
				float randomZ = Random.Range (-0.05f,-0.01f);
			
					
				transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y ,transform.localPosition.z + randomZ);

				cameracontroller.dorecoil(recoil);

				StartCoroutine(flashthemuzzle1());
				weaponfirer.currentmuzzle = muzzle1;
				weaponfirer.fire();

				fireAudioSource1.clip = fireSound;
				fireAudioSource1.pitch = 0.9f + 0.1f *Random.value;
				fireAudioSource1.Play();
				myanimation[fireAnim1.name].speed = fireAnimSpeed;     
				myanimation.Play(fireAnim1.name);
				currentammo -=1;
				
				StartCoroutine(ejectshell1(shellejectdelay));
				fireleft = true;
			}
			else
			{
				float randomZ = Random.Range (-0.05f,-0.01f);

				transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y ,transform.localPosition.z + randomZ);

				cameracontroller.dorecoil(recoil);

				StartCoroutine(flashthemuzzle2());
				weaponfirer.currentmuzzle = muzzle2;
				weaponfirer.fire();

				fireAudioSource2.clip = fireSound;
				fireAudioSource2.pitch = 0.9f + 0.1f *Random.value;
				fireAudioSource2.Play();
				myanimation[fireAnim2.name].speed = fireAnimSpeed;     
				myanimation.Play(fireAnim2.name);
				currentammo -=1;
				StartCoroutine(ejectshell2(shellejectdelay));
				fireleft = false;
			}


            if (currentammo <= 0)
			{
				reload();
			}

          
	            UIHandler.Instance.UpdateAmmoText();
   
            
		}


	}

	void reload()
	{
		if (!myanimation.isPlaying && canreload && !isreloading) {

			StartCoroutine(setreload (myanimation[reloadAnim.name].length));
			myAudioSource.clip = reloadSound;
			myAudioSource.loop = false;
			myAudioSource.volume = 1;
			myAudioSource.Play();		

			myanimation.Play(reloadAnim.name);
            InGameProperties.Instance.ReloadStart();
            
	            UIHandler.Instance.UpdateAmmoText();

            
		} 
	}



	void doNormal()
	{
		onstart();
	}

	IEnumerator ejectshell1(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		GameObject shellInstance;
		shellInstance = Instantiate (shell, shellPos1.transform.position, shellPos1.transform.rotation) as GameObject;
		yield return null;
		shellInstance.GetComponent<Rigidbody>().AddRelativeForce(60,70,0);
		shellInstance.GetComponent<Rigidbody>().AddRelativeTorque(500,20,800);
		shellInstance.transform.localRotation = transform.localRotation * Quaternion.Euler(0,Random.Range(-90f,90f),0);

	}
	IEnumerator ejectshell2(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		GameObject shellInstance;
		shellInstance = Instantiate (shell, shellPos2.transform.position, shellPos2.transform.rotation) as GameObject;
		yield return null;
		shellInstance.GetComponent<Rigidbody>().AddRelativeForce(60,70,0);
		shellInstance.GetComponent<Rigidbody>().AddRelativeTorque(500,20,800);
		shellInstance.transform.localRotation = transform.localRotation * Quaternion.Euler(0,Random.Range(-90f,90f),0);

	}



	IEnumerator setreload(float waitTime)
	{
		playercontrol.canclimb = false;
		inventory.canswitch = false;
		int oldammo = currentammo;
		isreloading = true;

		canaim = false;
		yield return new WaitForSeconds (waitTime * .5f);
		currentammo =  0 + (Mathf.Clamp(clipSize ,clipSize- oldammo,ammo ));
		ammo -= Mathf.Clamp(clipSize, clipSize,ammo);

		yield return new WaitForSeconds (waitTime * .5f);
		isreloading = false;
		canaim = true;
		inventory.canswitch = true;
		playercontrol.canclimb = true;

		InGameProperties.Instance.ReloadEnd();
	}

	IEnumerator flashthemuzzle1()
	{
		ParticleSystem smoke1 = muzzlesmoke1.GetComponent<ParticleSystem>();
		smoke1.Emit (1);
		muzzle1.transform.localEulerAngles = new Vector3(0f,0f,Random.Range(0f,360f));
		muzzle1.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.05f);
		muzzle1.gameObject.SetActive(false);
	}

	IEnumerator flashthemuzzle2()
	{
		ParticleSystem smoke2 = muzzlesmoke2.GetComponent<ParticleSystem>();
		smoke2.Emit (1);
		muzzle2.transform.localEulerAngles = new Vector3(0f,0f,Random.Range(0f,360f));
		muzzle2.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.05f);
		muzzle2.gameObject.SetActive(false);
	}

	IEnumerator setThrowGrenade()
	{
		canfire = false;
		retract = true;
		grenadethrower.gameObject.SetActive(true);
		grenadethrower.gameObject.BroadcastMessage("throwstuff");
		Animation throwerAnimation = grenadethrower.GetComponent<Animation> ();

		yield return new WaitForSeconds(throwerAnimation.clip.length);
		retract = false;
		canaim = true;
		canfire = true;
		grenadethrower.gameObject.SetActive(false);
	}

	void pickAmmo(int inventoryAmmo){
		ammo=inventoryAmmo;
	}
	IEnumerator setMelee()
	{
		if (!meleeweapon.gameObject.activeInHierarchy)
		{

			retract = true;
			canfire = false;
			meleeweapon.gameObject.SetActive (true);
			meleeweapon.gameObject.BroadcastMessage ("melee");
			Animation meleeAnimation = meleeweapon.GetComponent<Animation> ();
			yield return new WaitForSeconds(meleeAnimation.clip.length);
			retract = false;
			canaim = true;
			canfire = true;
			meleeweapon.gameObject.SetActive (false);
		}
	}

}
