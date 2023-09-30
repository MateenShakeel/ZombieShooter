using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSelectionHandler : MonoBehaviour
{
    private void Start()
    {
        //GameManager.instance.ResetConditions();
    }

    public void GoNext()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        // GameManager.instance.isDifficultySelection = true;
        GameManager.Instance.LoadScene("Loading");
    }
}
