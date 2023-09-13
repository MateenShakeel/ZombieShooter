using System;
using System.Collections;
using System.Collections.Generic;
using TacticalAI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelsController : MonoBehaviour
{
    
    #region Global Reference

    private static LevelsController _localInstance;
    public static LevelsController Instance 
    {
        get
        {
            if (_localInstance == null)
                _localInstance = GameObject.FindObjectOfType<LevelsController>();
            return _localInstance;
        }
    }
    #endregion
    
    [Space(10)]
    // public Text[] missionObjective;
    public GameObject[] enemyPrefab;
    [Header("-----------")] [Space(5)]
    public int[] levelReward;
    [Header("-----------")][Space(10)]
    public Enemy[] levelData;
    [HideInInspector]public int currentLevel;
    public Text totalCoins;
    
    private void Start()
    {
        if (PlayerPrefs.GetString("Mode") == "Campaign")
        {
//            currentLevel = GameManager.Instance.currentLevel - 1;
            for (int i = 0; i < levelData[currentLevel].enemiesType.Count; i++)
            {
                GameObject g = Instantiate(enemyPrefab[CheckEnemyType(i)],
                    levelData[currentLevel].enemiesPosition[i].transform.position,
                    levelData[currentLevel].enemiesPosition[i].transform.rotation);
                levelData[currentLevel].totalEnemies.Add(g.transform);
            }

            for (int i = 0; i < levelData[currentLevel].enemiesWave; i++)
            {
                levelData[currentLevel].totalEnemies[i].gameObject.SetActive(true);
            }
            SoundController.instance.PlayFromPool(AudioType.GameStartAudio);

            totalCoins.text = $"{PlayerPrefs.GetInt("Cash")}";
        }
        // SetObjective();
        
    }

    // private void SetObjective()
    // {
    //
    //     missionObjective[2].text = $"COMPLETE YOUR MISSION";
    //     missionObjective[1].text = levelData[currentLevel].objectives1 == ObjectiveType.Accuracy ? $"COMPLETE MISSION WITH {levelData[currentLevel].obj1}% ACCURACY" : $"COMPLETE MISSION WITH MORE THAN {levelData[currentLevel].obj1}% HEALTH";
    //     
    //     if (levelData[currentLevel].objectives2 == ObjectiveType.HeadShot)
    //     {
    //         missionObjective[0].text = $"EXECUTE ATLEAST {levelData[currentLevel].obj2} HEADSHOTS";
    //         Debug.Log("Objectives");
    //     }
    //     else
    //     {
    //         missionObjective[0].text = $"HIT ATLEAST {levelData[currentLevel].obj2} BODYSHOTS";
    //     }
    //     
    //     
    // }

    public void NextEnemy()
    {
        if (levelData[currentLevel].totalEnemies.Count > 0)
        {
            
            levelData[currentLevel].totalEnemies[0].gameObject.SetActive(true);
            levelData[currentLevel].totalEnemies[0].GetComponent<HealthScript>().isHealth = false;
            levelData[currentLevel].totalEnemies[0].GetComponent<HealthScript>().isWeapons = false;
            levelData[currentLevel].totalEnemies[0].GetComponent<HealthScript>().isGrenade = false;
            
        }
        else
        {
//            GameManager.Instance.GameComplete();
        }
    }

    public void EnemiesInList()
    {
        foreach (var item in levelData[currentLevel].totalEnemies)
        {
            item.GetComponent<HealthScript>().EnemyMeshRender.GetComponent<Outline>().enabled = false; 
        }
    } 
    
    public void EnemiesIndicatorDisable()
    {
        foreach (var item in levelData[currentLevel].totalEnemies)
        {
            item.GetComponent<IndicatorTarget>().enabled = false;
        }
    }
    
    public void EnemiesIndicatorEnable()
    {
        foreach (var item in levelData[currentLevel].totalEnemies)
        {
            item.GetComponent<IndicatorTarget>().enabled = true;
        }
    }

    private int x;
    private int CheckEnemyType(int i )
    {
        if (levelData[currentLevel].enemiesType[i] == EnemyType.A)
        {
                x= 0;
                return x;
        }
        if (levelData[currentLevel].enemiesType[i] == EnemyType.B)
        {
                x= 1;
                return x;
        }
        if (levelData[currentLevel].enemiesType[i] == EnemyType.C)
        {
                x= 2;
                return x;
        }
        if (levelData[currentLevel].enemiesType[i] == EnemyType.D)
        {
                x= 3;
                return x;
        }
        return x;
    }
}

[Serializable]
 public class Enemy
 {
     [Space(5)]
     [Space(2)]public Transform playerSpwanPosition;
     [Space(2)]public int enemiesWave;

     [Space(2)]public List<EnemyType> enemiesType;
     
     [Space(2)]public List<Transform > enemiesPosition;
     [Space(2)]public List<Transform> totalEnemies;

     [Space(3)]public ObjectiveType objectives1,objectives2;
     [Space(3)]public int obj1, obj2;

 }
 
[Serializable]
public enum EnemyType
{
    A,B,C,D
}

[Serializable]
public enum ObjectiveType
{
  Health,Accuracy,BodyShot,HeadShot
}

