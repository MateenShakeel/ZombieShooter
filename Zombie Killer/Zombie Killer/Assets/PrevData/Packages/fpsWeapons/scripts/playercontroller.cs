using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using TacticalAI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class playercontroller : MonoBehaviour {

	public GameData GData;
	public bool respondstoInput = true;
	public Transform mycamera;
	private Transform reference;

	public float jumpHeight = 2.0f;
	[Space(5)]
	[Header("Player Jump Interval Time")]
	public float jumpinterval = 1.5f;
	private float nextjump = 1.2f;
	public float maxhitpoints = 1000f;
	public float hitpoints = 1000f;
	public float regen = 100f;
	public Text healthtext;
	public Image HealthBar;
	public AudioClip[] hurtsounds;
	public RawImage painflashtexture;
	private float alpha;
	public Transform recoilCamera;

	public float gravity = 20.0f;
	public float rotatespeed = 4.0f;
	public float speed;
	[Space(5)]
	[Header("Player Movement Speed")]
	public float normalspeed = 0f;

	public Slider MovementSpeedBar;
	public Text MoveSpeedValue;

	public float runspeed = 8.0f;
	public float crouchspeed = 1.0f;
	public float crouchHeight = 1;
	private bool crouching = false;
	public float normalHeight = 3.0f;
	public float camerahighposition = 1.75f;
	public float cameralowposition = 0.9f;
	private float cameranewpositionY;
	private Vector3 cameranewposition;
	private float cameranextposition;
	public float dampTime = 2.0f;



	private float moveAmount;
	public float smoothSpeed = 2.0f;

	private Vector3 forward = Vector3.forward;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 right;

	private float movespeed;
	public Vector3 localvelocity;


	public bool climbladder = false;
	public Quaternion ladderRotation;
	public Vector3 ladderposition;
	public Vector3 ladderforward;
	public Vector3 climbdirection;




	public float climbspeed = 2.0f;


	public bool canclimb = true;
	private Vector3 addVector = Vector3.zero;


	public bool running = false;
	public bool canrun = true;

	public AudioSource myAudioSource;
	public AudioSource myAudioSource2;
	public AudioClip walkloop;
	public AudioClip runloop;
	public AudioClip crouchloop;
	public AudioClip climbloop;
	public AudioClip jumpclip;
	public AudioClip landclip;
	Vector3 targetDirection = Vector3.zero;
	//public Transform playermesh;
	//public Vector3 playermeshNormalPosition;
	//public Vector3 playermeshForwardPosition;
	//public Transform playerskinnedmesh;
	private bool canrun2 = true;
	public bool hideselectedweapon = false;
	Vector3 targetVelocity;
	public float falldamage;
	private float airTime;
	public float falltreshold = 2f;
	private bool prevGrounded;
	public Transform Deadplayer;
	public Transform weaponroot;

	Animator weaponanimator;
	public Transform head;
	Animator headanimator;
	public LayerMask mask;
	CharacterController controller;
	playerrotate rotatescript;
	weaponselector inventory; 
	private float nextcheck;
	public GameObject pickupHand;
	public int Ammo;
	
	private void Awake ()
	{
		
		reference = new GameObject().transform;
		weaponanimator = weaponroot.GetComponent<Animator>();
		headanimator = head.GetComponent<Animator>();
		controller = GetComponent<CharacterController>();
		rotatescript = GetComponent<playerrotate>();
		inventory = GetComponent<weaponselector>();	
		
		//if (!PlayerPrefs.HasKey("SettingChange"))
		//{
		//	PlayerPrefs.SetFloat("PlayerMove_Speed",5f);
		//}
		
		normalspeed = 5;
		//MovementSpeedBar.value = 5;
		//MoveSpeedValue.text = Mathf.RoundToInt(MovementSpeedBar.value * 10 )+"%";
		speed = 5;
		runspeed = 5;


		//IF WE WANT TO SET THE PLAYER STATIC IN ZOMBIE MODE

		//if (PlayerPrefs.GetString("Mode") == "Zombie")
		//	gameObject.isStatic = true;

	}

	//public void MovementSpeedApply()
	//{
	//	PlayerPrefs.SetFloat("PlayerMove_Speed",MovementSpeedBar.value);
	//	normalspeed = PlayerPrefs.GetFloat("PlayerMove_Speed");
	//	speed = PlayerPrefs.GetFloat("PlayerMove_Speed");
	//	runspeed = PlayerPrefs.GetFloat("PlayerMove_Speed");
	//	PlayerPrefs.SetInt("SettingChange",1);
	//	MoveSpeedValue.text = Mathf.RoundToInt(MovementSpeedBar.value * 10 )+"%";
	//}

	private void Start () 
	{
		
		painflashtexture.CrossFadeAlpha(0f,0f,true);
		cameranextposition = camerahighposition;

		HealthBar.fillAmount = maxhitpoints;

	}
	
	[HideInInspector]public bool watchvedioPanel;
	private void Update () 
	{
		
		reference.eulerAngles = new Vector3(0, mycamera.eulerAngles.y, 0);
		forward = reference.forward;
		right = new Vector3(forward.z, 0, -forward.x);
		
#if UNITY_EDITOR
		float hor = Input.GetAxisRaw("Horizontal");
		float ver = Input.GetAxisRaw("Vertical");
		
#elif !UNITY_EDITOR
		float hor = ControlFreak2.CF2Input.GetAxisRaw("Horizontal");
		float ver = ControlFreak2.CF2Input.GetAxisRaw("Vertical");
#endif
		
		
		Vector3 velocity = controller.velocity;
		localvelocity = transform.InverseTransformDirection(velocity);

		bool ismovingforward =localvelocity.z > .5f;
		
		if (climbladder && !controller.isGrounded && canclimb ) 
		{
			
			inventory.hideweapons = true;
			airTime = 0f;
			
			crouching = false;
			
			if ((localvelocity.magnitude / speed) >= 0.1f)
			{
				myAudioSource.clip = climbloop;
				if (!myAudioSource.isPlaying)
				{
					myAudioSource.loop = true;
					myAudioSource.Play();
				}

			}
			else
			{
				myAudioSource.Stop();
			}
			
			Vector3 wantedPosition = (ladderposition - transform.position);
			if( wantedPosition.magnitude > 0.05f)
			{
				addVector = wantedPosition.normalized;
				
			}
			else
			{
				addVector = Vector3.zero;
			}
			
			//meshanimator.SetBool ("climbladder", true);
			//meshanimator.SetFloat("ver", Input.GetAxis("Vertical"));
			
			rotatescript.climbing = true;
			rotatescript.ladderforward = ladderforward;
			targetDirection = (ver * climbdirection);
			targetDirection = targetDirection.normalized;
			targetDirection += addVector;

			moveDirection = targetDirection * climbspeed;
			
		} 
		else 
		{

			inventory.hideweapons = false;

			//playermesh.transform.localPosition = Vector3.MoveTowards(playermesh.transform.localPosition,playermeshNormalPosition, Time.deltaTime * 2f);
			//meshanimator.SetBool ("climbladder", false);
			//playerskinnedmesh.GetComponent<Renderer>().material.SetFloat("_cutoff", 1f);
			
			rotatescript.climbing = false;
			
			//CrossHair ZoomOut / ZoomIn
			if (hor != 0 || ver != 0)
			{
			    InGameProperties.Instance.CrossHairZoomOut();
			}
			else
			{
				InGameProperties.Instance.CrossHairZoomIn();
			}

			targetDirection = (hor * right) + (ver * forward);
			targetDirection = targetDirection.normalized;
			targetVelocity = targetDirection;
			if (controller.isGrounded) 
			{
				
				airTime = 0f;
				
				if(ControlFreak2.CF2Input.GetButtonDown("Crouch")) 
				{ 
					crouchcheck ();
				}
				
				if (!crouching)
				{   
					//meshanimator.SetBool ("crouch", false);
					canrun = true;
					controller.center = new Vector3(0f,normalHeight / 2f,0f);
					controller.height = normalHeight;
					canrun2 = true;
					cameranextposition = camerahighposition;
					canclimb = true;

				}	
				else if (crouching )
				{
					//meshanimator.SetBool ("crouch", true);
					canrun = false;
					controller.center = new Vector3(0f,crouchHeight / 2f,0f);
					controller.height = crouchHeight;
					canrun2 = false;
					cameranextposition = cameralowposition;
					canclimb = false;

				}
				// Jump
				if (ControlFreak2.CF2Input.GetButton ("Jump") && Time.time > nextjump)
				{
					nextjump = Time.time + jumpinterval;
					moveDirection.y = jumpHeight;
					myAudioSource.PlayOneShot(jumpclip);
					weaponanimator.SetBool("jump",true);
					headanimator.SetBool("jump",true);
					
					if (crouching)
					{
						crouchcheck ();
					}
				} 
				else 
				{
					weaponanimator.SetBool("jump",false);
					headanimator.SetBool("jump",false);
					
				}  

				if ((localvelocity.magnitude/speed) >= 0.1f)
				{
					
					if (speed == runspeed)
					{
						if (myAudioSource.clip == walkloop || myAudioSource.clip == crouchloop)
						{
							myAudioSource.clip = runloop;
						}
					}
					else if (speed == crouchspeed)
					{
						if (myAudioSource.clip == walkloop || myAudioSource.clip == runloop)
						
						{
							myAudioSource.clip = crouchloop;
						}
					}
					else
					{
						myAudioSource.clip = walkloop;	
					}

					if (!myAudioSource.isPlaying)
					{
						myAudioSource.loop = true;
						myAudioSource.Play();
					}
					
				}
				else
				{
					myAudioSource.Pause();
				}
			}
			else 
			{
				
				airTime += Time.deltaTime;
				moveDirection.y -= (gravity) * Time.deltaTime;
				nextjump = Time.time + jumpinterval;
				myAudioSource.Pause();
				
			}

			if (ControlFreak2.CF2Input.GetButton ("Fire2") && canrun && canrun2 || ismovingforward) 
			{
				Debug.Log("fklfnfnklfnfklnflnfklfnlfnlfnflfnlfnlfnlfnflnfl");
				speed = runspeed;
				running = false;
				
			}
			else if(crouching)
			{
				
				speed = crouchspeed;
				running = false;
				
			}
			else
			{
				
				speed = normalspeed;
				running = false;
				
			}
			if (respondstoInput)
			{
				
				targetVelocity *= speed;
				moveDirection.z = targetVelocity.z;
				moveDirection.x = targetVelocity.x;
				
			}
			else
			{
				
				moveDirection.z = 0;
				moveDirection.x = 0;
				
			}
		}

		//Level Fail System
		if (hitpoints <= 0)
		{
			if (GameManager.Instance.IsTutorialScence )
			{
				
				GameManager.Instance.LoadScene("IsTutorial");
			}
			else if (GameManager.Instance.SelectedMode==0)
			{
				UIHandler.Instance.OpenLevelFailPanelTime(4f);

			}
			else if (GameManager.Instance.SelectedMode==1)
			{
				hitpoints +=2;
				UIHandler.Instance.VedioForPlayerHealth();

			}
			//if (PlayerPrefs.GetString("Mode") != "MultiPlayer")
			//{
			//	//die
			//	GameManager.Instance.GameFail();
			//	Destroy(gameObject);
			//	Instantiate(Deadplayer, transform.position, transform.rotation);
			//	Debug.Log("PlayerDead");
			//}
			//else
			//{
			//	TeamDeathManager.Instance.PlayerDead();
			//}
		}
		
		cameranewpositionY = Mathf.Lerp(Camera.main.transform.localPosition.y,cameranextposition, Time.deltaTime * 4f);
		
		//meshanimator.SetBool ("grounded", controller.isGrounded);
						
		weaponanimator.SetBool ("grounded", controller.isGrounded);
		weaponanimator.SetFloat("speed",(localvelocity.magnitude), dampTime , .1f);
		headanimator.SetBool ("grounded", controller.isGrounded);
		headanimator.SetFloat("speed",(localvelocity.magnitude), dampTime , .1f);
		//meshanimator.SetFloat("hor",(localvelocity.x/speed) + (Input.GetAxis("Mouse X") /3f), dampTime , 0.2f);
		//meshanimator.SetFloat("ver",(localvelocity.z/ speed), dampTime , 0.8f);


		cameranewposition = new Vector3(Camera.main.transform.localPosition.x,cameranewpositionY,Camera.main.transform.localPosition.z);
		Camera.main.transform.localPosition = cameranewposition;


		controller.Move (moveDirection * Time.deltaTime);
		if (!prevGrounded && controller.isGrounded )
		{
			
			//doland
			if (airTime > falltreshold)
			{
			//	Damage(falldamage * airTime * 2f);
			}

			if (!myAudioSource.isPlaying && Time.time > nextcheck)
			{
				if (PlayerPrefs.GetInt("sfxvolume") == 1)
				{
					myAudioSource2.volume = 1;
				}
				else
				{
					myAudioSource2.volume = 0;
				}

				myAudioSource2.PlayOneShot(landclip);

			}
			nextcheck = Time.time + 0.8f;	
				
		}
		else if (prevGrounded && !controller.isGrounded)
		{
			//dojump
			
			myAudioSource2.PlayOneShot(jumpclip);

		}
		prevGrounded = controller.isGrounded;	
		
//		if (hitpoints < maxhitpoints)
//		hitpoints += regen * Time.deltaTime;
		
//		string healthstring = (Mathf.Round(hitpoints/10f)).ToString();
		healthtext.text= (hitpoints).ToString();
		HealthBar.fillAmount = (Mathf.Round(hitpoints))/maxhitpoints;
		float alpha = (hitpoints/1000f);
			
		// painflashtexture.CrossFadeAlpha(1f - alpha, .5f, false);
	}

	private IEnumerator PainFlash()
	{
		yield return new WaitForSeconds(0f);
		painflashtexture.gameObject.SetActive(true);
		yield return new WaitForSeconds(2f);
		painflashtexture.gameObject.SetActive(false);
	}

//	public static Transform dir;
	
	public void Damage (float damage) 
	{
        SoundManager.instance.PlayVocal(AudioClipsSource.Instance.zombieAttack);
        camerarotate cameracontroller = recoilCamera.GetComponent<camerarotate>();
		
		InGameProperties.Instance.IndicatorArrow(BulletScript.dir);


		StartCoroutine(PainFlash());
		
		cameracontroller.SendMessage("dorecoil", damage/3f,SendMessageOptions.DontRequireReceiver);
		if (!myAudioSource.isPlaying && hitpoints >= 0)
		{

			
				myAudioSource2.volume = 1;
			
			
			int n = Random.Range(1,hurtsounds.Length);
			myAudioSource2.clip = hurtsounds[n];
			myAudioSource2.pitch = 0.9f + 0.1f *Random.value;
			myAudioSource2.volume = 0.02f;
			myAudioSource2.Play();
			hurtsounds[n] = hurtsounds[0];
			hurtsounds[0] = myAudioSource2.clip;
		}
		//damaged = true;
		int a = Random.Range(0, 10);

		if(a > 6)
			SoundManager.instance.PlayEffect(AudioClipsSource.Instance.playerHurt);
        hitpoints = hitpoints - damage;
	}
	
	void crouchcheck()
	{
		//check ceiling!
		Ray heightray = new Ray (transform.position, Vector3.up);
		RaycastHit ceilinghit = new RaycastHit();

		if (!Physics.Raycast (heightray, out ceilinghit, 2.2f, mask)) 
		{
			crouching = !crouching;

		}
	}
	
	
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.collider.CompareTag("Health"))
		{
			InGameProperties.Instance.CollectItem();
			HealthPickup();
			Destroy(hit.gameObject);
		}

		else if (hit.collider.CompareTag("Gernade"))
		{
			InGameProperties.Instance.CollectItem();
			weaponselector.Instance.grenade += 1;
			Debug.Log("Grenade Pickup");
			Destroy(hit.gameObject);
		}


		
		
	}

	private int pickupWeapon_index;
	private GameObject pickupWeapon;
	private void OnTriggerStay(Collider other)
	{
		if  (other.CompareTag("PickUpWeapon") && !InGameProperties.Instance.isReloadTime)
		{
			// inventory.SwitchWeapon(other.GetComponent<TriggerWeapon>().weaponNumber);
			// Destroy(other.gameObject);
			pickupWeapon = other.gameObject;
			pickupWeapon_index = other.GetComponent<TriggerWeapon>().weaponNumber;
			pickupHand.SetActive(true);
			Invoke(nameof(pickUpHandOff),other.GetComponent<TriggerWeapon>().DestoryTime);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if  (other.CompareTag("PickUpWeapon"))
		{
			pickupHand.SetActive(false);
			
		}
	}

	void pickUpHandOff()
	{
		pickupHand.SetActive(false);
	}

	private void HealthPickup()
	{
	
			hitpoints = hitpoints + 25f;
			if (hitpoints  >= maxhitpoints)
			{
				hitpoints =  maxhitpoints;
			}
			Debug.Log("Current Health "+ hitpoints);
			
	}

	public void weaponPickUp()
	{
		inventory.SwitchWeapon(pickupWeapon_index);
		Destroy(pickupWeapon);
		pickupHand.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Bullet")
		{
			other.gameObject.SetActive(false);
			//bullet +100
			UIHandler.Instance.IsBulletUpdate = true;
			SoundManager.instance.PlayEffect(AudioClipsSource.Instance.Particle);

		}
	
	else if (other.gameObject.tag == "health")
		{
			other.gameObject.SetActive(false);
			HealthBar.DOFillAmount(1, 1);
			hitpoints = 100;
			healthtext.text=hitpoints.ToString();
			SoundManager.instance.PlayEffect(AudioClipsSource.Instance.Particle);

		}
		else if (other.gameObject.tag == "Speed")
		{
			other.gameObject.SetActive(false);
			runspeed = 12;
			SoundManager.instance.PlayEffect(AudioClipsSource.Instance.Particle);

			Invoke("SpeedNormal",20f);
		}
	}

	public void SpeedNormal()
	{
		runspeed = 6;

	}
}

