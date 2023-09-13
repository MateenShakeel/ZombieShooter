using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardedMenu : MonoBehaviour
{
    
    public int[] Rewards;
    public Image[] bg;
    public Image[] bgButtons;
    public GameObject noInternet;
    [HideInInspector]public bool rewardedMenu;
    [HideInInspector]public bool freeReward;
    [HideInInspector]public bool removeAdsrewarded;

    [Header("Unlock All Weapons")] 
    [Space(5)]
    public GameObject[] checkMark;
    [HideInInspector]public bool weaponrewarded;
    public Button buyAllWeapon;
    
    [Space(5)]
    [Header("Shipment Level Unlock")] 
    [Space(5)]
    public GameObject[] shipmentCheckMark;
    [HideInInspector]public bool shipmentrewarded;
    public Button shipmentLevels;
    public Text shipmentCount;
    
    
    
    [Space(5)]
    [Header("Remove Ads")] 
    [Space(5)]
    public Text removeAdsCheck;
    public Button removeAds;
    
    [Space(5)]
    [Header("UnlockFullGame")] 
    [Space(5)]
    public Text allGameCheck;
    [HideInInspector]public bool allGameRewarded;
    public Button allGame;  
    
    
    [Space(5)]
    [Header("Sniper Level Unlock")] 
    [Space(5)]
    public GameObject[] sniperCheckMark;
    [HideInInspector]public bool sniperrewarded;
    public Button sniperLevels;
    public Text sniperCount;
    
    [Space(5)]
    [Header("Zombie Level Unlock")] 
    [Space(5)]
    public GameObject[] zombieCheckMark;
    [HideInInspector]public bool zombierewarded;
    public Button zombieLevels;
    public Text zombieCount;
    
    public static RewardedMenu instance;
    
    [Space(5)]
    [Header("Icebox Level Unlock")] 
    [Space(5)]
    public Text iceboxCount;
    [HideInInspector]public bool iceboxrewarded;
    public Button iceboxLevels;
    
    [Space(5)]
    [Header("Stylized Level Unlock")] 
    [Space(5)]
    public Text stylizedCount;
    [HideInInspector]public bool stylizedrewarded;
    public Button stylizedLevels;
    
    private void Start()
    {
        instance = this;
        // WeaponRewardedStatus();
        // Rewardedvideostatus();
        // ShipmentRewardedStatus();
        // SniperRewardedStatus();
        // ZombieRewardedStatus();
        // UnlockAllGameStatus();
        // RemoveAdsStatus();
        // InitRewardedCheck();
    }
    
    public IEnumerator NoAccess()
    {
        noInternet.SetActive(true);
        yield return new WaitForSeconds(3f);
        noInternet.SetActive(false);
    }


    // private void InitRewardedCheck()
    // {
    //     
    //     sniperLevels.interactable = !PlayerPrefs.HasKey("UnloclAllSniperLevels");
    //     zombieLevels.interactable = !PlayerPrefs.HasKey("UnlockAllZombieLevels");
    //     shipmentLevels.interactable = !PlayerPrefs.HasKey("LevelPurchased");
    //     
    // }
    
    
    #region Rewarded Video Panel

    public void Rewardedvideostatus()
    {
        for (int i = 0; i < bg.Length; i++)
        {
            if (PlayerPrefs.GetInt("rewardedvideomenu") < i)
            {
                bg[i].GetComponent<Button>().interactable = false;
                bgButtons[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                bg[i].GetComponent<Button>().interactable = true;
                bgButtons[i].GetComponent<Button>().interactable = true;
            }
        }
        for (int i = 0; i <= bg.Length; i++)
        {
            if (PlayerPrefs.GetInt("rewardedvideomenu")>i)
            {
                bg[i].GetComponent<Button>().interactable = false;
                bgButtons[i].GetComponent<Button>().interactable = false;
            }
        }
    }
    
    public void Rewardedvideo()
    {
        
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
          //  AdsController.Instance.ShowRewarded();
            rewardedMenu = true;
        }
        else
        {
            StartCoroutine(NoAccess());
        }
    }
    
    #endregion

    #region Free Cash

    public void GetFreeCash()
    { 
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            freeReward = true;
          //  AdsController.Instance.ShowRewarded();
            
        }
        else
        {
            StartCoroutine(RewardedMenu.instance.NoAccess());
        }
    }
    
    public void FreeCashReward()
    {
        PlayerPrefs.SetInt("Cash",PlayerPrefs.GetInt("Cash")+200);
        MenuManager.Instance.CashShown();
    }

    #endregion
    
    #region Weapon Rewarded
    
    public void WeaponRewardedStatus()
    {
        for (int i = 0; i <= checkMark.Length; i++)
        {
            if (PlayerPrefs.GetInt("weaponrewarded")>i)
            {
                checkMark[i].SetActive(true);
            }
        }
    }

    [HideInInspector]public bool CatReward;
    [HideInInspector]public bool TankReward;
    public void WeaponRewardedVideo(string name)
    {
        
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (string.Equals(name, "Cat"))
                CatReward = true;
            else
                TankReward = true;
            
            // weaponrewarded = true;
          //  AdsController.Instance.ShowRewarded();
        }
        else
        {
            StartCoroutine(NoAccess());
        }
    }
    

    #endregion

    #region Shipmment Rewarded
    
    public void ShipmentRewardedStatus()
    {
        for (int i = 0; i <= 10; i++)
        {
            if (PlayerPrefs.GetInt("ShipmentRewarded")>i)
            {
                shipmentCount.text = $"{(i + 1)} / 10";
                Analyticsmanager.instance.CustomEvent("ShipmentLevelsRewarded");
            }
        }
    }
    public void ShipmentRewardedVideo()
    {
        
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
           // AdsController.Instance.ShowRewarded();
            shipmentrewarded = true;
        }
        else
        {
            StartCoroutine(NoAccess());
        }
    }

    #endregion

    #region Sniper Rewarded
    
    public void SniperRewardedStatus()
    {
        for (int i = 0; i <= 10; i++)
        {
            if (PlayerPrefs.GetInt("SniperRewarded")>i)
            {
                sniperCount.text = $"{(i + 1)} / 10";
                Analyticsmanager.instance.CustomEvent("SniperLevelsRewarded");
            }
        }
    }
    public void SniperRewardedVideo()
    {
        
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
           // AdsController.Instance.ShowRewarded();
            sniperrewarded = true;
        }
        else
        {
            StartCoroutine(NoAccess());
        }
    }

    #endregion
    
    #region ZombieRewardedStatus

    public void ZombieRewardedStatus()
    {
            for (int i = 0; i <= 10; i++)
            {
                if (PlayerPrefs.GetInt("ZombieRewarded")>i)
                {
                    zombieCount.text = $"{(i + 1)} / 10";
                    Analyticsmanager.instance.CustomEvent("ZombieLevelsRewarded");
                }
            }
    }
    public void ZombieRewardedVideo()
    {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                zombierewarded = true;
               // AdsController.Instance.ShowRewarded();
            }
            else
            {
                StartCoroutine(NoAccess());
            }
    }

    #endregion
    
    #region RemoveAds Rewarded
    
    public void RemoveAdsStatus()
    {
        for (int i = 0; i <= 25; i++)
        {
            if (PlayerPrefs.GetInt("RemoveAdsRewarded")>i)
            {
                removeAdsCheck.text = (i+1).ToString();
                Analyticsmanager.instance.CustomEvent("RemoveAdsRewarded");
            }
        }
    }
    
    public void RemoveAdsVideo()
    {
        
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            removeAdsrewarded = true;
           // AdsController.Instance.ShowRewarded();
        }
        else
        {
            StartCoroutine(NoAccess());
        }
    }
    
    #endregion
    
    #region Unlocl All Game Rewarded

    public void UnlockAllGameStatus()
    {
        for (int i = 0; i <= 40; i++)
        {
            if (PlayerPrefs.GetInt("UnlockAllGameRewarded")>i)
            {
                allGameCheck.text = $"{(i + 1)} / 40";
                Analyticsmanager.instance.CustomEvent("UnlockAllGameRewarded");
            }
        }
    }
    public void UnlockAllGameVideo()
    {
        
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            allGameRewarded = true;
            //  AdsController.Instance.ShowRewarded();
        }
        else
        {
            StartCoroutine(NoAccess());
        }
    }
    
    #endregion 
    
}
