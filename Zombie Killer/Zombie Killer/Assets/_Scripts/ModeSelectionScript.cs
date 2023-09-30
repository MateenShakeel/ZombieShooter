using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectionScript : MonoBehaviour
{ 
    public void GoBack()
    {
       SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        GameManager.Instance.LoadScene("MainMenu");
    }

    public void SelectSurvival()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);


        GameManager.Instance.SelectedMode = 0;

        int randomizer = Random.Range(0, 4);
        GameManager.Instance.levelSelected = randomizer;
        GameManager.Instance.LoadScene("GunSelection");
  
    }

    public void SelectCampaign()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
       

        GameManager.Instance.SelectedMode = 1;
        
        
        GameManager.Instance.LoadScene("GunSelection");
        
    }

   
}
