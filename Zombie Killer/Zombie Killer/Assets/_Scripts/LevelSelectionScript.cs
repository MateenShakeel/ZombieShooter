using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScript : MonoBehaviour
{
    public static LevelSelectionScript instance;
    public GameData GD;
    public GameObject[] levelsLockedState;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    private void Start()
    {
        
        //GameManager.instance.ResetConditions();
        //text.text = GD.coins.ToString();
        Unlock_Levels();
    }


  

    public void GoBack()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        //GameManager.instance.isModeSelection = true;
        //GameManager.instance.LoadScene("ModeSelection");
    }

    public void SelectedLevel(int level)
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
      ////  GameManager.instance.levelSelected = level;
       // GameManager.instance.isEnvironmentSelection = true;
        //GameManager.instance.LoadScene("Loading");
        //selectButton.SetActive(true);

    }

    public void Unlock_Levels()
    {
        for (int i = 0; i < GD.levelsUnlocked - 1 ; i++)
        {
            levelsLockedState[i].SetActive(false);

        }
    }

    //public void StartGame()
    //{
    //    SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
    //    GameManager.instance.isGunSelection = true;
    //    GameManager.instance.LoadScene("Loading");
    //}
}
