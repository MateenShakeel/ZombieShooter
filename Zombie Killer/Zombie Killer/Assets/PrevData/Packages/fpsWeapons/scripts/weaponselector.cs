using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class weaponselector : MonoBehaviour 
{
	
	#region GlobalInstance

	public static weaponselector LocalInstance;
	public static weaponselector Instance
	{
		get
		{
			LocalInstance = GameObject.FindObjectOfType<weaponselector>();
			return LocalInstance;
		}
	}
    
	#endregion
	
	public Transform[] Weapons;
	public Transform[] AI_WeaponDrop;

	public bool[] HaveWeapons;
	public int grenade;
	public float lastGrenade;

	private int previousWeapon = 0;
	public bool canswitch;

	public bool hideweapons = false;
	bool oldhideweapons = false;
	
	public Text currentAmmo;
	public Text totalMagSize;
	
	public Text grenadetext;
	
	[HideInInspector]public int currentammo = 10;
	[HideInInspector]public int ammoClipSize = 10;

	public GameObject AIM;

	int oldAmmo=-1;
	int oldTotalAmmo =-1;
	
	private bool inventoryOn;
	
	private int currentweapon;
	private int currentrow;
	
	
	private int weaponIndex;
	playercontroller playercontrol;

	public Transform pickupGrenade;
	public Transform pickupHealth;
	
	public int[] weaponsMagSize;
	
	private void Awake()
	{

		playercontrol = GetComponent<playercontroller>();

		for(int i = 0; i < Weapons.Length; i++){
			Weapons[i].gameObject.SetActive(false);
		}
	
		grenade=10;
		//if (PlayerPrefs.GetString("Mode") == "Sniper")
		//{
		//	currentweapon = 7;
		//}
		//else if (PlayerPrefs.GetString("Mode") == "Zombie")
		//{
		//	currentweapon = 5;
		//}
		//else
		//{

		//}
        currentweapon = GameManager.Instance.weaponSelected;


		Invoke(nameof(EquipWeapon), 0.3f);
		
	}

	void EquipWeapon()
	{
        canswitch = true;
        HaveWeapons = new bool[Weapons.Length];
        HaveWeapons[currentweapon] = true;

        for (int i = 0; i < Weapons.Length; i++)
        {
            HaveWeapons[i] = true;
        }
        totalMagSize.text = PlayerPrefs.GetInt("Totalammo").ToString();
    }

	private void BulletCountDisable()
	{
		
		currentAmmo.gameObject.SetActive(false);
		totalMagSize.gameObject.SetActive(false);
		
		foreach (var autoShootOn in InGameProperties.Instance.autoShootOn)
		{
			autoShootOn.SetActive(false);
		}
		
		foreach (var autoShootOf in InGameProperties.Instance.autoShootOff)
		{
			autoShootOf.SetActive(true);
		}
		InGameProperties.Instance.autoShootOff[0].SetActive(false);
	}
	private void BulletCountEnable()
	{
		currentAmmo.gameObject.SetActive(false);
		totalMagSize.gameObject.SetActive(false);

		if (PlayerPrefs.GetInt("AutoShoot") == 1)
		{
			foreach (var autoShootOn in InGameProperties.Instance.autoShootOn)
			{
				autoShootOn.SetActive(true);
			}
			
			foreach (var autoShootOf in InGameProperties.Instance.autoShootOff)
			{
				autoShootOf.SetActive(false);
			}
		}
		else
		{
			foreach (var autoShootOn in InGameProperties.Instance.autoShootOff)
			{
				autoShootOn.SetActive(true);
			}
			
			foreach (var autoShootOn in InGameProperties.Instance.autoShootOn)
			{
				autoShootOn.SetActive(false);
			}
			
		}
	}

	private void Start()
	{
		
		inventoryOn = false;
		currentrow = 0;
		Invoke(nameof(EquipWeaponAfterDelay), 0.5f);
	}

	public void EquipWeaponAfterDelay()
	{
		StartCoroutine(selectWeapon (currentweapon));
	}

	
	private void Update()
	{
		bool changeAmmoText=false;
		if(oldAmmo!=currentammo){
			oldAmmo=currentammo;
			changeAmmoText=true;
		}

		if (changeAmmoText)
			currentAmmo.text = $"{currentammo}";
		
		grenadetext.text = grenade.ToString();
		
	}



	public void WeaponSwitch(int index)
	{
		SwitchWeapon(index);
	}
	
	private IEnumerator HidecurrentWeapon(int index)
	{

		Weapons[index].gameObject.BroadcastMessage("doRetract",SendMessageOptions.DontRequireReceiver);
		yield return new WaitForSeconds (0.15f);
		Weapons[index].gameObject.SetActive(false);
		oldhideweapons = hideweapons;

	}

	private IEnumerator UnhidecurrentWeapon(int index)
	{
		yield return new WaitForSeconds (0.15f);
		Weapons[index].gameObject.SetActive(true);
		Weapons[index].gameObject.BroadcastMessage("doNormal",SendMessageOptions.DontRequireReceiver);
		oldhideweapons = hideweapons;
	}

	IEnumerator selectWeapon(int index)
	{
		
		Weapons[previousWeapon].gameObject.BroadcastMessage("doRetract",SendMessageOptions.DontRequireReceiver);
		yield return new WaitForSeconds (0.5f);
		Weapons[previousWeapon].gameObject.SetActive(false);
		Weapons[index].gameObject.SetActive(true);
		Weapons[index].gameObject.BroadcastMessage("doNormal",SendMessageOptions.DontRequireReceiver);
		
	}

	public void ABC()
	{
        Weapons[index].gameObject.SetActive(true);
    }
	
	public void ShowAim(bool show)
	{
		AIM.SetActive(true);
		if (show)
		{
			AIM.SetActive(true);
		    InGameProperties.Instance.CrosshairEnable();
		}
		else
		{
			AIM.SetActive(false);
			InGameProperties.Instance.CrosshairDisable();
		}
	}

	#region Drop Objects

	//Select Weapon By EnemyDrop
	public void SwitchWeapon(int i)
	{
		Weapons[previousWeapon].gameObject.BroadcastMessage("doRetract",SendMessageOptions.DontRequireReceiver);
		foreach (Transform item in Weapons)
		{
			item.gameObject.SetActive(false);
		}
		Weapons[i].gameObject.SetActive(true);
		Weapons[i].gameObject.BroadcastMessage("doNormal",SendMessageOptions.DontRequireReceiver);
		totalMagSize.text = $"{weaponsMagSize[i]}";
		
		InGameProperties.Instance.ReloadEnd();

		currentweapon = i;

	}
	
	// Drop Weapon on enemyKill

	private int index;
	public void DropWeapon(Transform location)
	{
		index = Random.Range(0, AI_WeaponDrop.Length);
		Vector3 instPos = new Vector3(location.position.x,location.position.y+0.8f,location.position.z+1f);
		Instantiate(AI_WeaponDrop[index],instPos,Quaternion.identity);
	}
	
	//Drop Grenades on enemyKill

	public void DropGrenades(Transform location)
	{
		Vector3 instPos = new Vector3(location.position.x,location.position.y+0.9f,location.position.z+1f);
		Instantiate(pickupGrenade,instPos,Quaternion.identity);
	}
	
	//Drop Health on enemyKill
	
	public void DropHealth(Transform location)
	{
		Vector3 instPos = new Vector3(location.position.x,location.position.y+0.9f,location.position.z+1f);
		Instantiate(pickupHealth,instPos,Quaternion.identity);
	}
	
	#endregion

	public void CurrentClipSize(int i)
	{
		totalMagSize.text = $"{i}";
	}

}



	
