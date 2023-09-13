using UnityEngine;
using UnityEngine.UI;

public class AudioManager : AudioClipsSource {
	
	public static AudioManager Instance;   

	public Sprite SoundBtnEnabledImage;
	public Sprite SoundBtnDisabledImage;
	public Sprite MusicBtnEnabledImage;
	public Sprite MusicBtnDisabledImage;
    

	[HideInInspector]
	public AudioSource SfxSource;
	[HideInInspector]
	public AudioSource SoundSource;

    public static bool IsSoundOn = true;
    public static bool IsMusicOn = true;



    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }



    public void PlaySingleBg(AudioClip clip , float v , bool loop )
	{
		if (IsMusicOn) {
			SoundSource = CheckNullAudioSource (SoundSource , false);
			setAudioPref (SoundSource, clip , loop , v);
			//Play the clip.
			if (SoundSource.time != 0f) {
				SoundSource.UnPause ();
			} else {
				SoundSource.Play ();
			}

		} else {
			SoundSource = CheckNullAudioSource (SoundSource , false);
			setAudioPref (SoundSource, clip , true , v);
		}
	}

	public void PlaySingleSfx(AudioClip clip  , float v)
	{
		if (IsSoundOn) {
            SfxSource = CheckNullAudioSource (SfxSource , false);
			setAudioPref (SfxSource, clip , false , v);
			//Play the clip.
			if (!SfxSource.isPlaying) {
                SfxSource.Play ();
			}
		} else {
            SfxSource = CheckNullAudioSource (SfxSource , false);
			setAudioPref (SfxSource, clip , true , v);
		}
	}
	
	void setAudioPref(AudioSource source , AudioClip clip , bool loop , float volume){
		source.name = clip.name;
		source.playOnAwake = true;
		source.loop = loop;
		source.volume = volume;
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		source.clip = clip;

	}

	public void PlayOnShot(AudioClip clip , float v , AudioSource source)
	{
		if(IsSoundOn){
            source = CheckNullAudioSource (source , false);
			if(!source.isPlaying)
                source.Play ();
		}

	}
    


	public void StopSingle(){

		if(SoundSource != null){
			if (SoundSource.isPlaying) {
				SoundSource.Pause ();
			} 
		}
	}
	public void StopSFx(){

		if(SfxSource != null){
		    SfxSource.Stop ();
		}
	}
	public void StopExtraSFx(AudioSource source){

		if(source != null){
			source.Stop ();
		}
	}

	public void ResumeBgSound(){
		if(IsSoundOn){
			if(SoundSource != null){

			    if (!SoundSource.isPlaying)
			    {
			        SoundSource.Play();
			    }
            }
		}

	}
	
	AudioSource CheckNullAudioSource(AudioSource audio , bool is3D){

		if (audio == null) {
			GameObject audiosource = new GameObject ();
			AudioSource s = audiosource.AddComponent<AudioSource> ();
		    if (is3D)
		    {
		        s.spatialBlend = 1;
            }
            audiosource.transform.parent = transform;
			return s;
		}

		return audio;
	}



	public void SoundToggling(Button button)
	{
		Sprite image;
		if (!IsSoundOn) {
			image = SoundBtnEnabledImage;
			IsSoundOn = true;
		} else {
			image = SoundBtnDisabledImage;
			IsSoundOn = false;

		}
		button.GetComponent<Image>().sprite = image;
	}

	public void MusicToggling(Button button)
	{
		Sprite image;
		if (IsMusicOn) {
			image = MusicBtnDisabledImage;
            IsMusicOn = false;
            StopSingle ();
		} else {
			image = MusicBtnEnabledImage;
			IsMusicOn = true;
			if (SoundSource != null) {
				if (!SoundSource.isPlaying) {
					SoundSource.Play ();
				}
			}			
		}
		button.GetComponent<Image>().sprite = image;
	}

  

}