using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Weapon_Manager : MonoBehaviour
{
	#region GlobalRefrence
	public static Weapon_Manager LocalInstance;
	public static Weapon_Manager Instance
	{
		get
		{
			if (LocalInstance == null)
				LocalInstance = GameObject.FindObjectOfType<Weapon_Manager>();
			return LocalInstance;
		}
	}
	#endregion
	
	public GameObject[] weapons;
	public GameObject equiped;
	public GameObject buy;

	[Header ("Weapons Stats")]
	[Space(5)]
	public Slider power;
	public Slider damage;
	public Slider grip;
	public Slider range;
	[Space(5)]
	public int[] powerStat;
	public int[] damageStat;
	public int[] gripStat;
	public int[] rangeStat;
	[Space(5)]
	public Text[] specsValue;
	public Text weaponsNameText;
	[Space(5)]
	public string[] weaponNames;


	public int[] weaponPrices;
	public Text weaponPrice;
	public GameObject weaponPriceParent;

	public GameObject notEnoughCash;
	public GameObject weaponPanel;
	public GameObject parentObject;

	private int currentWeapon = 0;
	private Vector3 originalScale;

	public GameObject[] selectedBorder;
	public GameObject unlockWeapons;

	public GameObject[] locks;

	private void Awake()
	{
		
		PlayerPrefs.SetInt ("WeaponIsUnlock" + 0, 5);

		currentWeapon = 0;
		
		notEnoughCash.SetActive(false);
		UpdateStatus();
		LockStatus();
	}
	
	private void SelectedWeaponsBorder(int i)
	{
		foreach (GameObject item in selectedBorder)
		{
			item.SetActive(false);
		}
		selectedBorder[i].SetActive(true);
	}
	
	
	public void CustomSelect(int index)
	{
		currentWeapon = index;
		UpdateStatus();
		Analyticsmanager.instance.GunsTrackingMenu(index);
	}
	
	//---------------------------------------------------------------------------------------------------------------//
	public void UpdateStatus()
	{
		
		if(PlayerPrefs.HasKey("WeaponIsUnlock" + currentWeapon))
		{
			equiped.gameObject.SetActive (true);
			buy.gameObject.SetActive (false);
			weaponPriceParent.SetActive(false);
			locks[currentWeapon].SetActive(false);
			
		}
		else
		{
			
			buy.gameObject.SetActive (true);
			weaponPriceParent.SetActive(true);
			weaponPrice.text = weaponPrices[currentWeapon].ToString();
			
			equiped.gameObject.SetActive (false);
			locks[currentWeapon].SetActive(true);
		}
		
		power.value = damageStat [currentWeapon];
		damage.value = damageStat [currentWeapon];
		grip.value = gripStat [currentWeapon];
		range.value = rangeStat [currentWeapon];

		////Change Weapon Name
        weaponsNameText.text = weaponNames[currentWeapon];
		
		specsValue[2].text = powerStat [currentWeapon].ToString();
		specsValue[0].text = damageStat [currentWeapon].ToString();
		specsValue[1].text = gripStat [currentWeapon].ToString();
		specsValue[2].text = rangeStat [currentWeapon].ToString();
		
		foreach (var item in weapons) {
			item.SetActive (false);
		}
		
		SelectedWeaponsBorder(currentWeapon);
		weapons [currentWeapon].SetActive (true);

	}
	
	private void LockStatus()
	{
		for (int i = 0; i < locks.Length; i++)
		{
			if (PlayerPrefs.HasKey("WeaponIsUnlock"+i))
			{
				locks[i].SetActive(false);
			}
			else
			{
				locks[i].SetActive(true);
			}
		}
	}
	
	public void BuyWeapon()
	{
		if(PlayerPrefs.GetInt("Cash") >= weaponPrices[currentWeapon])
		{
			PlayerPrefs.SetInt ("Cash", PlayerPrefs.GetInt ("Cash") - weaponPrices[currentWeapon]);
			PlayerPrefs.SetInt ("WeaponIsUnlock" + currentWeapon, 10);
			UpdateStatus();
			MenuManager.Instance.CashShown();
		}
		else
		{
			StartCoroutine(NotEnough());
		}
	}

	private IEnumerator NotEnough()
	{
		yield return new WaitForSeconds(0);
		notEnoughCash.SetActive (true);
		yield return new WaitForSeconds(2f);
		notEnoughCash.SetActive (false);
	}

	public bool getFreeWeapon;
	public bool tryFreeWeapon;
	public void BuyForAds()
	{
		if (Application.internetReachability != NetworkReachability.NotReachable)
		{    
			//AdsController.Instance.ShowRewarded();
			getFreeWeapon = true;
			tryFreeWeapon = true;
		}
		else
		{
			//No Internet
		}
	}
	
	
	public void Equip()
	{
		
		PlayerPrefs.SetInt ("Weapon", currentWeapon);
		CloseParent();
		MenuManager.Instance.EnablePanel(1);
		Setting.Instance.ClickAudio(2);
		

		
	}

	public void CloseParent()
	{
		parentObject.SetActive(false);
	}
	public void OpenParent()
	{
		parentObject.SetActive(true);
	}

	private void LocksDisable()
	{
		for (int i = 0; i < locks.Length; i++) {
			locks[i].SetActive(false);
		}
	}
	
	public void PurchaseAllWeapons()
	{
		for (int i = 0; i < 15; i++) {
			PlayerPrefs.SetInt ("WeaponIsUnlock" + i, 10);
		    PlayerPrefs.SetString("AllWeaponsPurchased","Done");
		}
		unlockWeapons.SetActive(false);
		UpdateStatus();
		LockStatus();
	}
	
	public void Not_EnoughCashPanel()
	{
		notEnoughCash.SetActive(false);
		weaponPanel.SetActive(true);
		parentObject.SetActive(true);
	}

	// private bool indicatorCheck;
	// public void ScrollRectValue()
	// {
	// 	if (indicatorCheck)
	// 	{
	// 		if (scrollbarVirtical.value < 0.7 && indicatorCheck) 
	// 		{
	// 		   indicatorHand.SetActive(false);
	// 		   indicatorCheck = false; 
	// 		}
	// 		else 
	// 		{ 
	// 			indicatorHand.SetActive(true); 
	// 		}
	// 	}
	// }

}

