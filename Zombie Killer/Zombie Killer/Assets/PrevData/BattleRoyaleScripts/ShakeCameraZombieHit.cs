using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShakeCameraZombieHit : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource _audio;
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    void attackIndicator()
    {
        cameraShake.Instance.ShakeStart();
        StartCoroutine(InGameProperties.Instance.EnemyAttackIndicator());
        _audio.PlayOneShot(clips[Random.Range(0,clips.Length)]);
    }
}
