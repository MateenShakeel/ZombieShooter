using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundController : MonoBehaviour
{
	private float sfxVolume;
	private float musicVolume;

	public AudioSource audioMusic;
	public AudioSource[] gameAudioSources;

	public AudioPool[] pool;

	public AudioClip[] clickAudioClips;
	public AudioClip[] campaignVoices;
	public AudioClip musicAudio;
	
	private Dictionary<AudioType, AudioPool> audioList;
	private static SoundController _instance;
	
	public static SoundController instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<SoundController>();
			}
			return _instance;
		}
	}

	private void Awake()
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "AudioPool";
		audioList = new Dictionary<AudioType, AudioPool>();
		foreach (AudioPool audioPool in pool)
		{
			audioPool.length = audioPool.clips.Length;
			audioPool.sources = new AudioSource[audioPool.length];
			for (int j = 0; j < audioPool.length; j++)
			{
				AudioClip audioClip = audioPool.clips[j];
				GameObject gameObject2 = new GameObject();
				gameObject2.name = audioClip.name;
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.AddComponent<AudioSource>();
				gameObject2.GetComponent<AudioSource>().clip = audioClip;
				audioPool.sources[j] = gameObject2.GetComponent<AudioSource>();
			}
			audioList[audioPool.type] = audioPool;
		}

		MusicVolumeChanged(PlayerPrefs.GetInt("musicvolume"));
		SfxVolumeChanged(PlayerPrefs.GetInt("sfxvolume"));
		
		StartMusic();
	}

	public void PlayFromPool(AudioType audioType)
	{
		audioList[audioType].play();
	}
	
	
	public void SfxVolumeChanged(int newVolume)
	{
		sfxVolume = newVolume;
		foreach (KeyValuePair<AudioType, AudioPool> keyValuePair in audioList)
		{
			keyValuePair.Value.setVolume(newVolume);
		}

		foreach (AudioSource item in gameAudioSources)
		{
			item.volume = newVolume;
		}
	}

	public void MusicVolumeChanged(float newVolume)
	{
		musicVolume = newVolume;
		audioMusic.volume = newVolume;
		
	}

	private void StartMusic()
	{
		//audioMusic.clip = musicSounds[Random.Range(0, musicSounds.Length)];
		if(PlayerPrefs.GetString("Mode") == "Campaign") 
			StartCoroutine(EnvironmentVoices());
	}
	

	public void ClickAudio(int index)
	{
		audioMusic.PlayOneShot(clickAudioClips[index]);
		audioMusic.Play();
	}

	private IEnumerator EnvironmentVoices()
	{
		yield return new WaitForSeconds(4f);
		audioMusic.PlayOneShot(campaignVoices[Random.Range(0, campaignVoices.Length)]);
		audioMusic.Play();
		StartCoroutine(EnvironmentVoices());
	}

}
