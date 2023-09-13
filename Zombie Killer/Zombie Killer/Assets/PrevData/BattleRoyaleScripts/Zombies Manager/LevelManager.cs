using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
    {

        #region GlobalInstance

        public static LevelManager localInstance;
        public static LevelManager Instance
        {
            get
            {
                localInstance = GameObject.FindObjectOfType<LevelManager>();
                return localInstance;
            }
        }
    
        #endregion

        #region Properties

        [Space(4)] [Header("UI Reference")]
        public GameObject[] moveableObjects;
        public GameObject[] enableObjects;
        [HideInInspector]public int currentLevel;
        public Text doubleRewardText;
        public Text levelRewardText;
        [Space(4)]
        [Header("Levels")]
        public GameObject[] levelsList;
        [Space(4)]
        [Header("Total Zombie in Level")]
        public int[] totalZombie;
        [Space(4)]
        [Header("Total Grenades in Level")]
        [Space(3)]public int[] grenades;        
        [Header("Level Complete Cash")]
        [Space(3)]public int[] levelsCash;
        [Space(4)]
        [Header("Player SpawnPosition and Angle")]
        [Space(2)]public Transform[] playerSpwanPosition;
        [Space(2)]public float[] playerRotationAngle;

        [Space(4)] [Header("Scripts Reference")]
        public GameManager gameManager;

        public GameObject zombieController;

        public Camera playerCam;
        
        [Header("Zombie")] 
        public GameObject ZombieModeStart;
        public int[] ZombiesHealth;

        #endregion

        #region Initilization
        
        private void Start()
        {
            
            if(PlayerPrefs.GetString("Mode") == "Zombie")
            {
                
                Debug.Log("Zombie");
                zombieController.SetActive(true);
                // playerCam.clearFlags = CameraClearFlags.SolidColor;
                // RenderSettings.fog = true;
                // RenderSettings.fogColor = Color.black;
                
                foreach (var item in moveableObjects)
                {
                    item.SetActive(false);
                } 
                foreach (var item in enableObjects)
                {
                    item.SetActive(true);
                }
                currentLevel = PlayerPrefs.GetInt("LevelZombie");
                
                for (int i = 0; i < levelsList.Length; i++) 
                { 
                    if (currentLevel == i+1) 
                    { 
                        levelsList[i].SetActive(true); 
                    } 
                }
                
//                gameManager.playerController.transform.position = playerSpwanPosition[currentLevel - 1].transform.position;
                
                playerrotate.Instance.rotationX = playerRotationAngle[currentLevel - 1]; 
                weaponselector.Instance.grenade = grenades[currentLevel - 1]; 
                StartCoroutine(ZombieModeStartHeading());
                
            }
            else
            {
                RenderSettings.fog = false;
                playerCam.clearFlags = CameraClearFlags.Skybox;
                Debug.Log("Campaign");
            }
        }

        private IEnumerator ZombieModeStartHeading()
        {
            yield return new WaitForSecondsRealtime(3f);
            ZombieModeStart.SetActive(true);
            yield return new WaitForSecondsRealtime(2f);
            ZombieModeStart.SetActive(false);
            
        }
        private int totalReward;
        public void SetFixedZombieLevelsCash()
        {
            PlayerPrefs.SetInt("Cash",PlayerPrefs.GetInt("Cash")+levelsCash[currentLevel-1]);
            totalReward = levelsCash[currentLevel - 1];
            levelRewardText.text = $"{totalReward}";
            doubleRewardText.text = $"{totalReward * 2}";
        }

        public void DoubleRewardZombie()
        {
            levelRewardText.text = $"{totalReward * 2}";
            PlayerPrefs.SetInt("Cash",PlayerPrefs.GetInt("Cash")+ (totalReward*2));
        }

        [HideInInspector]public bool isRewarded;
        public void GetDoubleReward()
        {
            isRewarded = true;
           // AdsController.Instance.ShowRewarded();
        }

        #endregion
        
    }
