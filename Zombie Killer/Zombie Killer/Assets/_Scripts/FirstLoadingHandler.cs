
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
        if (GData.firstTime)
        {
            // GameManager.instance.firstTimeGameplay = true;
            GData.firstTime = false;
            GData.levelsUnlocked = 0;
            GData.Weapons[0].isUnlocked = true;


            for (int i = 1; i < GData.Weapons.Length; i++)
            {
                GData.Weapons[i].isUnlocked = false;
            }

            PersistentDataManager.instance.SaveData();
        }
    }
}
