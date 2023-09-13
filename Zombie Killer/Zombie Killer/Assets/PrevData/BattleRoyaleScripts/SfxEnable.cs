using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxEnable : MonoBehaviour
{
    public AudioSource audioSource;
    private void OnEnable()
    {
        audioSource.volume = PlayerPrefs.GetInt("sfxvolume") == 1 ? 1 : 0;
    }
    
}
