using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public static MainMenuHandler instance;

    public GameData GData;
    [SerializeField]
    private GameObject mainMenuPanel;
    [SerializeField]
    private GameObject infoPanel;
    [SerializeField]
    private GameObject settingsPanel;
    [SerializeField]
    private GameObject shopPanel;
    [SerializeField]
    private GameObject exitPanel;
    [SerializeField]
    private GameObject rateUsPanel;
    [SerializeField]
    private GameObject unlockEverythingBtn;
    [SerializeField]
    private GameObject unlockEverythingBtnInShop;
    [SerializeField]
    private GameObject removeAdsBtn;
    [SerializeField]
    private GameObject removeAdsBtnInShop;

    bool musicStatus = true;
    bool soundStatus = true;

    public GameObject musicOn;
    public GameObject soundOn;

    public Text[] coinsText;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    void Start()
    {

        SoundManager.instance.PlayBackgroundMusic(AudioClipsSource.Instance.MainMenuClip);
        
       
    }

   
    public void Music_Btn()
    {
        if(musicStatus)
        {
            SoundManager.instance.MusicEnabled = false;
            //GData.musicStatus = false;
            musicOn.SetActive(false);
            musicStatus = false;
           // PersistentDataManager.instance.SaveData();
        }
        else
        {
            SoundManager.instance.MusicEnabled = true;
            SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
            musicOn.SetActive(true);
            //GData.musicStatus = true;
            musicStatus = true;
            //PersistentDataManager.instance.SaveData();
        }
    }

    public void Sound_Btn()
    {
        if (soundStatus)
        {
            SoundManager.instance.EffectsEnabled = false;
            //GData.musicStatus = false;
            soundOn.SetActive(false);
            soundStatus = false;
           // PersistentDataManager.instance.SaveData();
        }
        else
        {
            SoundManager.instance.EffectsEnabled = true;
            SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
          //  GData.musicStatus = true;
            soundOn.SetActive(true);
            soundStatus = true;
          //  PersistentDataManager.instance.SaveData();Manager.instance.SaveData();
        }
    }

   

    public void OpenSettingsPanel()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void CloseSettingsPanel()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OpenShopPanel()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        shopPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void CloseShopPanel()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        shopPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    

    public void OpenRateUsPanel()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        rateUsPanel.SetActive(true);
    }

    public void CloseRateUsPanel()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        rateUsPanel.SetActive(false);
    }

    public void PlayGame()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
      
        GameManager.Instance.LoadScene("EnvironmentSelection");
    }

    public void QuitGame()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        Application.Quit();
    }

    
}
