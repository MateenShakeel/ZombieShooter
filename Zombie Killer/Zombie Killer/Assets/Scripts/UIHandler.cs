using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
public class UIHandler : MonoBehaviour
{
    public GameData GData;
    #region SINGLETON
    public static UIHandler Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    [Header("Texts")]
    public Text ammoText;
    public Text killsText;
    public Text levelCompleteKills;
    public Text levelCompleteCashReward;
    public Text levelCompleteTotalReward;
    public Text objectiveText;

    [Header("Panels")]
    public GameObject levelCompletePanel;
    public GameObject YouWonPanel;
    public GameObject levelFailPanel;
    public GameObject pauseGamePanel;
    public GameObject watchVideoPanel;

    public GameObject objectivePanel;

    [Header("Buttons")]
    public GameObject nextLevelButton;

    [Header("Random Variables")]
    public bool canUpdateText;
   public int totalAmmo;
   public int currentAmmo;
    public bool IsBulletUpdate;
    public GameObject Pistol, SmgRed, SmgSkin,SmgRedBtn, SmgSkinBtn;
    public GameObject Dogbtn;
    public GameObject BossTxt;
    public bool IsPistol;
    public Image DogShadow, DoubleDamage;
    public GameObject Damage2x;
    public GameObject WatchVedioPlayerHealth;
    private void Update()
    {
        if(canUpdateText)
        { 
            UpdateAmmoText();
            UpdateKillsText();
        }
        if (IsBulletUpdate)
        {
            IsBulletUpdate = false;
            BulletUpdate();
            Debug.Log("Sddddjhkhfkdjhfkhjdkjhfdkhdkhjk");
        } 
    }


    public void OpenObjectivePanel()
    {
        if (GameManager.Instance.SelectedMode == 0)
        {
            if(!objectivePanel.activeInHierarchy)
            {
                objectiveText.text = GameplayHandler.Instance.levels[GameManager.Instance.levelSelected].objectiveText;
                objectivePanel.SetActive(true);
            }
            else
            {
                objectivePanel.SetActive(false);
            }    
        }
        
        
       
    }

    public void VedioForPlayerHealth()
    {
        WatchVedioPlayerHealth.SetActive(true);
    }

    
    public void UpdateAmmoText()
    {
        if(weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<genericShooter>())
        {
             totalAmmo = weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<genericShooter>().ammo;
             currentAmmo = weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<genericShooter>().currentammo;
        }
        if(weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<akimboShooter>())
        {
             totalAmmo = weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<akimboShooter>().ammo;
             currentAmmo = weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<akimboShooter>().currentammo;
        }
        if (weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<sniper>())
        {
             totalAmmo = weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<sniper>().ammo;
             currentAmmo = weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<sniper>().currentammo;
        }
             ammoText.text = currentAmmo.ToString() + " / " + totalAmmo.ToString();
         
    }

    void UpdateKillsText()
    {
            int currentKills = GameplayHandler.Instance.currentKills;
            int totalKills = GameplayHandler.Instance.levels[GameManager.Instance.levelSelected].totalKills;
            killsText.text = currentKills.ToString() + " / " + totalKills.ToString(); 
    }

    public void OpenLevelCompletePanelTime(float time)
    {
        Invoke(nameof(OpenLevelCompletePanel), 1f);
    }
    
