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
    public Text waveAlert;

    [Header("Panels")]
    public GameObject levelCompletePanel;
    public GameObject YouWonPanel;
    public GameObject levelFailPanel;
    public GameObject pauseGamePanel;
    public GameObject watchVideoPanel;

    public GameObject survivalPanel;
    public GameObject campaignPanel;

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

    private void Start()
    {
        if (GameManager.Instance.SelectedMode == 0)
            survivalPanel.SetActive(true);
        if (GameManager.Instance.SelectedMode == 1)
            campaignPanel.SetActive(true);

    }

    private void Update()
    {
        if (canUpdateText)
        {
            UpdateAmmoText();
        }
    }

    public void ShowWaveAlert()
    {
        waveAlert.gameObject.SetActive(true);
        waveAlert.gameObject.GetComponent<DOTweenAnimation>().DOPlayForward();
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
        if (weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<shotgun>())
        {
            totalAmmo = weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<shotgun>().ammo;
            currentAmmo = weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<shotgun>().currentammo;
        }
        if (weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<sniper>())
        {
             totalAmmo = weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<sniper>().ammo;
             currentAmmo = weaponselector.Instance.Weapons[GameManager.Instance.weaponSelected].gameObject.GetComponent<sniper>().currentammo;
        }
             ammoText.text = currentAmmo.ToString() + " / " + totalAmmo.ToString();
         
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

    public void RestartLevel()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        Time.timeScale = 1;
        GameManager.Instance.LoadScene("Gameplay");
    }

    public void GoToHome()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        Time.timeScale = 1;
        GameManager.Instance.LoadScene("MainMenu");
    }

    public void OpenLevelCompletePanel()
    {
        levelCompletePanel.SetActive(true);
    } 

    public void OpenLevelFailPanelTime(float time)
    {
       // SoundManager.instance.PlayVocal(AudioClipsSource.Instance.playerDie);
        Invoke(nameof(OpenLevelFailPanel), 1f);
    }

    public void OpenLevelFailPanel()
    {
        Time.timeScale = 0;
        levelFailPanel.SetActive(true);
    }
}
