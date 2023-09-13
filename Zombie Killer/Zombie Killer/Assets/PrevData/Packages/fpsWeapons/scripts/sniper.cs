using UnityEngine;
using System.Collections;
using TacticalAI;
using UnityEngine.UI;


public class sniper : MonoBehaviour {

	public Vector3 normalposition;
	public Vector3 aimposition;	
	public Vector3 retractPos;
	
	public float aimFOV = 45f;
	public float normalFOV  = 65f;
	public float weaponnormalFOV = 32f;
	public float weaponaimFOV  = 20f;
	public float speed;


	public AudioSource myAudioSource;

	
	public AudioSource fireAudioSource;
	public AudioClip emptySound;
	public AudioClip fireSound;
	
	public AudioClip readySound;
	public AudioClip reloadSound;
	
	
	public int ammoToReload = 20;

	public int projectilecount = 1;
	public float inaccuracy = 0.02f;
	public float spreadNormal = 0.08f;
	public float spreadAim = 0.02f;
	public float force  = 500f;
	public float damage = 50f;
	public float range = 100f;
	public float smoothdamping  = 2f;
	public float recoil = 5f;
	public float maxrecoil;


	public AnimationClip fireAnim;
	public float fireAnimSpeed = 1.1f;

	public AnimationClip reloadAnim;
	public AnimationClip readyAnim;
	public AnimationClip ejectshellAnim;
	public AnimationClip hideAnim;
	
	public GameObject shell;

	public Transform shellPos;

	public float shellejectdelay = 0;
	public int ammo = 200;
	public int currentammo= 20;
	public int clipSize = 20;

	public Transform muzzle;
	public Transform clipShell;
	public Transform muzzlesmoke;

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
	public Transform rayfirer;
	public Transform player;
	public Transform meleeweapon;
	raycastfire weaponfirer;
	playercontroller playercontrol ;
	weaponselector inventory;
	camerarotate cameracontroller;
	Animation myanimation;

	public GameObject scopeOriginal;
	public GameObject scopeQuad;

	public Slider aimSlider;
	public Slider normelSlider;
	private bool isZoom;
	public GameObject zoomImage;
	public GameObject arms;

	public Transform bulletTranform;
	private float autoShootDistance;
	
	private void Awake()
	{
		weaponfirer = rayfirer.GetComponent<raycastfire>();
		playercontrol = player.GetComponent<playercontroller>();
		myanimation = GetComponent<Animation>();
		cameracontroller = recoilCamera.GetComponent<camerarotate>();
		maxrecoil = recoil;
	}
	
	private void Start()
	{
		clipSize=currentammo;
		weaponselector.Instance.CurrentClipSize(clipSize);
		nextField = normalFOV ;
		weaponnextfield = weaponnormalFOV;
		myanimation.Stop();
		Onstart();
        


        normelSlider.value  = 45; 
			normelSlider.gameObject.SetActive(false);
			aimSlider.gameObject.SetActive(false);
		
		
		//autoShootDistance = PlayerPrefs.GetString("Mode") == "Zombie" ? 7f : Mathf.Infinity;
		
	}
	
	RaycastHit hit;
	private Outline enemy;
	private bool isOutline = true;
	private float step;
	
