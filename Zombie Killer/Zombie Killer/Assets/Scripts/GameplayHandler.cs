using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class GameplayHandler : MonoBehaviour
{

    #region SINGLETON
    public static GameplayHandler Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    public GameData GData;

    [Header("Player")]
    public GameObject FPSPlayer;

    [Header("CF2")]
    public GameObject CF2Canvas;
    public GameObject mainControls;

    [Header("UGUIMiniMap")]
    public GameObject miniMap;

    [Header("Variables")]
    public int currentKills;

    [Header("Levels")]
    public Levels[] levels;

    [Header("Audio Sources")]
    public AudioSource[] aS;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ClearSceneBool();
        SoundManager.instance.StopBackgroundMusic(AudioClipsSource.Instance.MainMenuClip);
        SoundManager.instance.PlayBackgroundMusic(AudioClipsSource.Instance.GamePlayClip);
        StartLevel();
        HandleSound();
        
        //GameManager.Instance.GamePlayStart();
        //GameManager.Instance.GamePlayGA()
    }

    public void HandleSound()
    {
        if(!GData.soundEnabled)
        {
            foreach(AudioSource aS in aS)
            {
                aS.enabled = false;
            }
        }
    }

    void ActivateCutCam()
    {
        miniMap.SetActive(false);
        CF2Canvas.GetComponent<Canvas>().enabled = false;
        levels[GameManager.Instance.levelSelected].StartingCutCam.SetActive(true);
        float time = (float)levels[GameManager.Instance.levelSelected].StartingCutCam.GetComponent<PlayableDirector>().duration;
        Invoke("DeactivateCutCam", time);
    }


    void DeactivateCutCam()
    {
        
        levels[GameManager.Instance.levelSelected].StartingCutCam.SetActive(false);
        miniMap.SetActive(true);
        CF2Canvas.GetComponent<Canvas>().enabled = true;
        StartGameplay();
    }

    void StartLevel()
    {
        if (levels[GameManager.Instance.levelSelected].StartingCutCam)
            ActivateCutCam();
        else
            StartGameplay();
    }

    //Start the Gameplay by activating player and enemies
    void StartGameplay()
    {
        UIHandler.Instance.OpenObjectivePanel();
        ActivateZombies();
        ActivatePlayer();
    }

    //Activate Player and Set Position and Rotation
    void ActivatePlayer()
    {
        FPSPlayer.transform.position = levels[GameManager.Instance.levelSelected].playerPosition.position;
        FPSPlayer.transform.rotation = levels[GameManager.Instance.levelSelected].playerPosition.rotation;
        FPSPlayer.SetActive(true);

        UIHandler.Instance.canUpdateText = true;
        //weaponselector.Instance.ABC();
    }

    //Activate the Zombies
    void ActivateZombies()
    {
        for (int i = 0; i < levels[GameManager.Instance.levelSelected].zombies.Length; i++)
        {
            levels[GameManager.Instance.levelSelected].zombies[i].SetActive(true);
        }
    }

    //Increment Kills And Then Check For Level Complete
    public void KillHandler()
    {
        currentKills++;
        if(currentKills >= levels[GameManager.Instance.levelSelected].totalKills)
        {
            //Level Complete
            //FPSPlayer.SetActive(false);
            mainControls.SetActive(false);
            UIHandler.Instance.YouWonPanel.SetActive(true);
            SoundManager.instance.PlayEffect(AudioClipsSource.Instance.ImpactHit);

        }
    }

    public void GoToNextLevel()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        Time.timeScale = 1;
        GameManager.Instance.levelSelected++;
        GameManager.Instance.isGameplay = true;
        GameManager.Instance.LoadScene("Loading");
    }

    public void RestartLevel()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        Time.timeScale = 1;
        GameManager.Instance.isGameplay = true;
        GameManager.Instance.LoadScene("Loading");
    }

    public void GoToMainMenu()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        Time.timeScale = 1;
     //   GameManager.Instance.isSurvialMode = false;

        GameManager.Instance.isMainMenu = true;
        GameManager.Instance.LoadScene("Loading");
        
        GameManager.Instance.HomeBtnAd();
    }
}


[System.Serializable]
public class Levels
{
    public string levelName;
    public int cashToGive;
    public int totalKills;
    public string objectiveText;
    public Transform playerPosition;
    public GameObject StartingCutCam;
    public GameObject[] zombies;
}