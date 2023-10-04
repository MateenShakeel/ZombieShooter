using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayHandler : MonoBehaviour
{
    #region SINGLETON
    public static GameplayHandler Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("Player Controller")]
    public Transform _playerController;
    public Camera PlayerCamera;
    public GameObject PlayerVolumetricLight;

    [Header("Modes")]
    public Mode[] Modes;


    Mode currentMode;
    SurvivalLevel currentSurvivalLevel;
    CampaignLevel currentCampaignLevel;

    private void Start()
    {
        Time.timeScale = 1;
        SoundManager.instance.PlayBackgroundMusic(AudioClipsSource.Instance.GamePlayClips[0]);
        InitializeVariables();
        StartGame();
    }

    private void InitializeVariables()
    {
        currentMode = Modes[GameManager.Instance.SelectedMode];
        if(GameManager.Instance.SelectedMode == 0)
            currentSurvivalLevel = currentMode.SurvivalLevels[GameManager.Instance.levelSelected];
        else
            currentCampaignLevel = currentMode.CampaignLevels;

    }

    void StartGame()
    {
        if (GameManager.Instance.SelectedMode == 0)
            StartSurvivalMode();
        else
            StartCampaignMode();

    }

    private void StartSurvivalMode()
    {
        //Activating Player And Setting Transform
        _playerController.transform.position = currentSurvivalLevel.PlayerPosition.transform.position;
        _playerController.transform.rotation = currentSurvivalLevel.PlayerPosition.transform.rotation;
        _playerController.gameObject.SetActive(true);


        //Activating Wave Spawners
        for (int i = 0; i < currentSurvivalLevel.ZombieSpawners.Length; i++)
        {
            currentSurvivalLevel.ZombieSpawners[i].SetActive(true);
        }


        //Activating Collider Props
        for (int i = 0; i < currentSurvivalLevel.ColliderProps.Length; i++)
        {
            currentSurvivalLevel.ColliderProps[i].SetActive(true);
        }
    }

    private void StartCampaignMode()
    {
        
        _playerController.GetComponent<playercontroller>().runspeed = 5;
        //Activating Player And Setting Transform
        _playerController.transform.position = currentCampaignLevel.PlayerPosition.transform.position;
        _playerController.transform.rotation = currentCampaignLevel.PlayerPosition.transform.rotation;
        PlayerCamera.farClipPlane = 750;
        
        RenderSettings.fog = true;
        _playerController.gameObject.SetActive(true);
        PlayerVolumetricLight.SetActive(true);
        currentCampaignLevel.CampaignMode.SetActive(true);
    }

}

[System.Serializable]
public class Mode
{
    public SurvivalLevel[] SurvivalLevels;
    public CampaignLevel CampaignLevels;
}

[System.Serializable]
public class SurvivalLevel
{
    public Transform PlayerPosition;
    public GameObject[] ZombieSpawners;
    public GameObject[] ColliderProps;

}

[System.Serializable]
public class CampaignLevel
{
    public Transform PlayerPosition;
    public GameObject CampaignMode;

}

