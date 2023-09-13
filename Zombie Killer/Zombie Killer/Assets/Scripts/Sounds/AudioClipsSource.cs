using UnityEngine;


public class AudioClipsSource : MonoBehaviour
{
    public static AudioClipsSource Instance;

    [Header("Music Clips")]
    public AudioClip MainMenuClip;
    public AudioClip GamePlayClip;
    public AudioClip GamePlayCipPolice;
    public AudioClip Particle;
    public AudioClip GenericButtonClick;
    public AudioClip LockButtonClick;
    public AudioClip LevelFailedClip;
    public AudioClip LevelSuccessClip;
    public AudioClip ButtonSound;
    public AudioClip PowerButton;
    public AudioClip DogSound;
    
    
    public AudioClip ImpactHit;


    [Header("Gameplay SFX")]
    public AudioClip zombieAttack;
    public AudioClip zombiePain;
    public AudioClip zombieDie;
    public AudioClip zombieIdle;
    public AudioClip playerDie;
    public AudioClip playerHurt;


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
