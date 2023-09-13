using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MenuManager : MonoBehaviour
{

    #region Global Refrence

    public static MenuManager localInstance;
    public static MenuManager Instance
    {
        get
        {
            if (localInstance == null)
                localInstance = GameObject.FindObjectOfType<MenuManager>();
            return localInstance;
        }
    }

    #endregion

    #region Properties

    [Space(5)]
    [Header("--- UI Panels")]
    [Space(3)] public GameObject[] reference;
    [Space(2)] public GameObject[] customPanelRef;
    [Space(2)] private int currentActivePanel;

    public GameObject[] campaignMapLevels;
    public GameObject[] selectedMap;

    public Text[] coinsText;

    [Space(6)]
    public Text loadingProgress;
    private AsyncOperation async;
    private int selectedLevel;

    [Space(2)] public GameObject[] modeClicked;

    [Space(5)] [Header("--- Nuke-Area")] [Space(3)]
    public GameObject[] levelsNuke;

    [Space(5)] [Header("--- Octane-Ghost")] [Space(3)]
    public GameObject[] levelsOctane;

    [Space(5)] [Header("--- Levels Sniper")] [Space(3)]
    public GameObject[] levelsSniper;
    private int indexSniperLevel;
    [Space(5)] [Header("--- Levels ZOMBIE")] [Space(3)]
    public GameObject[] levelsZombie;
    private int indexZombieLevel;

    public GameObject[] tdmModeSelected;
    public Weapon_Manager weaponManager;
    public GameObject bananaCharacter;


    [Space(5)] [Header("--- Buy Buttons")] [Space(3)]
    public GameObject[] buyLevelbuttons;
    public GameObject unlockAllGamebuton;
    public GameObject removeAdsButton;

    #endregion

    #region Initilization

    private void Start()
    {
        if (PlayerPrefs.GetString("removeAd", "no").Equals("no"))
        {
            //Advertisements.Instance.ShowBanner(BannerPosition.TOP, BannerType.Banner);
        }
           
        Time.timeScale = 1;
        ControlFreak2.CFCursor.lockState = CursorLockMode.None;

        EnablePanel(0);
        CashShown();

        if (!PlayerPrefs.HasKey("Session"))
            PlayerPrefs.SetString("Session", "Done");
        else
        {
            EnableDailyReward();
        }

        ////////////////////////////////////// Levels Locking

        NukeArea_UnlockLevels();
        Octane_UnlockLevels();
        UnlockSniperLevels();
        UnlockZombieLevels();
        InitTdm();
        InitSelectedMode();
        InitCampaignMapStatus();

        ////////////////////////////////////// Ads Calling


    }

    private void EnableDailyReward()
    {
        if (PlayerPrefs.HasKey("Session"))
        {
            if (DailyReward.Instance.isRewardReady || !PlayerPrefs.HasKey("Claimed" + 0))
            {
                DailyReward.Instance.ClaimBtn.SetActive(true);
                DailyReward.Instance.RewardedClaimBtn.SetActive(false);
                EnablePanel(11);
                // AdsController.Instance.HideBanner();
                // Advertisements.Instance.HideBanner();
            }
        }
    }

    public void CashShown()
    {
        foreach (var item in coinsText)
        {
            item.text = PlayerPrefs.GetInt("Cash").ToString();
        }
    }

    #endregion

    #region Generic

    public void CustomEnableReference(int x)
    {
        foreach (var item in customPanelRef)
        {
            item.SetActive(false);
        }
        customPanelRef[x].SetActive(true);

        foreach (var item in reference)
        {
            item.SetActive(false);
        }
    }

    public void BackCustom()
    {
        foreach (var item in customPanelRef)
        {
            item.SetActive(false);
        }
        EnablePanel(currentPanel);
    }
    public void back_Banner()
    {
        if (PlayerPrefs.GetString("removeAd", "no").Equals("no"))
        {
            // Advertisements.Instance.ShowBanner(BannerPosition.TOP, BannerType.Banner);
        }
    }
    public void Exit()
    {
        
        EnablePanel(8);
        //  AdsController.Instance.UnityVideo();
        // if (PlayerPrefs.GetString("removeAd", "no").Equals("no"))
        //     Advertisements.Instance.ShowInterstitial();
        
    }
    public void showbottombanner()
    {

        if (PlayerPrefs.GetString("removeAd", "no").Equals("no"))
        {
            // Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM, BannerType.SmartBanner);
        }
       
    }
    private int currentPanel;
    public void EnablePanel(int x)
    {
       
        foreach (var item in reference)
        {
            item.SetActive(false);
        }
        reference[x].SetActive(true);
        currentPanel = x;
        
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void UnlockAllGame()
    {
        Weapon_Manager.Instance.PurchaseAllWeapons();
        BuyNukeArea_Levels();
        Buy_OctaneGhostLevels();
        BuyAllSniperLevels();
        BuyAllZombieLevels();
        PlayerPrefs.SetString("AllGameUnlocked","Done");
        unlockAllGamebuton.SetActive(false);
    }
   

    
    #endregion
    
    #region Loading
    
    private IEnumerator LoadAsynchronously(int sceneIndex)
    {
        
        yield return new WaitForSecondsRealtime(1.5f);
        async = SceneManager.LoadSceneAsync(sceneIndex);
        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / .9f);
            loadingProgress.text = (progress * 100f).ToString("0") + "%";
            yield return null;
        }
        
    }
    
    private IEnumerator LoadingScene(int sceneIndex)
    {
        EnablePanel(10);
        CurrentModeAnalytics();
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(sceneIndex);
    }

    #endregion
    
    #region Nuke-Area Levels

    public void NukeLevelIndex(int z)
    {
        PlayerPrefs.SetInt("Level", z);
        StartCoroutine(LoadingScene(3));
    }
    
    private void NukeArea_UnlockLevels()
    {
        for (int t = 0; t < levelsNuke.Length; t++)
        {
            if (t < PlayerPrefs.GetInt("Unlock"))
            {
               
                levelsNuke[t].GetComponent<Button>().interactable = true;
                levelsNuke[t].transform.GetChild(1).gameObject.SetActive(false);
                levelsNuke[t].transform.GetChild(4).gameObject.SetActive(true);
                levelsNuke[t].transform.GetChild(3).gameObject.SetActive(false);
            }
            else
            {
                levelsNuke[t].GetComponent<Button>().interactable = false;
                levelsNuke[t].transform.GetChild(1).gameObject.SetActive(true); 
                levelsNuke[t].transform.GetChild(2).gameObject.SetActive(false);
            } 
        }
        levelsNuke[PlayerPrefs.GetInt("SelectedLevel")].transform.GetChild(0).gameObject.SetActive(true);
        levelsNuke[PlayerPrefs.GetInt("SelectedLevel")].transform.GetChild(1).gameObject.SetActive(false);
        levelsNuke[PlayerPrefs.GetInt("SelectedLevel")].transform.GetChild(2).gameObject.SetActive(true);
        levelsNuke[PlayerPrefs.GetInt("SelectedLevel")].GetComponent<Button>().interactable = true;
    }

    public void SetLevel()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("SelectedLevel")+1);
        StartCoroutine(LoadingScene(3));
    }
        
    public void BuyNukeArea_Levels()
    {
        for (int y = 0; y < 20; y++)
        {
            levelsNuke[y].GetComponent<Button>().interactable = true;
            levelsNuke[y].transform.GetChild(1).gameObject.SetActive(false);
            levelsNuke[y].transform.GetChild(2).gameObject.SetActive(true);
            PlayerPrefs.SetInt("Unlock", 20);
        }
        PlayerPrefs.SetString("LevelPurchased", "Done");
    }
    
    #endregion

    #region Octain-Ghost Levels
    
    public void OctaneLevelIndex(int x)
    {
        PlayerPrefs.SetInt("Level2", x);
        StartCoroutine(LoadingScene(2));
    }
    
    private void Octane_UnlockLevels() 
    {
        
        for (int q = 0; q < levelsOctane.Length; q++)
        {
            if (q < PlayerPrefs.GetInt("Unlock2")) 
            {
                levelsOctane[q].GetComponent<Button>().interactable = true;
                levelsOctane[q].transform.GetChild(1).gameObject.SetActive(false);
                levelsOctane[q].transform.GetChild(4).gameObject.SetActive(true);
                levelsOctane[q].transform.GetChild(3).gameObject.SetActive(false);
            }
            else
            {
                levelsOctane[q].GetComponent<Button>().interactable = false;
                levelsOctane[q].transform.GetChild(1).gameObject.SetActive(true);
                levelsOctane[q].transform.GetChild(2).gameObject.SetActive(false);
            } 
        }
        levelsOctane[PlayerPrefs.GetInt("SelectedLevel2")].transform.GetChild(0).gameObject.SetActive(true);
        levelsOctane[PlayerPrefs.GetInt("SelectedLevel2")].transform.GetChild(2).gameObject.SetActive(true);
        levelsOctane[PlayerPrefs.GetInt("SelectedLevel2")].transform.GetChild(1).gameObject.SetActive(false);
        levelsOctane[PlayerPrefs.GetInt("SelectedLevel2")].GetComponent<Button>().interactable = true;
            
    }
    
    public void SetLevel2()
    {
        PlayerPrefs.SetInt("Level2", PlayerPrefs.GetInt("SelectedLevel2")+1);
        StartCoroutine(LoadingScene(2));
    }
        
    public void Buy_OctaneGhostLevels()
    {
        for (int q = 0; q < 25; q++)
        {
            levelsOctane[q].GetComponent<Button>().interactable = true;
            levelsOctane[q].transform.GetChild(1).gameObject.SetActive(false);
            levelsOctane[q].transform.GetChild(2).gameObject.SetActive(true);
            PlayerPrefs.SetInt("Unlock2", 25);
        }
        PlayerPrefs.SetString("LevelPurchased2", "Done");
    }
    
    #endregion
    
    #region LevelStatus Sniper Mission

    public void LevelsIndexSniper(int w)
    {
        PlayerPrefs.SetInt("LevelSniper", w);
        StartCoroutine(LoadingScene(2));
    }
    
    private void UnlockSniperLevels() 
    {
        
        for (int w = 0; w < levelsSniper.Length; w++)
        {
            if (w < PlayerPrefs.GetInt("UnlockSniperLevel")) 
            {
                levelsSniper[w].GetComponent<Button>().interactable = true;
                levelsSniper[w].transform.GetChild(1).gameObject.SetActive(false);
                levelsSniper[w].transform.GetChild(4).gameObject.SetActive(true);
                levelsSniper[w].transform.GetChild(3).gameObject.SetActive(false);
            }
            else
            {
                levelsSniper[w].GetComponent<Button>().interactable = false;
                levelsSniper[w].transform.GetChild(1).gameObject.SetActive(true);
                levelsSniper[w].transform.GetChild(2).gameObject.SetActive(false);
            } 
        }
        
        levelsSniper[PlayerPrefs.GetInt("SelectedLevelSniper")].transform.GetChild(0).gameObject.SetActive(true);
        levelsSniper[PlayerPrefs.GetInt("SelectedLevelSniper")].transform.GetChild(2).gameObject.SetActive(true);
        levelsSniper[PlayerPrefs.GetInt("SelectedLevelSniper")].transform.GetChild(1).gameObject.SetActive(false);
        levelsSniper[PlayerPrefs.GetInt("SelectedLevelSniper")].GetComponent<Button>().interactable = true;
            
        }

    public void SetSniperLevel()
    {
        PlayerPrefs.SetInt("LevelSniper", PlayerPrefs.GetInt("SelectedLevelSniper")+1);
        StartCoroutine(LoadingScene(2));
    }
        
    public void BuyAllSniperLevels()
    {
        for (int w = 0; w < 10; w++)
        {
            levelsSniper[w].GetComponent<Button>().interactable = true;
            levelsSniper[w].transform.GetChild(1).gameObject.SetActive(false);
            levelsSniper[w].transform.GetChild(2).gameObject.SetActive(true);
            PlayerPrefs.SetInt("UnlockSniperLevel", 10);
        }
        PlayerPrefs.SetString("UnlockAllSniperLevels", "Done");
    }
    
    #endregion
    
    #region LevelStatus Zombie Mode

    public void LevelsIndexZombie(int s)
    {
        PlayerPrefs.SetInt("LevelZombie", s);
        StartCoroutine(LoadingScene(3));
    }
    
    public void UnlockZombieLevels() 
    {
        
        for (int s = 0; s < levelsZombie.Length; s++)
        {
            if (s < PlayerPrefs.GetInt("UnlockZombieLevel")) 
            {
                levelsZombie[s].GetComponent<Button>().interactable = true;
                levelsZombie[s].transform.GetChild(1).gameObject.SetActive(false);
                levelsZombie[s].transform.GetChild(4).gameObject.SetActive(true);
                levelsZombie[s].transform.GetChild(3).gameObject.SetActive(false);
            }
            else
            {
                levelsZombie[s].GetComponent<Button>().interactable = false;
                levelsZombie[s].transform.GetChild(1).gameObject.SetActive(true);
                levelsZombie[s].transform.GetChild(2).gameObject.SetActive(false);
            } 
        }
        levelsZombie[PlayerPrefs.GetInt("SelectedLevelZombie")].transform.GetChild(0).gameObject.SetActive(true);
        levelsZombie[PlayerPrefs.GetInt("SelectedLevelZombie")].transform.GetChild(2).gameObject.SetActive(true);
        levelsZombie[PlayerPrefs.GetInt("SelectedLevelZombie")].transform.GetChild(1).gameObject.SetActive(false);
        levelsZombie[PlayerPrefs.GetInt("SelectedLevelZombie")].GetComponent<Button>().interactable = true;
            
    }

    public void SetZombieLevel()
    {
            PlayerPrefs.SetInt("LevelZombie", PlayerPrefs.GetInt("SelectedLevelZombie")+1);
            StartCoroutine(LoadingScene(3));
    }
        
    public void BuyAllZombieLevels()
    {
            for (int x = 0; x < 15; x++)
            {
                levelsZombie[x].GetComponent<Button>().interactable = true;
                levelsZombie[x].transform.GetChild(1).gameObject.SetActive(false);
                levelsZombie[x].transform.GetChild(2).gameObject.SetActive(true);
                PlayerPrefs.SetInt("UnlockZombieLevel", 15);
            }
            PlayerPrefs.SetString("UnlockAllZombieLevels", "Done");
    }
    
    #endregion

    #region Team Death Match (TDM)
    
    public void SelectTdmEnv()
    {
        PlayerPrefs.SetString("MultiPlayerMap",EventSystem.current.currentSelectedGameObject.name);
        InitTdm();
    }
    
    private void InitTdm()
    {
        if (!PlayerPrefs.HasKey("MultiPlayerMap"))
        {
            PlayerPrefs.SetString("MultiPlayerMap","Map1");
            InitTdm();
        }
        
        if (PlayerPrefs.GetString("MultiPlayerMap") == "Map1")
        {
            tdmModeSelected[0].SetActive(true);
            tdmModeSelected[2].SetActive(true);
            tdmModeSelected[1].SetActive(false);
            tdmModeSelected[3].SetActive(false);
        }
        else
        {
            tdmModeSelected[1].SetActive(true);
            tdmModeSelected[3].SetActive(true);
            tdmModeSelected[0].SetActive(false);
            tdmModeSelected[2].SetActive(false);
        }
    }
    
    public void GoToMatchMaking()
    {
        weaponManager.CustomSelect(PlayerPrefs.GetInt("Weapon"));
        EnablePanel(6);
        Invoke(PlayerPrefs.GetString("MultiPlayerMap") == "Map1" ? nameof(SceneLoadShipment) : nameof(SceneLoadIcebox),
            13f);

    }
    
    private void SceneLoadShipment()
    {
        StartCoroutine(LoadAsynchronously(3));
    }
    private void SceneLoadIcebox()
    {
        StartCoroutine(LoadAsynchronously(2));
    }
    
    
    
    #endregion
    
    #region Current Map In Campaign

    public void SelectCampaignMap(int x)
    {
        PlayerPrefs.SetString("Map",EventSystem.current.currentSelectedGameObject.name);
        MapSelectionIndex(x);
        
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
    }

    private void MapSelectionIndex(int x)
    {
        foreach (var item in campaignMapLevels)
        {
            item.SetActive(false);
        }
        campaignMapLevels[x].SetActive(true);
        
        foreach (var item in selectedMap)
        {
            item.SetActive(false);
        }
        selectedMap[x].SetActive(true);
    }
    
    
    private void InitCampaignMapStatus()
    {
        if (!PlayerPrefs.HasKey("Map"))
        {
            PlayerPrefs.SetString("Map", "OctaneGhost");
            MapSelectionIndex(0);
        }
        else
        {
            if (PlayerPrefs.GetString("Map") == "OctaneGhost")
            {
                MapSelectionIndex(0);
            }
            else if (PlayerPrefs.GetString("Map") == "NukeArea")
            {
                MapSelectionIndex(1);
            }
        }
    }
    
    #endregion
    
    #region Mode Selection

    public void SelectMode()
    {
        PlayerPrefs.SetString("Mode",EventSystem.current.currentSelectedGameObject.name);
        CurrentMode();
    }
    
    public void CheckSelectedMode(int clicked)
    {
        foreach (GameObject item in modeClicked)
        {
            item.SetActive(false);
        }
        modeClicked[clicked].SetActive(true);
    }

    public void CurrentMode()
    {
        if (PlayerPrefs.GetString("Mode") == "Campaign")
        {
            CheckSelectedMode(0);
            EnablePanel(2);
        }
        else if (PlayerPrefs.GetString("Mode") == "Zombie")
        {
            EnablePanel(3);
            CheckSelectedMode(2);
        }else if (PlayerPrefs.GetString("Mode") == "Sniper")
        {
            EnablePanel(4);
            CheckSelectedMode(1);
        }else if (PlayerPrefs.GetString("Mode") == "MultiPlayer")
        {
            EnablePanel(5);
            CheckSelectedMode(3);
        }
        InterstitialAd();
    }

    private void CurrentModeAnalytics()
    {
        if (PlayerPrefs.GetString("Mode") == "Campaign")
        {
            if (PlayerPrefs.GetString("Map") == "NukeArea")
            {
                Analyticsmanager.instance.ModeNameEvent("NukeArea");
            }
            else if (PlayerPrefs.GetString("Map") == "OctaneGhost")
            {
                Analyticsmanager.instance.ModeNameEvent("OctaneGhost");
            }
        }
        else 
        if (PlayerPrefs.GetString("Mode") == "Zombie")
        {
            Analyticsmanager.instance.ModeNameEvent("Zombie_Mode");
        }
        else if (PlayerPrefs.GetString("Mode") == "Sniper")
        {
            Analyticsmanager.instance.ModeNameEvent("Sniper_Mode");
        }
        else if (PlayerPrefs.GetString("Mode") == "MultiPlayer")
        {
            Analyticsmanager.instance.ModeNameEvent("MultiPlayer");
        }
    }    
    
    private void InitSelectedMode()
    {
        if (!PlayerPrefs.HasKey("Mode"))
        {
            PlayerPrefs.SetString("Mode","Campaign");
            CheckSelectedMode(0);
        }
        
        if (PlayerPrefs.GetString("Mode") == "Campaign")
        {
            CheckSelectedMode(0);
        }
        if (PlayerPrefs.GetString("Mode") == "Zombie")
        {
            CheckSelectedMode(2);
        }else if (PlayerPrefs.GetString("Mode") == "Sniper")
        {
            CheckSelectedMode(1);
        }else if (PlayerPrefs.GetString("Mode") == "MultiPlayer")
        {
            CheckSelectedMode(3);
        }
    }

    #endregion

    private void LateUpdate()
    {

        if (reference[7].activeInHierarchy && !reference[9].activeInHierarchy)
        {
            weaponManager.OpenParent();
        }
        else
        {
            weaponManager.CloseParent();
        }
        
    }
    
    public void InterstitialAd()
    {

    }
    
}



