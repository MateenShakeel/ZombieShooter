using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TeamDeathManager : MonoBehaviour
{

    #region singleton
    
    public static TeamDeathManager localinstance;
    public static TeamDeathManager Instance 
    {
        get
        {
            if (localinstance == null)
                localinstance = GameObject.FindObjectOfType<TeamDeathManager>();
            return localinstance;
        }
    }
    
    #endregion

    #region Properties

    [Header("TDM Player")]
    [Space(3)]public GameObject enemyTeam;
    [Space(3)]public GameObject playerTeam;

    [Space(3)]public Transform player;
    
    [Header("Spawn Points")]
    [Space(3)]public Transform[] playerSpawnPoints;
    [Space(3)]public Transform[] enemyTeamSpawnPoints;
    [Space(3)]public Transform[] playerTeamSpawnPoints;
    [Header("List Add")]
    [Space(3)]public List<Transform> playerTeamList;
    [Space(3)]public List<Transform> enemyTeamList;

    [Header("Death Counts")]

    [Space(3)]public static int enemyDeathCount;
    [Space(3)]public static int playerDeathCount;
    
    [Header("UI Panels")]
    [Space(3)]public GameObject[] reference;

    [Header("TDM Stats")]
    [Space(3)]public Text[] playerStats;
    [Space(3)]public Text[] enemyStats;
    
    // public Button inventory;
    [Space(3)]public Text[] matchReward;
    [Space(3)]public Text[] matchDoubleReward;
    

    [Space(3)]public GameObject playerRespawn;
    [HideInInspector]public bool isReload;

    [Header("Objects Enable Disbale")]
    [Space(3)]public GameObject[] controller;
    [Space(3)]public GameObject[] tDmEnable;


    #endregion

    #region Initilization

    private void Start()
    {
        
        Time.timeScale = 1;
        
        if (PlayerPrefs.GetString("Mode") != "MultiPlayer")
        {
            
            foreach (var items in tDmEnable)
            {
                items.SetActive(false);
            }
            
        }
        else
        {
            foreach (var items in controller)
            {
                items.SetActive(false);
            }
            foreach (var items in tDmEnable)
            {
                items.SetActive(true);
            }
            enemyDeathCount = playerDeathCount = 0;
            InitializeTeam();
//            GameManager.Instance.selectedMode.text = $"TEAM DEATH MATCH";


            
        }

    }

    private void InitializeTeam()
    {
        EnemyTeamSpawn();
        PlayerTeamSpawn();
        InitilizedPlayer();

    }

#endregion

#region TDM Region

    private void EnemyTeamSpawn()
    {
        for (int i = 0; i <  4; i++)
        {
            GameObject g =Instantiate(enemyTeam,enemyTeamSpawnPoints[i].position,enemyTeamSpawnPoints[i].rotation);
            enemyTeamList.Add(g.transform); 
        }
    }

    private void PlayerTeamSpawn()
    {
        for (int i = 0; i <  3; i++)
        {
            GameObject g =Instantiate(playerTeam,playerTeamSpawnPoints[i].position,playerTeamSpawnPoints[i].rotation);
            playerTeamList.Add(g.transform); 
        }
    }

    private void InitilizedPlayer()
    {
        int spawnIndex = Random.Range(0, playerSpawnPoints.Length);

        var transform1 = player.transform;
        transform1.position = playerSpawnPoints[spawnIndex].transform.position;
        transform1.rotation = playerSpawnPoints[spawnIndex].transform.rotation;

        if (PlayerPrefs.GetString("Mode") == "MultiPlayer")
        {
            player.GetComponent<playercontroller>().hitpoints = 10;
            player.GetComponent<playercontroller>().maxhitpoints = 10;
        }
        else
        {
            player.GetComponent<playercontroller>().hitpoints = 100;
            player.GetComponent<playercontroller>().maxhitpoints = 100;
        }
        isplayerDead = false;
    }

    private void PlayerSpawn()
    {
        int spawnIndex = Random.Range(0, playerSpawnPoints.Length);
        Instantiate(player, playerSpawnPoints[spawnIndex].transform.position,Quaternion.identity);
    }
    
    public void PlayerTeamDead(Transform x)
    {
        
        playerDeathCount++;
        Stats();
        Debug.Log("Player Death Count "+playerDeathCount);
        int y=playerTeamList.IndexOf(x);
        playerTeamList.RemoveAt(y);
        SoundController.instance.PlayFromPool(AudioType.TdmVoice);
        
        if (playerTeamList.Count < 3)
        {
            int i = Random.Range(0, playerTeamSpawnPoints.Length);
            GameObject g =Instantiate(playerTeam,playerTeamSpawnPoints[i].position,playerTeamSpawnPoints[i].rotation);
            playerTeamList.Add(g.transform); 
        }
        
    }
    
    public void EnemyTeamDead(Transform x)
    {
        enemyDeathCount++;
        Stats();
        
        int y=enemyTeamList.IndexOf(x);
        enemyTeamList.RemoveAt(y);
        SoundController.instance.PlayFromPool(AudioType.TdmVoice);
        
        Invoke(nameof(EnemyTeamSpawnByTime),3f);
    }

    private void EnemyTeamSpawnByTime()
    {
        if (enemyTeamList.Count < 4)
        {
            int i = Random.Range(0, enemyTeamSpawnPoints.Length);

            GameObject g =Instantiate(enemyTeam,enemyTeamSpawnPoints[i].position,enemyTeamSpawnPoints[i].rotation);
            enemyTeamList.Add(g.transform); 
        }
    }

    private bool isplayerDead = false;
    public void PlayerDead()
    {
        if (!isplayerDead && !InGameProperties.Instance.isReloadTime)
        {

         isplayerDead = true;
         playerDeathCount++;
         StartCoroutine(Respawning());
         SoundController.instance.PlayFromPool(AudioType.TdmVoice);
         
         Stats();
        }
    }

    private IEnumerator Respawning()
    {
        playerRespawn.SetActive(true);
        weaponselector.Instance.SwitchWeapon(8);
//        GameManager.Instance.reference[1].SetActive(false);
        yield return new WaitForSeconds(3);
        weaponselector.Instance.SwitchWeapon(PlayerPrefs.GetInt("Weapon"));
        InitilizedPlayer();
        playerRespawn.SetActive(false);
        //GameManager.Instance.EnableReference(0);
    }
    

#endregion
    
#region State Mathod

    private void EnableReference(int index)
    {
        foreach (var item in reference)
        {
            item.SetActive(false);
        }
        reference[index].SetActive(true);
        
    }
    

    public void Continue()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Resume()
    {
        Time.timeScale = 1;
//        GameManager.Instance.EnableReference(0);
        EnableIndicator();
    }
    
    public void LeftMatch()
    {
        SceneManager.LoadScene(1);
    }

    public void DisableIndicator()
    {
        // foreach (var item in playerTeamList)
        // {
        //     item.GetComponent<IndicatorTarget>().enabled = false;
        // }
        foreach (var item in enemyTeamList)
        {
            item.GetComponent<IndicatorTarget>().enabled = false;
        }
    }
    
    public void EnableIndicator()
    {
        // foreach (var item in playerTeamList)
        // {
        //     item.GetComponent<IndicatorTarget>().enabled = true;
        // }
        foreach (var item in enemyTeamList)
        {
            item.GetComponent<IndicatorTarget>().enabled = true;
        }
    }

#endregion

#region Results!

    private IEnumerator Victory()
    {
        
        yield return new WaitForSecondsRealtime(0f);
//        GameManager.Instance.winEffect.SetActive(true);
        SoundController.instance.PlayFromPool(AudioType.LevelCompleteAudio);
        yield return new WaitForSecondsRealtime(3.5f);
       // GameManager.Instance.EnableReference(6);
        //GameManager.Instance.winEffect.SetActive(false);

        matchReward[0].text = $"{750}";
        matchDoubleReward[0].text = $"{1500}";
        
        PlayerPrefs.SetInt("Cash",PlayerPrefs.GetInt("Cash")+750);

        Time.timeScale = 0;
        
    }

    private IEnumerator Defeat()
    {
        
        yield return new WaitForSecondsRealtime(0f);
       // GameManager.Instance.defeatEffect.SetActive(true);
       // SoundController.instance.PlayFromPool(AudioType.LevelFailAudio);
        yield return new WaitForSecondsRealtime(2f);
       // GameManager.Instance.EnableReference(7);
       // GameManager.Instance.defeatEffect.SetActive(false);
        
        matchReward[1].text = $"{750}";
        matchDoubleReward[1].text = $"{1500}";

        Time.timeScale = 0;
       
    }

#endregion

#region Stats

    private void Stats()
    {
        
        if (playerDeathCount == 31)
        {
            StartCoroutine(Defeat());
            enemyStats[0].text = enemyStats[1].text = enemyStats[2].text = 30.ToString();
        }
        else if(enemyDeathCount == 30)
        {
            StartCoroutine(Victory());
            playerStats[0].text = playerStats[1].text = playerStats[2].text = 30.ToString();
        }
        
        playerStats[0].text =playerStats[1].text =playerStats[2].text = enemyDeathCount <= 30 ? enemyDeathCount.ToString() : 30.ToString();
        enemyStats[0].text=enemyStats[1].text=enemyStats[2].text = playerDeathCount <= 30 ? playerDeathCount.ToString() : 30.ToString();
    }
    
    public void TimerStats()
    {
        playerStats[0].text = playerStats[1].text = playerStats[2].text = enemyDeathCount.ToString(); 
        enemyStats[0].text = enemyStats[1].text = enemyStats[2].text = playerDeathCount.ToString();
        
        if (playerDeathCount > enemyDeathCount)
        {
            StartCoroutine(Defeat());
        }
        else
        {
            StartCoroutine(Victory());
        }
    }
    
#endregion

    public void DoubleTDM_Reward()
    {
        matchReward[0].text = matchReward[1].text = $"{1500}";
        PlayerPrefs.SetInt("Cash",PlayerPrefs.GetInt("Cash")+1500);
    }
    
    [HideInInspector]public bool tdmReward;
    public void RewardedVideo()
    {
        tdmReward = true;
       // AdsController.Instance.ShowRewarded();
    }
    
}
