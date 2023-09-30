using UnityEngine;


public class AudioClipsSource : MonoBehaviour
{
    public static AudioClipsSource Instance;

    [Header("Music Clips")]
    public AudioClip MainMenuClip;
    public AudioClip[] GamePlayClips;

    public AudioClip GenericButtonClick;
    public AudioClip LockButtonClick;
    public AudioClip LevelFailedClip;
    public AudioClip LevelSuccessClip;

    public AudioClip ZombiesSpawningClip;
    public AudioClip playerDeath;

    [Header("Gun Sounds")]
    public AudioClip[] GunSounds;
    


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
