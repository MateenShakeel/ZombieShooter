
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FirstLoadingHandler : MonoBehaviour
{
    public GameData GData;
    public Image fillImage;
    public Text loadingText;

    private void Start()
    {
        Application.targetFrameRate = 300;
        fillImage.fillAmount = 0;
        Invoke("FirstTime", 1f);
        StartLoading();
    }

   
   

    void StartLoading()
    { 
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        yield return new WaitForSeconds(0.02f);

        if(fillImage.fillAmount < 1)
        {
            fillImage.fillAmount += 0.005f;
            float fill = fillImage.fillAmount * 100;
            loadingText.text = ((int)fill).ToString() + "%";
            StartCoroutine(Loading());
        }
        if(fillImage.fillAmount >= 1)
        {
                GameManager.Instance.LoadScene("MainMenu");
        }
    }

    public void FirstTime()
    {
        //if (GData.isFirst)
        //{
        //   // GameManager.instance.firstTimeGameplay = true;
        //    GData.isFirst = false;
        //    //GData.isTutorial = true;
        //    GData.coins = 0;
        //    //GData.isEverythingUnlocked = false;
        //    //GData.isLevelsUnlocked = false;
        //    //GData.musicStatus = true;
        //    //GData.soundStatus = true;
        //    //GData.removeAds = false;
        //    //GData.levelsCompleted = 0;
        //    GData.levelsUnlocked = 1;
        //    GData.guns[0].isUnlocked = true;
        //    GData.guns[1].isUnlocked = true;

        //    for (int i = 2; i < GData.guns.Length; i++)
        //    {
        //        GData.guns[i].isUnlocked = false;
        //    }

        //    PersistentDataManager.instance.SaveData();
        //}
    }
}
