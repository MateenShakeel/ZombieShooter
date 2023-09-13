/// <summary>
/// SoundManager.cs 
/// version 1.0
/// Sound Manager to handle Background Music and effects.
/// By Awais Mazhar
/// </summary>
///---------------
/*
	 Copyright (c) 2016 Awais Mazhar

Permission is hereby granted, free of charge, 
to any person obtaining a copy of this software and associated documentation files (the "Software"), 
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, 
publish, distribute, sublicense, and/or sell copies of the Software, 
and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using UnityEngine;
using System.Collections;



public class SoundManager : SingletonSounds<SoundManager> {

	const string PlayerPrefs_KEY = "sounds_settings_key";
	public AudioClip _defaultBGClip = null;

	private bool _soundEnabled = true;
	private bool _effectEnabled = true;
	private bool _musicEnabled = true;
	private AudioSource _BGAudioSource;
	private AudioSource _FGAudioSource;
	private float _effectsVolume = 0;
	private float _musicVolume = 0;


    void Awake()
    {
        //Debug.Log(" SoundManager booting up ");
        // add requred components
        _FGAudioSource = gameObject.AddComponent<AudioSource>();
        _FGAudioSource.name = "(AudioSource)FG";
        _BGAudioSource = gameObject.AddComponent<AudioSource>();
        _BGAudioSource.name = "(AudioSource)BG";
        // load Persistent Settings
        //@TODO: persistence by object serialization
        _soundEnabled = bool.Parse(PlayerPrefs.GetString(PlayerPrefs_KEY, _soundEnabled.ToString()));
    }

    public bool IsEffectsPlaying()
    {
        if (_FGAudioSource.isPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EnableSounds {
		set { 
			_soundEnabled = value;
			_BGAudioSource.enabled = _soundEnabled;
			_FGAudioSource.enabled = _soundEnabled;
		}
		get { 
			return _soundEnabled;
		}
	}

	public bool MusicEnabled {
		set { 
			_musicEnabled = value;
			_BGAudioSource.enabled = _musicEnabled;
//			_FGAudioSource.enabled = _soundEnabled;
		}
		get { 
			return _soundEnabled;
		}
	}
	public bool EffectsEnabled {
		set { 
			_effectEnabled = value;
//			_BGAudioSource.enabled = _soundEnabled;
			_FGAudioSource.enabled = _effectEnabled;
		}
		get { 
			return _soundEnabled;
		}
	}

	public float EffectsVolume {
		set { 
			_effectsVolume = value;
			if (_effectsVolume < 0) {
				_effectsVolume = 0;
			}
			_FGAudioSource.volume = Mathf.Clamp01 (_effectsVolume);
		}
	}
	public float MusicVolume {
		set { 
			_musicVolume = value;
			if (_musicVolume < 0) {
				_musicVolume = 0;
			}
			_BGAudioSource.volume = Mathf.Clamp01 (_musicVolume);
		}
	}

	public AudioClip DefaultBGClip {
		set { 
			_defaultBGClip = value;
		}
		get { 
			return _defaultBGClip;
		}
	}
	

	public void Play ()
	{
		if (_defaultBGClip ==null) {
			Debug.LogWarning ("Default Audio Clip is required for SoundManager ");
			return;
		}
		PlayBackgroundMusic (_defaultBGClip);
	}


	public void PlayEffect (AudioClip _clip)
	{
		if (_effectEnabled & _clip != null) {
			_FGAudioSource.PlayOneShot (_clip);
		}
	}

	public void PlayVocal (AudioClip _clip)
	{
		if (_soundEnabled & _clip != null) {
			_FGAudioSource.Stop ();
			_FGAudioSource.clip = _clip;
			_FGAudioSource.Play ();
		}

	}
	public void PlayEffect(AudioClip _clip , bool status){
		_FGAudioSource.clip = _clip;
		_FGAudioSource.loop = status;
		_FGAudioSource.Play ();
	}
	public void StopEffect(AudioClip _clip){
		_FGAudioSource.Stop ();

	}

    public void Stop()
    {
        EnableSounds = false;
    }

	public void PlayBackgroundMusic (AudioClip _clip)
	{
		if (_musicEnabled && _clip != null) {
			_BGAudioSource.clip = _clip;
			_BGAudioSource.loop = true;
			_BGAudioSource.Play ();
		}
	}
	public void StopBackgroundMusic(AudioClip _clip)
	{
		if (_musicEnabled && _clip != null)
		{
			_BGAudioSource.clip = _clip;
			_BGAudioSource.loop = true;
			_BGAudioSource.Stop();
		}
	}
	public void Pause ()
	{
		EnableSounds = false;
	}

	public void Resume ()
	{
		EnableSounds = true;
	}

	void PersistSetting ()
	{
		PlayerPrefs.SetString (PlayerPrefs_KEY, _soundEnabled.ToString ());
		PlayerPrefs.Save ();
	}

}
