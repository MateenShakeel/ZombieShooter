using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    
    #region GlobalInstance
    public static Setting localInstance;
    public static Setting Instance
    {
        get
        {
            localInstance = GameObject.FindObjectOfType<Setting>();
            return localInstance;
        }
    }
    #endregion

    #region Properties

    [Space(5)] [Header("--- Setting")][Space(3)]
    public GameObject soundOn;
    public GameObject soundOff;
    [Space(3)]
    public GameObject musicOn;
    public GameObject musicOff;
    [Space(3)]
    public GameObject autoShootOn;
    public GameObject autoShootOff;
    [Space(3)] public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip[] uiClips;
    
    #endregion
    
    #region Initilization 

    private void Start()
    {
        
        ////////////////////////////////////// Setting
        
        ChangeMusicVolume();
        ChangeSfxVolume();
        AutoShootStatus();

    }
    
    #endregion
    
    #region Setting

    #region Music
    
    public void MusicVoluneOn()
    {
        PlayerPrefs.SetInt("musicvolume",1);
        musicOn.SetActive(true);
        musicOff.SetActive(false);
        musicSource.gameObject.SetActive(true);
    }
    public void MusicVoluneOff()
    {
        PlayerPrefs.SetInt("musicvolume",0);
        musicOn.SetActive(false);
        musicOff.SetActive(true);
        musicSource.gameObject.SetActive(false);
    }
    
    private void ChangeMusicVolume()
    {
        if (!PlayerPrefs.HasKey("musicvolume"))
        {
            MusicVoluneOn();
        }
        
        if (PlayerPrefs.GetInt("musicvolume")  == 0)
        {
            MusicVoluneOff();
        }
        else
        {
            MusicVoluneOn();
        }
    }
    
    #endregion

    #region SFX Sound

    public void SfxVolumeOff()
    {
        PlayerPrefs.SetInt("sfxvolume",0);
        soundOn.SetActive(false);
        soundOff.SetActive(true);
        sfxSource.gameObject.SetActive(false);
    } 
    
    public void SfxVolumeOn()
    {
        PlayerPrefs.SetInt("sfxvolume",1);
        soundOn.SetActive(true);
        soundOff.SetActive(false);
        sfxSource.gameObject.SetActive(true);
    }

    private void ChangeSfxVolume()
    {
        if (!PlayerPrefs.HasKey("sfxvolume"))
        {
            SfxVolumeOn();
        }
        
        if (PlayerPrefs.GetInt("sfxvolume") == 0)
        {
            SfxVolumeOff();
        }
        else
        {
            SfxVolumeOn();
        }
    }
   
    #endregion
    
    #region Auto Shoot

    public void AutoShootOff()
    {
        PlayerPrefs.SetInt("AutoShoot",0);
        autoShootOn.SetActive(false);
        autoShootOff.SetActive(true);
    } 
    
    public void AutoShootOn()
    {
        PlayerPrefs.SetInt("AutoShoot",1);
        autoShootOn.SetActive(true);
        autoShootOff.SetActive(false);
    }

    private void AutoShootStatus()
    {
        if (!PlayerPrefs.HasKey("AutoShoot"))
        {
            AutoShootOn();
        }
        
        if (PlayerPrefs.GetInt("AutoShoot") == 0)
        {
            AutoShootOff();
        }
        else
        {
            AutoShootOn();
        }
    }
   
    #endregion
    
    
    public void ClickAudio(int r)
    {
        sfxSource.PlayOneShot(uiClips[r]);
    }
    
    #endregion

}