	private void Update () 
	{
		if(ControlFreak2.CF2Input.GetButton("Reload") ){
			if (currentammo !=clipSize && ammo >0)
			{
				Reload();
			}
		}
		
		if (Physics.Raycast(muzzle.position, transform.TransformDirection(Vector3.forward), out hit, autoShootDistance))
		{
			 //if (PlayerPrefs.GetInt("AutoShoot") == 1)
			 //{
			 //	if (hit.collider.CompareTag("HitBox")&& hit.collider.gameObject.layer == 14)
			 //	{
			 //		if (currentammo == 0)
			 //		{
			 //			Reload();
			 //		}
			 //		else
			 //		{
			 //			Fire();
			 //			// if (PlayerPrefs.GetString("Mode") != "Sniper")
			 //			// {
			 //			// 	InGameProperties.Instance.CrossAssistColor();
			 //			// }
			 //		}
			 //	}
			 //	else
			 //	{
			 //		// if (PlayerPrefs.GetString("Mode") != "Sniper")
			 //		// {
			 //		//  InGameProperties.Instance.CrossAimColor();
			 //		// }
			 //	}
			
			 //}

			 if (hit.collider.CompareTag("HitBox") && hit.collider.gameObject.layer == 14)
			 {
			 	enemy = hit.transform.GetComponent<HitBox>().myScript.transform.GetComponent<HealthScript>()
			 		.EnemyMeshRender.GetComponent<Outline>();
			 	// if (PlayerPrefs.GetString("Mode") != "Sniper")
			 	// {
			 	// 	playerrotate.Instance.Pointer_Aim_Assist(); //Enemy OutLine Show 
			 	// }
			 	enemy.enabled = true;
			 }
			 else
			 {
			 	if (enemy != null)
			 	{
			 		enemy.enabled = false;
			        if (PlayerPrefs.GetString("Mode") == "Campaign")
			 		{
			 			LevelsController.Instance.EnemiesInList();
			 		}
			
			 		enemy = null;
			 	}
			
			 	// if (PlayerPrefs.GetString("Mode") != "Sniper")
			 	// {
			 	// 	playerrotate.Instance.Normel_Aim_Assist();
			 	// }
			 }
		}
		
		step = speed * Time.deltaTime;
		
		float newField = Mathf.Lerp(Camera.main.fieldOfView, nextField, Time.deltaTime * 2);
		float newfieldweapon = Mathf.Lerp(weaponcamera.fieldOfView, weaponnextfield, Time.deltaTime * 2);
		Camera.main.fieldOfView = newField;
		weaponcamera.fieldOfView = newfieldweapon;

		inventory.currentammo = currentammo;
		if (ControlFreak2.CF2Input.GetButton("ThrowGrenade") && !myanimation.isPlaying && inventory.grenade>0 && canfire)
		{
			if(Time.timeSinceLevelLoad>(inventory.lastGrenade+1)){
				inventory.lastGrenade=Time.timeSinceLevelLoad;			
				StartCoroutine(SetThrowGrenade());
			}
		}
		
		if (ControlFreak2.CF2Input.GetButton("Melee") && !myanimation.isPlaying && canfire) 
		{
			StartCoroutine(SetMelee());
		}
		
		if (retract)
		{
			canfire = false;
			canaim = false;
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, retractPos, step *2f);
			weaponnextfield = weaponnormalFOV;
			nextField = normalFOV;
		}
		else if (playercontrol.running)
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


