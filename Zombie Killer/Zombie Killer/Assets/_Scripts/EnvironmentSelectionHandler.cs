using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSelectionHandler : MonoBehaviour
{

    private void Start()
    {
        //GameManager.instance.ResetConditions();
    }

    public void GoNext()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        //GameManager.instance.isWeatherSelection = true;
        GameManager.Instance.LoadScene("ModeSelection");
    }
}
