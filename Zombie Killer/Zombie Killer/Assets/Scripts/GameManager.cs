using UnityEngine.SceneManagement;
using UnityEngine;
// using MonetizationSallman;
//using GameAnalyticsIngenious;

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    public static GameManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(gameObject);
    }
    #endregion

    public GameData GData;

    [Header("Variables")]
    public int SelectedMode;
    public int levelSelected;
    public int weaponSelected;
    
   
    [Header("Game Link")]
    public string GameLink;

    public bool RewardedForBullets;
    public bool RewardedForHealth;
    public bool RewardedForDog;
    public bool RewardedFor2xDamage;
    
   

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    #region IN APPS

    public void BuyCashSucces(int amount)
    {
        GData.cash += amount;
        //MainMenuHandler.instance.BuyCashSuccess();
        //PersistentDataManager.instance.SaveGameData();
    }

    public void BuyGoldSuccess(int amount)
    {
        GData.gold += amount;
        //MainMenuHandler.instance.BuyGoldSuccess();
        //PersistentDataManager.instance.SaveGameData();
    }

    public void BuyUnlockEverythingSuccess()
    {
        for (int i = 0; i < GData.Weapons.Length; i++)
        {
            GData.Weapons[i].isUnlocked = true;
        }

        for (int i = 0; i < GData.Skins.Length; i++)
        {
            GData.Skins[i].isUnlocked = true;
        }

        GData.levelsUnlocked = 9;
        GData.isUnlockEverything = true;
        GData.RemoveAds = true;
        GData.isUnlockGuns = true;
        
        //MainMenuHandler.instance.UnlockEverythingSuccess();
        //PersistentDataManager.instance.SaveGameData();
        HideBanners();
    }

    public void BuyUnlockGunsSuccess()
    {
        for (int i = 0; i < GData.Weapons.Length; i++)
        {
            GData.Weapons[i].isUnlocked = true;
        }

        for (int i = 0; i < GData.Skins.Length; i++)
        {
            GData.Skins[i].isUnlocked = true;
        }

        GData.isUnlockGuns = true;

        //MainMenuHandler.instance.UnlockGunsSuccess();
        //PersistentDataManager.instance.SaveGameData();
    }

    public void RemoveAdsSuccess()
    {
        GData.RemoveAds = true;
        //MainMenuHandler.instance.RemoveAdsSuccess();
        //PersistentDataManager.instance.SaveGameData();
        
        HideBanners();
    }
    #endregion

    public void FirstLoadingStart()
    {
        // MonetizationManager.instance.ShowBanner(MonetizationManager.instance.MData.FirstSceneB);
        // MonetizationManager.instance.HideBigBanner();
    }
    public void TutorialStart()
    {
        // MonetizationManager.instance.HideBanner();
        // MonetizationManager.instance.HideBigBanner();
    }
    public void MainMenuStart()
    {
        // MonetizationManager.instance.HideBanner();
        // MonetizationManager.instance.HideBigBanner();
    }
    public void InfoPanelAds()
    {
        // MonetizationManager.instance.ShowBanner(MonetizationManager.instance.MData.InfoB);
        // MonetizationManager.instance.ShowBigBanner(MonetizationManager.instance.MData.InfoBB);
    }
    public void SettingPanelAds()
    {
        // MonetizationManager.instance.ShowBanner(MonetizationManager.instance.MData.SettingB);
        // MonetizationManager.instance.ShowBigBanner(MonetizationManager.instance.MData.SettingBB);
    }
    public void PrivacyPanelAds()
    {
        // MonetizationManager.instance.ShowBanner(MonetizationManager.instance.MData.OtherSceneB);
        // MonetizationManager.instance.ShowBigBanner(MonetizationManager.instance.MData.OtherSceneBB);
    }
    public void ShopPanelAds()
    {
        // MonetizationManager.instance.ShowBanner(MonetizationManager.instance.MData.StoreB);
        // MonetizationManager.instance.HideBigBanner();
    }
    public void ExitPanelAds()
    {
        // MonetizationManager.instance.HideBanner();
        // MonetizationManager.instance.ShowBigBanner(MonetizationManager.instance.MData.ExitBB);
    }
    public void LevelSelectionStart()
    {
        // MonetizationManager.instance.HideBigBanner();
        // MonetizationManager.instance.ShowBanner(MonetizationManager.instance.MData.LevelSelectionB);
    }
    public void WeaponSelectionStart()
    {
        // MonetizationManager.instance.HideBigBanner();
        // MonetizationManager.instance.ShowBanner(MonetizationManager.instance.MData.CharacterSelectionB);
    }
    public void LoadingStart()
    {
        // MonetizationManager.instance.ShowBanner(MonetizationManager.instance.MData.LoadingBB);
        // MonetizationManager.instance.HideBigBanner();
    }
    public void GunSelectionStart()
    {
        // MonetizationManager.instance.HideBigBanner();
        // MonetizationManager.instance.ShowBanner(MonetizationManager.instance.MData.OtherSceneB);
    }
    public void ModeSelectionStart()
    {
        // MonetizationManager.instance.HideBigBanner();
        // MonetizationManager.instance.ShowBanner(MonetizationManager.instance.MData.OtherSceneB);
    }
    public void GamePlayStart()
    {
        // MonetizationManager.instance.HideBigBanner();
        // MonetizationManager.instance.ShowBanner(MonetizationManager.instance.MData.GamePlayB);
    }
    public void PausePanelAds()
    {
        // MonetizationManager.instance.HideBanner();
        // MonetizationManager.instance.ShowBigBanner(MonetizationManager.instance.MData.PauseBB);
    }
    public void LevelCompletePanelAds()
    {
        // MonetizationManager.instance.HideBanner();
        // MonetizationManager.instance.ShowBigBanner(MonetizationManager.instance.MData.CompleteBB);
    }
    public void LevelFailPanelAds()
    {
        // MonetizationManager.instance.HideBanner();
        // MonetizationManager.instance.ShowBigBanner(MonetizationManager.instance.MData.FailBB);
    }
    public void WatchVideoPanelAds()
    {
        // MonetizationManager.instance.HideBanner();
        // MonetizationManager.instance.ShowBigBanner(MonetizationManager.instance.MData.WatchVideoBB);
    }
    public void HideBanners()
    {
        // MonetizationManager.instance.HideBigBanner();
        // MonetizationManager.instance.HideBanner();
    }
    public void PlayBtnAd()
    {
        // MonetizationManager.instance.ShowInterstitial(MonetizationManager.instance.MData.PlayBtnInt);
    }
    public void LevelBtnAd()
    {
        // MonetizationManager.instance.ShowInterstitial(MonetizationManager.instance.MData.LevelBtnInt);
    }
    public void PauseBtnAd()
    {
        // MonetizationManager.instance.ShowInterstitial(MonetizationManager.instance.MData.PauseBtnInt);
    }
    public void LevelCompleteAd()
    {
        // MonetizationManager.instance.ShowInterstitial(MonetizationManager.instance.MData.CompleteInt);
    }
    public void LevelFailAd()
    {
        // MonetizationManager.instance.ShowInterstitial(MonetizationManager.instance.MData.FailInt);
    }
    public void HomeBtnAd()
    {
        // MonetizationManager.instance.ShowInterstitial(MonetizationManager.instance.MData.HomeBtnInt);
    }
    public void ExitBtnAd()
    {
        // MonetizationManager.instance.ShowInterstitial(MonetizationManager.instance.MData.ExitBtnInt);
    }
    public void RewardedAd()
    {
        // MonetizationManager.instance.ShowRewarded();
    }

    //public void OnWatchVideoReward()
    //{
    //    Scene scene = SceneManager.GetActiveScene();
    //    if (scene.name == "MainMenu")
    //    {
    //        MainMenuHandler.instance.OnRewardSuccess();
    //    }
    //    else
    //    {
    //        UIHandler.Instance.OnWatchVideoSuccess();
    //    }
    //}
    
    //public void FirstLoadingGA()
    //{
    //    GameAnalyticsManager.instance.GA_FirstLoading();
    //}
    //public void MainMenuGA()
    //{
    //    GameAnalyticsManager.instance.GA_MainMenu();
    //}
    //public void ModeSelectionGA()
    //{
    //    GameAnalyticsManager.instance.GA_ModeSelection();
    //}
    //public void LevelSelectionGA()
    //{
    //    GameAnalyticsManager.instance.GA_LevelSelection();
    //}
    //public void GamePlayGA()
    //{
    //    GameAnalyticsManager.instance.GA_GamePlayStart();
    //}
    //public void LevelCompleteGA()
    //{
    //    GameAnalyticsManager.instance.GA_LevelComplete();
    //}
    //public void LevelFailGA()
    //{
    //   GameAnalyticsManager.instance.GA_LevelFail();
    //}
}
