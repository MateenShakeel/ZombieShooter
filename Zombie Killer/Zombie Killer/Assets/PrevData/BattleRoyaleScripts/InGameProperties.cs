using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InGameProperties : MonoBehaviour
{
    
    #region GlobalInstance

    public static InGameProperties localInstance;
    public static InGameProperties Instance
    {
        get
        {
            localInstance = GameObject.FindObjectOfType<InGameProperties>();
            return localInstance;
        }
    }
    
    #endregion

    #region Generic Properties
    
    public GameObject screenFade;
    
    [Space(5)] 
    [Header("--- Damage Indicator")]
    public RectTransform indicator;
    public RectTransform indicatorForZombies;
    public RectTransform ZombieHandSign;
    public GameObject zombiekillIndicator;
    Transform otherTransform;
	
    public Transform player;
    public bool useDirection;
	
    private float angle;
    
    [Space(5)] 
    [Header("--- CrossHair-HitMarker")]
    
    public Color normelColor;
    public Color enemyPointColor;
    
    public Image[] crossHair;
    public Animator crossHairAnimator;

    public GameObject hitMarker;
    
    [Space(5)] 
    [Header("--- Stats")]
    
    public Text[] headShot;
    public Text[] bodyShot;
    public Text levelRewardText;
    public Text doubleRewardText;
    public Button doubleRewardedBtn;
    [HideInInspector]public bool isDoubleReward;
    
    public Text currentKill;
    
    public bool isbodyShot = true;
    
    [Space(5)] 
    [Header("--- Setting")]
    
    public GameObject[] autoShootOn;
    public GameObject[] autoShootOff;
    
    public GameObject soundOn;
    public GameObject soundOff;
    public GameObject musicOn;
    public GameObject musicOff;
    public GameObject vibrationOn;
    public GameObject vibrationOff;
    public GameObject vibration;
    
    [Space(5)] 
    [Header("--- Reload Notification")]
    public GameObject reloadTime;
    public bool isReloadTime = false;

    public GameObject headShotEffect;
    
    
    
    [Space(5)] 
    [Header("--- Scripts Refrences")]
    public playercontroller playerController;
    
    
    
    [Header("--- Zombie---")] public GameObject zombieControls;
    public GameObject touchScreen;
    public Transform canvas;
    public RuntimeAnimatorController AnimatorController;

    #endregion
    
    #region Initilization

    private void Awake()
    {
        Time.timeScale = 1;
        if (!PlayerPrefs.HasKey("AutoShoot"))
        {
            PlayerPrefs.SetInt("AutoShoot",1);
            AutoShoot_Off();
        }
        else
        {
            AutoshootStatus();
        }
    }

    private void Start()
    {
        // StartCoroutine(ScreenFadeIn());
        
        StatsValueNull();
        ChangeSfxVolume();
        ChangeMusicVolume();
        indicator.gameObject.SetActive (false);

        if (vibration != null)
        {
          if (PlayerPrefs.GetString("Mode") == "Zombie")
          {
              vibration.SetActive(true);
          }else
                vibration.SetActive(false);
            
        }
        
    }


    public void ZombieMode()
    {
        foreach (var item in autoShootOn)
        {
            item.SetActive(false);
        }
        foreach (var item in autoShootOff)
        {
            item.SetActive(false);
        }
        zombieControls.SetActive(true);
        touchScreen.SetActive(false);
    }
    
    private IEnumerator ScreenFadeIn()
    {
        screenFade.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        screenFade.SetActive(false);
    }
    
    #endregion
    
    #region Setting

    public void SettingReset()
    {
        
        PlayerPrefs.SetFloat("PlayerMove_Speed",5f);
        playerController.MovementSpeedBar.value = PlayerPrefs.GetFloat("PlayerMove_Speed");
        playerController.MoveSpeedValue.text = Mathf.RoundToInt(playerController.MovementSpeedBar.value * 10 )+"%";
    
        PlayerPrefs.SetFloat("TouchSens",2f);
        playerrotate.Instance.SensitivityBar.value = PlayerPrefs.GetFloat("TouchSens");
        playerrotate.Instance.SensPercenetage.text = Mathf.RoundToInt(playerrotate.Instance.SensitivityBar.value * 10 )+"%";
        
    }
  public void AutoshootStatus()
    {
        if (PlayerPrefs.GetInt("AutoShoot") == 0)
        {
            AutoShoot_On();
        }
        else
        {
            AutoShoot_Off();
        }
    }
    public void AutoShoot_On()
    {
        PlayerPrefs.SetInt("AutoShoot",0);
        
        foreach (var item in autoShootOn)
        {
            item.SetActive(false);
        }

        foreach (var item in autoShootOff)
        {
            item.SetActive(true);
        }
    }
    public void AutoShoot_Off()
    {
        PlayerPrefs.SetInt("AutoShoot",1);
        foreach (var item in autoShootOn)
        { 
            item.SetActive(true);
        }

        foreach (var item in autoShootOff)
        { 
            item.SetActive(false);
        }
      
        
    }
    
    #endregion
    
    #region Game Stats
    
    private void StatsValueNull()
    {
        totalHeadShot = 0;
        totalBodyShot = 0;
        
        bodyShotIndex = 0;
        currentKill.text = 0.ToString();
    }
    
    private int totalHeadShot;
    public void HeadShotEvent()
    {
        totalHeadShot++;
        headShot[0].text = headShot[1].text = totalHeadShot.ToString();
        // StartCoroutine(HeadShotEffect1());
        
        PlayerPrefs.SetInt("HeadShot",PlayerPrefs.GetInt("HeadShot")+1);
        
        if (PlayerPrefs.GetString("Mode") == "Campaign")
        {
            //if (LevelsController.Instance.levelData[GameManager.Instance.currentLevel - 1].objectives2 ==
            //    ObjectiveType.HeadShot)
            //{
            //    if (totalHeadShot == LevelsController.Instance.levelData[GameManager.Instance.currentLevel  - 1].obj2)
            //    {
            //        PlayerPrefs.SetInt("Star", PlayerPrefs.GetInt("Star") + 1);
            //    }
            //}
        }
    }

    private IEnumerator HeadShotEffect1()
    {
        headShotEffect.SetActive(true);
        
        SoundController.instance.PlayFromPool(AudioType.HeadShot);
        yield return new WaitForSeconds(1.5f);
        headShotEffect.SetActive(false);
    }

    private int totalBodyShot;
    [HideInInspector]public int bodyShotIndex;
    
    public void BodyShotEvent()
    {
        if (isbodyShot)
        {
            totalBodyShot++;
            PlayerPrefs.SetInt("BodyShot",PlayerPrefs.GetInt("BodyShot")+1);
            bodyShot[0].text = bodyShot[1].text = totalBodyShot.ToString();
            
            //if (PlayerPrefs.GetString("Mode") == "Campaign")
            //{
            //    SoundController.instance.PlayFromPool(AudioType.BodyShot);
            //    if (LevelsController.Instance.levelData[GameManager.Instance.currentLevel-1].objectives2 == ObjectiveType.BodyShot) 
            //    { 
            //        if (totalBodyShot == LevelsController.Instance.levelData[GameManager.Instance.currentLevel-1].obj2)
            //        {
            //          PlayerPrefs.SetInt("Star", PlayerPrefs.GetInt("Star")+1);
            //        }
            //    }
            //}
        }
        
        bodyShotIndex++;
        
        PlayerPrefs.SetInt("totalKill",PlayerPrefs.GetInt("totalKill")+1);
        currentKill.text = bodyShotIndex.ToString();
        
    }

    
    public void GameStats()
    {
        if (PlayerPrefs.GetString("Mode") == "Campaign")
        {
            levelRewardText.text = $"{LevelsController.Instance.levelReward[LevelsController.Instance.currentLevel]}";
            PlayerPrefs.SetInt("Cash",PlayerPrefs.GetInt("Cash")+LevelsController.Instance.levelReward[LevelsController.Instance.currentLevel]);
            doubleRewardText.text = (LevelsController.Instance.levelReward[LevelsController.Instance.currentLevel]*2).ToString();
        }
        //else if (PlayerPrefs.GetString("Mode") == "Sniper")
        //{
        //    levelRewardText.text = $"{LevelsControllerSniper.Instance.missionReward[GameManager.Instance.currentLevel-1]}";
        //    PlayerPrefs.SetInt("Cash",PlayerPrefs.GetInt("Cash")+LevelsControllerSniper.Instance.missionReward[GameManager.Instance.currentLevel-1]);
        //    doubleRewardText.text = (LevelsControllerSniper.Instance.missionReward[GameManager.Instance.currentLevel-1]*2).ToString();
        //}
    }

    
    public void GameDoubleReward()
    {
        if (PlayerPrefs.GetString("Mode") == "Campaign")
        {
            doubleRewardText.text = $"{LevelsController.Instance.levelReward[LevelsController.Instance.currentLevel]*2}";
            PlayerPrefs.SetInt("Cash",PlayerPrefs.GetInt("Cash")+ (LevelsController.Instance.levelReward[LevelsController.Instance.currentLevel]*2));
            doubleRewardedBtn.interactable = false;
        }
       // else if (PlayerPrefs.GetString("Mode") == "Sniper")
        //{
 //           doubleRewardText.text = $"{LevelsControllerSniper.Instance.missionReward[GameManager.Instance.currentLevel-1]*2}";
   //         PlayerPrefs.SetInt("Cash",PlayerPrefs.GetInt("Cash")+ (LevelsControllerSniper.Instance.missionReward[GameManager.Instance.currentLevel-1]*2));
     //       doubleRewardedBtn.interactable = false;
        //}

        

    }
    private void CompleteMethod(bool completed)
    {
       
            //	GleyMobileAds.ScreenWriter.Write("Completed " + completed);
            if (completed == true)
            {
            GameDoubleReward();
        }
            else
            {
            
           

        }
    }
    public void GetDoubleReward()
    {
        // if (PlayerPrefs.GetString("removeAd", "no").Equals("no"))
        //     Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
        //if (Application.internetReachability != NetworkReachability.NotReachable)
        //{
        //    isDoubleReward = true;
        //   // AdsController.Instance.ShowRewarded();
        //}
    }

    #endregion
    
    #region SFX_Music Volume
    
    public void MusicVoluneOn()
    {
        PlayerPrefs.SetInt("musicvolume",1);
        musicOn.SetActive(true);
        musicOff.SetActive(false);
        //SoundController.instance.MusicVolumeChanged(1);
    }
    public void MusicVoluneOff() 
    {
        PlayerPrefs.SetInt("musicvolume",0);
        musicOn.SetActive(false);
        musicOff.SetActive(true);
        //SoundController.instance.MusicVolumeChanged(0);
    }
    
    public void SfxVolumeOn()
    {
            PlayerPrefs.SetInt("sfxvolume",1);
            soundOn.SetActive(true);
            soundOff.SetActive(false);
            //SoundController.instance.SfxVolumeChanged(1);
    }
    
    public void SfxVolumeOff()
    {
        PlayerPrefs.SetInt("sfxvolume",0);
        soundOn.SetActive(false);
        soundOff.SetActive(true);
        //SoundController.instance.SfxVolumeChanged(0);
    }

    public void VibrationOn()
    {
        PlayerPrefs.SetInt("Vibration",1);
        vibrationOn.SetActive(true);
        vibrationOff.SetActive(false);
    }

    public void VibrationOff()
    {
        PlayerPrefs.SetInt("Vibration", 0);
        vibrationOn.SetActive(false);
        vibrationOff.SetActive(true);
    }

    public void ChangeSfxVolume()
    {
        if (PlayerPrefs.GetInt("sfxvolume")  == 0)
        {
            SfxVolumeOff();
        }
        else
        {
            SfxVolumeOn();
        }

    }
    
    public void ChangeMusicVolume()
    {
        if (PlayerPrefs.GetInt("musicvolume")  == 0)
        {
            MusicVoluneOff();
        }
        else
        {
            MusicVoluneOn();
        }
        //SoundController.instance.MusicVolumeChanged(PlayerPrefs.GetInt("musicvolume"));
            
    }
    
    #endregion
    
    #region Reload Notification
    
    public void ReloadEnd()
    {
        // reloadTime.SetActive(false);
        isReloadTime = false;
        //if (PlayerPrefs.GetString("Mode") == "TeamDeath")
        //{
        //    TeamDeathManager.Instance.isReload = false;
        //}
    }
    public void ReloadStart()
    {
        // reloadTime.SetActive(true);
        isReloadTime = true;
        
    }

    #endregion
    
    #region Indicator Damager

    private void Update()
    {
        if (useDirection) {
            // Angle based on the direction of the shooter relative to player
            if (otherTransform == null) {
                //
            } else {
                angle = GetHitAngle (player, otherTransform.forward);
                indicator.rotation = Quaternion.Euler (0, 0, -angle);
            }
        } else if (!useDirection) {
            // Angle taken from other objects pos and player
            if (otherTransform == null) {
                //
            } else {
                angle = GetHitAngle (player, (player.position - otherTransform.position).normalized);
                indicator.rotation = Quaternion.Euler (0, 0, -angle);
            }
        }
    }

    public float GetHitAngle(Transform player, Vector3 incomingDir)
    {
        // Flatten to plane
        var otherDir = new Vector3(-incomingDir.x, 0f, -incomingDir.z);
        var playerFwd = Vector3.ProjectOnPlane(this.player.forward, Vector3.up);

        // Direction between player fwd and incoming object
        var angle = Vector3.SignedAngle(playerFwd, otherDir, Vector3.up);

        return angle;
    }
	
    public void IndicatorArrow(Transform obj)
    {
        otherTransform = obj;
        StartCoroutine (IndicatorCoroutine ());
    }
	
    private IEnumerator IndicatorCoroutine()
    {
        if (PlayerPrefs.GetString("Mode") != "Zombie")
        {
            yield return new WaitForSeconds (0f);
            indicator.gameObject.SetActive (true);
            yield return new WaitForSeconds (2f);
            indicator.gameObject.SetActive (false);
        }
        
    }
    
    public IEnumerator EnemyAttackIndicator()
    {
        
        var createImage = Instantiate(ZombieHandSign);
        createImage.transform.SetParent(canvas.transform, false);
        createImage.rotation = Quaternion.Euler (0, 0, Random.Range(-20,80));
        if (PlayerPrefs.GetInt("Mirror") == 0)
            createImage.anchoredPosition = new Vector3(Random.Range(-115,-312),Random.Range(-70,70),Random.Range(-1,1));
        else
            createImage.anchoredPosition = new Vector3(Random.Range(115,312),Random.Range(-70,70),Random.Range(-1,1));
            
        createImage.gameObject.SetActive(true);
        indicatorForZombies.gameObject.SetActive(true);
        yield return new WaitForSeconds (0.5f);
        indicatorForZombies.gameObject.SetActive(false);
        Destroy(createImage.gameObject);
    }
    

    #endregion

    #region CrossHair

     public void CrossHairZoomOut()
        {
            crossHairAnimator.Play("ZoomOut");
        }
    
        public void CrossHairZoomIn()
        {
            crossHairAnimator.Play("ZoomIn");
        }
    
        public void CrossAssistColor()
        {
            foreach (var item in crossHair)
            {
                item.color = enemyPointColor;
            }  
            hitMarker.SetActive(true);
        }
    
        public void CrossAimColor()
        {
            foreach (var item in crossHair)
            {
                item.color = normelColor;
            }  
            hitMarker.SetActive(false);
        }
    
        public void CrosshairEnable()
        {
            foreach (var item in crossHair)
            {
                item.gameObject.SetActive(true);
            }
        }
        public void CrosshairDisable()
        {
            foreach (var item in crossHair)
            {
                item.gameObject.SetActive(false);
            }
        }

    #endregion

    public void CollectItem()
    {
        if (PlayerPrefs.GetInt("sfxvolume") == 1)
        {
            GetComponent<AudioSource>().Play();
        }
        else
        {
            GetComponent<AudioSource>().volume = 0;
        }
        
    }
    
    public void KillIndicatorOn()
    {
        if (PlayerPrefs.GetString("Mode") == "Zombie")
            StartCoroutine(KillIndicatorOff());
        
    }

    IEnumerator KillIndicatorOff()
    {
        zombiekillIndicator.SetActive(true);
        yield return new WaitForSeconds(1f);
        zombiekillIndicator.SetActive(false);
    }
    
    
}