    public void PauseGame()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        Time.timeScale = 0;
        pauseGamePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        Time.timeScale = 1;
        pauseGamePanel.SetActive(false);
    }

    public void OpenLevelCompletePanel()
    {
        levelCompletePanel.SetActive(true);

        if (GameManager.Instance.levelSelected == 9)
            nextLevelButton.SetActive(false);

        GData.levelsUnlocked++;
        GiveReward();
    }

    public void OpenWatchVideoPanelTime(float time)
    {
        Invoke(nameof(OpenWatchVideoPanel), time);
    }


    public void OpenWatchVideoPanel()
    {
        Time.timeScale = 0;
        watchVideoPanel.SetActive(true);
    }

    public void OnWatchVideoSuccess()
    {
        Time.timeScale = 1;
        
        if (GameManager.Instance.RewardedForBullets)
        {
            GameManager.Instance.RewardedForBullets = false;
            if (weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<genericShooter>())
            {
                weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<genericShooter>().ammo = 300;
                weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<genericShooter>().currentammo = 30;
            }
            if (weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<akimboShooter>())
            {
                weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<akimboShooter>().ammo = 300;
                weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<akimboShooter>().currentammo = 30;
            }
            if (weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<sniper>())
            {
                weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<sniper>().ammo = 100;
                weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<sniper>().currentammo = 10;
            }

            watchVideoPanel.SetActive(false);
        }
        if (GameManager.Instance.RewardedForHealth)
        {
            GameManager.Instance.RewardedForHealth = false;
            WatchVedioPlayerHealth.SetActive(false);

           
        }
        if (GameManager.Instance.RewardedForDog)
        {
            GameManager.Instance.RewardedForDog = false;
            DogInstBtns();
        }
        if (GameManager.Instance.RewardedFor2xDamage)
        {
            GameManager.Instance.RewardedFor2xDamage = false;
            BulletDamageIncrese();
        }
    }
    public void WatchVideo()
    {
        GameManager.Instance.RewardedAd();
    }
    
    public void WatchVideoToGetDog()
    {
        GameManager.Instance.RewardedForHealth = false;
        GameManager.Instance.RewardedForBullets = false;
        GameManager.Instance.RewardedForDog = true;
        GameManager.Instance.RewardedFor2xDamage = false;
        
        GameManager.Instance.RewardedAd();
    }
    public void WatchVideoToGet2XDamage()
    {
        GameManager.Instance.RewardedForHealth = false;
        GameManager.Instance.RewardedForBullets = false;
        GameManager.Instance.RewardedForDog = false;
        GameManager.Instance.RewardedFor2xDamage = true;
        
        GameManager.Instance.RewardedAd();
    }
    public void WatchVideoToGetBullets()
    {
        GameManager.Instance.RewardedForHealth = false;
        GameManager.Instance.RewardedForBullets = true;
        GameManager.Instance.RewardedForDog = false;
        GameManager.Instance.RewardedFor2xDamage = false;
        
        GameManager.Instance.RewardedAd();
    }
    public void PlayerHeathFull()
    {        
        GameManager.Instance.RewardedForHealth = true;
        GameManager.Instance.RewardedForBullets = false;
        GameManager.Instance.RewardedForDog = false;
        GameManager.Instance.RewardedFor2xDamage = false;
        
        GameManager.Instance.RewardedAd();
    }

    public void CloseWatchVideoPanel()
    {
        watchVideoPanel.SetActive(false);
        OpenLevelFailPanel();
    }

    public void CloseWatchVideoPlayerHealth()
    {
        WatchVedioPlayerHealth.SetActive(false);
        OpenLevelFailPanelTime(.1f);
    }

    public void GiveReward()
    {
        int killReward = (GameplayHandler.Instance.levels[GameManager.Instance.levelSelected].totalKills );
        levelCompleteKills.text = killReward.ToString();
        int levelReward = (int)Random.Range(100, 200);
        levelCompleteCashReward.text = levelReward.ToString();
        levelCompleteTotalReward.text = (killReward + levelReward).ToString();
        GData.cash += killReward + levelReward;
      
    }

    public void OpenLevelFailPanelTime(float time)
    {
        SoundManager.instance.PlayVocal(AudioClipsSource.Instance.playerDie);
        Invoke(nameof(OpenLevelFailPanel), 1f);
    }

    public void OpenLevelFailPanel()
    {
        Time.timeScale = 0;
        levelFailPanel.SetActive(true);
    }

    public void BulletUpdate()
    {
        if (IsPistol)
        {
            totalAmmo=   weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<akimboShooter>().ammo+=
                100;
            currentAmmo = weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<akimboShooter>().currentammo;
        }
        else
        {
            totalAmmo=   weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<genericShooter>().ammo+=
                100;
            currentAmmo = weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<genericShooter>().currentammo; 
        }
        

        ammoText.text = currentAmmo.ToString() + " / " + totalAmmo.ToString();
    } 
    public void ReloadBulletUpdate()
    {            Debug.Log("ccccccc");

        if (weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject
            .GetComponent<genericShooter>().currentammo <= 0 && weaponselector.Instance
            .Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<genericShooter>().ammo >= 30)
        {
            currentAmmo = 30;
            totalAmmo -= 30;
            ammoText.text = currentAmmo.ToString() + " / " + totalAmmo.ToString();
            Debug.Log("ddddfffjkghkfhkjh");

        }
        else
        {
            currentAmmo = totalAmmo;
            ammoText.text = currentAmmo.ToString() + " / " + totalAmmo.ToString();


        }

    }
    public void BulletDamageIncrese()
    {
        Time.timeScale = 1;
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.PowerButton);
        Damage2x.GetComponent<Button>().interactable = false;
        if (weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<genericShooter>())
        {
           
   
            
        }
        if (weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<akimboShooter>())
        {
            weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<akimboShooter>().damage = 20;
            weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<genericShooter>().weaponfirer.damage = 20;

        }
        if (weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<sniper>())
        {
            weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<sniper>().damage = 100;
            weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<genericShooter>().weaponfirer.damage = 20;

        }
        DoubleDamage.DOFillAmount(1, 20).OnComplete(delegate
        {
            DoubleDamage.fillAmount = 0;
            Damage2x.GetComponent<Button>().interactable = true;
            if (weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<genericShooter>())
            {
               
            }
            if (weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<akimboShooter>())
            {
                weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<akimboShooter>().damage = 8;
                weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<genericShooter>().weaponfirer.damage = 8;
            }
        });
    }

    public void PlayerPistol()
    {        
        IsPistol = true;
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.ButtonSound);

        SmgRed.SetActive(false); 
        SmgSkin.SetActive(false);
        Pistol.SetActive(true); 
        GameManager.Instance.weaponSelected = 4;

       // Pistol.GetComponent<genericShooter>().Reload();
       UpdateAmmoText();

    }  
    public void SmgRedPlayer()
    {        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.ButtonSound);

        IsPistol = false;
        SmgSkin.SetActive(false);
        Pistol.SetActive(false);
        SmgRedBtn.SetActive(false);
        SmgRed.SetActive(true);
        SmgRed.GetComponent<genericShooter>().Reload();
        GameManager.Instance.weaponSelected = 0;

        UpdateAmmoText();
        SmgSkinBtn.SetActive(true);

    } 
    public void SmgSkinPlayer()
    {    
        IsPistol = false;
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.ButtonSound);

        Pistol.SetActive(false); 
        SmgRed.SetActive(false); 
        SmgSkinBtn.SetActive(false);
        GameManager.Instance.weaponSelected = 2;

        UpdateAmmoText();
        SmgSkin.SetActive(true);
        SmgSkin.GetComponent<genericShooter>().Reload();
        SmgRedBtn.SetActive(true);

    }

    public void DogInstBtns()
    {      
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.PowerButton);

        Dogbtn.GetComponent<Button>().interactable=false;
        DogShadow.DOFillAmount(1, 20).OnComplete(delegate
        {
            Dogbtn.GetComponent<Button>().interactable=true;
            DogShadow.fillAmount = 0;
        });
       
    }

    public void RestartForFreeMode()
    {
        GameManager.Instance.LoadScene("FreeModeGameplay");
    }
}
