using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingHandler : MonoBehaviour
{
    public Image fillImage;


    private void Start()
    {
        fillImage.fillAmount = 0;
        StartLoading();
        //if(!GameManager.instance.isGameplay)
        //    StartLoading();
        //else
        //    StartCoroutine(LoadGameplay());

    }

    //private void Update()
    //{
    //    if (GameManager.instance.isGameplay)
    //        StartCoroutine(LoadGameplay());
    //}
    void StartLoading()
    {
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        yield return new WaitForSeconds(0.02f);

        if (fillImage.fillAmount < 1)
        {
            fillImage.fillAmount += 0.015f;
            StartCoroutine(Loading());
        }
        else
        {
            StopAllCoroutines();
            GameManager.Instance.LoadScene("Gameplay");
        }
    }
    //public void LoadScene()
    //{
    //    if (GameManager.instance.isModeSelection)
    //        GameManager.instance.LoadScene("ModeSelection");
    //    else if (GameManager.instance.isGunSelection)
    //        GameManager.instance.LoadScene("GunSelection");
    //    else if (GameManager.instance.isLevelSelection)
    //        GameManager.instance.LoadScene("LevelSelection");
    //    else if (GameManager.instance.isMainMenu)
    //        GameManager.instance.LoadScene("MainMenu");
    //    else if (GameManager.instance.isDifficultySelection)
    //        GameManager.instance.LoadScene("DifficultySelection");
    //    else if (GameManager.instance.isWeatherSelection)
    //        GameManager.instance.LoadScene("WeatherSelection");
    //    else if (GameManager.instance.isEnvironmentSelection)
    //        GameManager.instance.LoadScene("EnvironmentSelection");

    //}

    IEnumerator LoadGameplay()
    {

        AsyncOperation loading = SceneManager.LoadSceneAsync("Gameplay");

        while(!loading.isDone)
        {
            fillImage.fillAmount = Mathf.Clamp01(loading.progress / 0.09f);
            yield return null;
        }
    }
}