			if (PlayerPrefs.GetString("Mode") == "Sniper")
			{
				if (aimSlider.value < 65)
				{
					inaccuracy = spreadAim;
					transform.localPosition = Vector3.MoveTowards(transform.localPosition, aimposition, step);
					zoomImage.SetActive(true);
					scopeOriginal.SetActive(false);
					arms.SetActive(false);
					weaponnextfield = weaponaimFOV;

					playerrotate.Instance.normalSens = 0.75f;
					camerarotate.Instance.normalSens = 0.75f;

					nextField = aimSlider.value;

					recoil = 0f;
					isZoom = true;
				}
				else
				{
					inaccuracy = spreadNormal;
					transform.localPosition = Vector3.MoveTowards(transform.localPosition, normalposition, step);
					weaponnextfield = weaponnormalFOV;
					nextField = normalFOV;
					scopeQuad.SetActive(false);
					zoomImage.SetActive(false);
					scopeOriginal.SetActive(true);
					arms.SetActive(true);
					recoil = maxrecoil;
					isZoom = false;
					playerrotate.Instance.normalSens = PlayerPrefs.GetFloat("TouchSens");
					camerarotate.Instance.normalSens = PlayerPrefs.GetFloat("TouchSens");
				}
			}
			else
			{
				if (((ControlFreak2.CF2Input.GetButton("Fire2") || ControlFreak2.CF2Input.GetAxis("Aim") > 0.1)) &&
				    canaim && !playercontrol.running)
				{
					// check zoom is enabled
					inaccuracy = spreadAim;
					transform.localPosition = Vector3.MoveTowards(transform.localPosition, aimposition, step);
					zoomImage.SetActive(true);
					normelSlider.gameObject.SetActive(true);
					scopeOriginal.SetActive(false);
					arms.SetActive(false);
					weaponnextfield = weaponaimFOV;

					nextField = normelSlider.value;
					recoil = 0f;
					isZoom = true;
				}
				else
				{
					// check zoom is enabled
					inaccuracy = spreadNormal;
					transform.localPosition = Vector3.MoveTowards(transform.localPosition, normalposition, step);
					weaponnextfield = weaponnormalFOV;
					nextField = normalFOV;
					
					scopeQuad.SetActive(false);
					normelSlider.gameObject.SetActive(false);
					zoomImage.SetActive(false);
					scopeOriginal.SetActive(true);
					arms.SetActive(true);
					
					recoil = maxrecoil;
					isZoom = false;
				}
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
					if (!myAudioSource.isPlaying)
					{
                        UIHandler.Instance.OpenWatchVideoPanelTime(1f);
                        myAudioSource.PlayOneShot(emptySound);
					}
				}
				else
				{
					canreload = true;
				}
			}
			else 
			{
				Reload();
			}
		}

		if(!isreloading && canfire){
			if(weaponnextfield == weaponaimFOV)
				inventory.ShowAim(false);
			else
				inventory.ShowAim(true);
			if ((ControlFreak2.CF2Input.GetButton("Fire2")  || ControlFreak2.CF2Input.GetAxis ("Fire1")>0.1)  )
			{
				Fire();
			}
		}else{
			inventory.ShowAim(true);
		}
	}
	
	private void DoRetract()
	{
		myanimation.Play(hideAnim.name);
	}
	
	private void Onstart()
	{
		myAudioSource.Stop();
		fireAudioSource.Stop();

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
		inventory.ShowAim(true);

		myanimation.Stop();
		if (isreloading) {
			Reload ();
		} 
		else 
		{
			clipShell.gameObject.SetActive (true);
			myAudioSource.clip = readySound;

				myAudioSource.loop = false;
				myAudioSource.volume = PlayerPrefs.GetInt("sfxvolume");
				myAudioSource.Play();
			

			myanimation.Play (readyAnim.name);
			
			canaim = true;
			canfire = true;
		}

        UIHandler.Instance.UpdateAmmoText();
    }

	private void Fire()
	{
		if (!myanimation.isPlaying) {
			
			StartCoroutine(Setfire());
		}

		if (currentammo <= 0)
		{
			Reload();
		}
	}
	
	private void Reload()
	{
		if (!myanimation.isPlaying && canreload && !isreloading) {
            
            StartCoroutine(Setreload ());
			StartCoroutine (DeactivateShell (myanimation[reloadAnim.name].length * 0.5f)); 
			myAudioSource.clip = reloadSound;
			myAudioSource.loop = false;

				myAudioSource.volume = PlayerPrefs.GetInt("sfxvolume");
				myAudioSource.Play();
			

			myanimation.Play(reloadAnim.name);

			InGameProperties.Instance.ReloadStart();
            UIHandler.Instance.UpdateAmmoText();
        }
	}
	

	private void DoNormal()
	{
		Onstart();
	}

	private IEnumerator Setfire()
	{
		if (currentammo > 1) 
		{
			StartCoroutine(Flashthemuzzle());
			weaponfirer.currentmuzzle = muzzle;
			weaponfirer.fire();
            
            fireAudioSource.clip = fireSound;
				fireAudioSource.pitch = 0.9f + 0.1f * Random.value;
				fireAudioSource.Play();

			float randomZ = Random.Range (-0.05f,-0.01f);
			
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y ,transform.localPosition.z + randomZ);

			cameracontroller.dorecoil(recoil);
			myanimation.Play (fireAnim.name);
			currentammo -= 1;
			
			yield return new WaitForSeconds (myanimation[fireAnim.name].length) ;

			myAudioSource.clip = readySound;
			myAudioSource.Play ();
			myanimation.Play(ejectshellAnim.name); 
			StartCoroutine (Ejectshell (shellejectdelay)); 
			yield return new WaitForSeconds ((myanimation[ejectshellAnim.name].length));
			
		} 
		else if (currentammo <= 1) {
			if (currentammo <= 0) {
				Reload ();
			}
			StartCoroutine(Flashthemuzzle());
			weaponfirer.currentmuzzle = muzzle;
			weaponfirer.fire();
			float randomZ = Random.Range (-0.05f,-0.01f);
			
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y ,transform.localPosition.z + randomZ);

			cameracontroller.dorecoil(recoil);

				fireAudioSource.clip = fireSound;
				fireAudioSource.pitch = 0.9f + 0.1f * Random.value;
				fireAudioSource.Play();
				Debug.Log("Fire3");
			

			myanimation.Play (fireAnim.name);
			
			currentammo -= 1;
            UIHandler.Instance.UpdateAmmoText();
            yield return new WaitForSeconds (myanimation[fireAnim.name].length);
		}
		
	}

	private IEnumerator Ejectshell(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		var transform1 = shellPos.transform;
		var shellInstance = Instantiate(shell, transform1.position,transform1.rotation) as GameObject;
		yield return null;
		shellInstance.GetComponent<Rigidbody>().AddRelativeForce(60,70,0);
		shellInstance.GetComponent<Rigidbody>().AddRelativeTorque(500,20,800);
		shellInstance.transform.localRotation = transform.localRotation * Quaternion.Euler(0,Random.Range(-90f,90f),0);
		
	}
	
	private IEnumerator DeactivateShell(float waitTime)
	{
		clipShell.gameObject.SetActive (false);
		yield return new WaitForSeconds(waitTime);
		clipShell.gameObject.SetActive (true);
	}

	private IEnumerator Setreload()
	{
		inventory.canswitch = false;
		isreloading = true;
		myAudioSource.clip = reloadSound;
		myAudioSource.loop = false;
		myAudioSource.volume = 1;
		myAudioSource.Play();		
		myanimation.Play(reloadAnim.name);
		playercontrol.canclimb = false;
		int oldammo = currentammo;


		canaim = false;
		yield return new WaitForSeconds (myanimation[reloadAnim.name].length *0.5f) ;
		currentammo =  0 + (Mathf.Clamp(clipSize ,clipSize- oldammo,ammo ));
		ammo -= Mathf.Clamp(clipSize, clipSize,ammo);

		yield return new WaitForSeconds (myanimation[reloadAnim.name].length *0.5f) ;
		myAudioSource.clip = readySound;
		myAudioSource.loop = false;
		myAudioSource.volume = 1;
		myAudioSource.Play();		
		myanimation.Play(ejectshellAnim.name);
		yield return new WaitForSeconds (myanimation[ejectshellAnim.name].length) ;

		playercontrol.canclimb = true;
		isreloading = false;
		canaim = true;
		inventory.canswitch = true;

		InGameProperties.Instance.ReloadEnd();
		
	}
	
	private IEnumerator Flashthemuzzle()
	{
		ParticleSystem smoke = muzzlesmoke.GetComponent<ParticleSystem>();
		smoke.Emit (1);
		muzzle.transform.localEulerAngles = new Vector3(0f,0f,Random.Range(0f,360f));
		muzzle.gameObject.SetActive(true);
		yield return new WaitForSecondsRealtime(0.05f);
		muzzle.gameObject.SetActive(false);
	}

	private IEnumerator SetThrowGrenade()
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

	private void PickAmmo(int inventoryAmmo){
		ammo=inventoryAmmo;
	}
	
	private IEnumerator SetMelee()
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

	public void SetAimFov()
	{
		aimFOV = aimSlider.value;
	}

	public void SetAimFovNormel()
	{
		aimFOV = normelSlider.value;
	}
	
}

