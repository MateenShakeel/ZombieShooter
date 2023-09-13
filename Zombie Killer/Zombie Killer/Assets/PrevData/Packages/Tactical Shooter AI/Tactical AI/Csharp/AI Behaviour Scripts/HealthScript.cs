using System;
using UnityEngine;
using System.Collections;
/*
 * Manages the agent's health. 
 * Will trigger the suppresion state on agents using cover if shields are down.
 * */
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace TacticalAI
{	

[System.Serializable]
    public class HealthScript : MonoBehaviour
{

	public GameObject enemyGun;
	public GameObject[] FloatingTextPrefab;

	public float health = 100;
	public float Totalhealth = 100;
	
	public float shields = 25;	
	
	[Header("HealthBar")]
	public GameObject HealthObject;
	public Image HealthUI;

	private float maxShields = 10;	
	public bool shieldsBlockDamage = false;	
	public float timeBeforeShieldRegen = 5;
	private float currentTimeBeforeShieldRegen;
	public float shieldRegenRate = 10;	
	    
	public TacticalAI.TargetScript myTargetScript;
	public TacticalAI.BaseScript myAIBaseScript;	
	public TacticalAI.SoundScript soundScript;
	
	[Header("Pickup Object")]
	[Space(5)]
	public bool isWeapons;
	public bool isHealth;
	public bool isGrenade;

	public bool FriendlyTeam;
	
	
	public Rigidbody[] rigidbodies;
	public Collider[] collidersToEnable;
	public TacticalAI.RotateToAimGunScript rotateToAimGunScript;
	public Animator animator;
	public TacticalAI.GunScript gunScript;
	    
	private bool beenHitYetThisFrame = false;

	public SkinnedMeshRenderer EnemyMeshRender;
	public GameObject deathParticle;


    //Initiation stuff.
	private void Awake()
		{
			soundScript = gameObject.GetComponent<TacticalAI.SoundScript>();
            if (shields < 0)
            {
                shields = 0.1f;
            }
			maxShields = shields;
			Totalhealth = health;
			HealthObject.SetActive (false);
			
		}
	
	private void Update()
	{
		currentTimeBeforeShieldRegen -= Time.deltaTime;
        timeTillNextStagger -= Time.deltaTime;

         //Only let us take explosion damage once per frame. (could also be used for weapons that would pass through an agent's body)
         //This will prevent the agent from taking the damage multiple times- once for each hitbox.
         beenHitYetThisFrame = false;
		

		if(currentTimeBeforeShieldRegen < 0  && shields < maxShields)
		{
			shields = Mathf.Clamp(shields + shieldRegenRate*Time.deltaTime, 0, maxShields);

			//When our shields are fully charged, stop being suppressed.
			if (shields == maxShields)
			{ 
				myAIBaseScript.ShouldFireFromCover(true);
			}
		}
	}
	
	public void Damage(float damage)
		{	
            //Look for the source of the damage.
			if(myTargetScript)
				myTargetScript.CheckForLOSAwareness(true);	
		
			ReduceHealthAndShields(damage);
			myAIBaseScript.CheckToSeeIfWeShouldDodge();
				
			if(health <= 0)
			{
				DeathCheck();
			}	
		}
	
	public IEnumerator SingleHitBoxDamage(float damage)
		{
            //Look for the source of the damage.
			if(myTargetScript)
				myTargetScript.CheckForLOSAwareness(true);

            //Only let us take explosion damage once per frame. (could also be used for weapons that would pass through an agent's body)
            //This will prevent the agent from taking the damage multiple times- once for each hitbox.
			if(!beenHitYetThisFrame)
			{
					ReduceHealthAndShields(damage);
									
					if(health <= 0)
					{
							DeathCheck();
					}
					beenHitYetThisFrame = true;	
			}
					
			yield return null;
			beenHitYetThisFrame = false;
		}

	public float damageCount;
	
	public void ReduceHealthAndShields(float damage)
		{
            //Shields are mandatory for the suppressioon mechanic to work.
            //However, as you may not want the agent to have any sort of regenerating health, you can choose whether or not they will actually block damage or merely work as a recent damage counter.
			if(shieldsBlockDamage)
			{
					if(damage > shields)
					{
							if(soundScript && myAIBaseScript.HaveCover() && shields > 0)
								soundScript.PlaySuppressedAudio();				
						
                            //Eliminate shields and pass on remaining damage to health.
							damage -= shields;
							shields = 0;
							health -= damage;
							
	                        //If the agent's shields go down, become suppressed (ie: agent will stay in cover as much as possible, and will avoid standing up to fire)
							myAIBaseScript.ShouldFireFromCover(false);								
					}
					else
					{
							shields -= damage;
					}
			}
			else
			{
					if(damage > shields)
					{
							if(soundScript && myAIBaseScript.HaveCover() && shields > 0)
								soundScript.PlaySuppressedAudio();

                            //If the agent's shields go down, become suppressed (ie: agent will stay in cover as much as possible, and will avoid standing up to fire)
							myAIBaseScript.ShouldFireFromCover(false);
					}	
				    
	                //Do the same amount of damage to shields AND health.
					shields = Mathf.Max(shields-damage, 0);
					health -= damage;										
			}
					
			currentTimeBeforeShieldRegen = timeBeforeShieldRegen;
			
			//Sound
			if(soundScript)
				soundScript.PlayDamagedAudio();

            if (health > 0 && damage > staggerThreshhold && canStagger && Random.value < staggerOdds && timeTillNextStagger < 0)
            {
                myAIBaseScript.StaggerAgent();
                timeTillNextStagger = timeBetweenNextStaggers;
            }
			
			HealthObject.SetActive (true);
			HealthUI.fillAmount = health / Totalhealth;

			Invoke (nameof(DisableHealthObject), 3f);
			
			// if (FloatingTextPrefab != null)
			// {
			// 	ShowFloatingText();
			// }
		}

	    /////////////////////////// Stagger State Behaviour ///////////////////??
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			myAIBaseScript.StaggerAgent();
			timeTillNextStagger = timeBetweenNextStaggers;
			// print("Stagger State");
		}
	}

	// private int Index = 0;
	// void ShowFloatingText()
	// {
	// 	if (Index==0)
	// 	{
	// 		    // Vector3 y = InGameProperties.Instance.RectPoisition();
	// 		    // var gameObject = Instantiate(FloatingTextPrefab[Random.Range(0,FloatingTextPrefab.Length)],y,Quaternion.identity,InGameProperties.Instance.floatingDamage);
	// 		    // gameObject.GetComponentInChildren<Text>().text = Random.Range(20,50).ToString();
	// 		    // Destroy(gameObject,2f);
	// 		    // Index++;
	// 		    //
	// 		    // Invoke(nameof(FloatingDamageShow),0.2f);
	// 	}
	// }
	// void FloatingDamageShow()
	// {
	// 	    Index = 0;
	// }
	    
	    //////////////////////////// Floating Damage ///////////////////
	
	public void DisableHealthObject ()
	{
		HealthObject.SetActive (false);
	}

    public float staggerThreshhold = 1.0f;
    public bool canStagger = false;
    public float staggerOdds = 0.5f;
    private float timeTillNextStagger = 0.2f;
    public float timeBetweenNextStaggers = 0.2f;
	

    //Check to see if we are dead.
        private void DeathCheck()
		{
			KillAI();
		
			if(myAIBaseScript)
				myAIBaseScript.KillAi();
			this.enabled = false;
			
			HealthObject.SetActive(false);
		}
	
	    private void KillAI()
		{
			//Check if we've done this before
			if(this.enabled)
			{
				
					int i;
					for(i = 0; i < rigidbodies.Length; i++)
					{
							rigidbodies[i].isKinematic = false;
					}
					for(i = 0; i < collidersToEnable.Length; i++)
					{				
							collidersToEnable[i].enabled = true;
					}
					for(i = 0; i < collidersToEnable.Length; i++)
					{				
						collidersToEnable[i].gameObject.tag = "Untagged";
					}
					
					deathParticle.SetActive(true);
					
					//On Death Drop Objects
					if (isWeapons)
					{
						enemyGun.SetActive(false);
						weaponselector.Instance.DropWeapon(gameObject.transform);    //Drop Weapon
					}
					else if (isHealth)
					{
						weaponselector.Instance.DropHealth(gameObject.transform);    //Drop Health
					}
					else if (isGrenade)
					{
						weaponselector.Instance.DropGrenades(gameObject.transform);  //Drop Grenades
					}

					//Enemy Death Count
					if (PlayerPrefs.GetString("Mode") != "MultiPlayer" && PlayerPrefs.GetString("Mode") == "Campaign")
					{
						
						int x = LevelsController.Instance.levelData[LevelsController.Instance.currentLevel].totalEnemies.IndexOf(transform);
						LevelsController.Instance.levelData[LevelsController.Instance.currentLevel].totalEnemies.RemoveAt(x);
						LevelsController.Instance.NextEnemy();
						
						InGameProperties.Instance.BodyShotEvent();
						
					}
					else if(PlayerPrefs.GetString("Mode") == "Sniper")
					{
						
						int x = LevelsControllerSniper.Instance.levelManager[LevelsControllerSniper.Instance.currentLevel].totalEnemies.IndexOf(transform);
						LevelsControllerSniper.Instance.levelManager[LevelsControllerSniper.Instance.currentLevel].totalEnemies.RemoveAt(x);
						LevelsControllerSniper.Instance.NextEnemy();
						
						InGameProperties.Instance.BodyShotEvent();
						
					}
					else
					{
						if (FriendlyTeam)
						{
							TeamDeathManager.Instance.PlayerTeamDead(transform);
						}
						else
						{
							TeamDeathManager.Instance.EnemyTeamDead(transform);
						}
						Debug.Log("Team Death Match");
						
					}

					//Disable scripts
					if(rotateToAimGunScript)
						rotateToAimGunScript.enabled = false;		
						
					if(animator)
						animator.enabled = false;				
											
					if(gunScript)
					{ 
						gunScript.enabled = false;
					}
					this.enabled = false;
					
			}
		}
	    
	    //Action Cam
	    public void StopAnimation()
	    {
		    
		    if(rotateToAimGunScript)
			    rotateToAimGunScript.enabled = false;		
						
		    if(animator)
			    animator.enabled = false;				
											
		    if(gunScript)
		    { 
			    gunScript.enabled = false;
		    }
		    this.enabled = true;
		    
	    }

}
}
