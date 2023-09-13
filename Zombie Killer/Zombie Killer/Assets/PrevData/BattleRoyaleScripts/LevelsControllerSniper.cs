using System;
using System.Collections;
using System.Collections.Generic;
using TacticalAI;
using UnityEngine;

public class LevelsControllerSniper : MonoBehaviour
{
    
    #region singleton
    public static LevelsControllerSniper localinstance;
    public static LevelsControllerSniper Instance 
    {
        get
        {
            if (localinstance == null)
                localinstance = GameObject.FindObjectOfType<LevelsControllerSniper>();
            return localinstance;
        }
    }
    #endregion
    
    public GameObject[] enemies;
    public Enemies[] levelManager;
    [HideInInspector]public int currentLevel;

    public GameObject[] moveableObject;
    public GameObject[] enableObjects;

    public int[] missionReward;

    private void Start()
    {
        
        if (PlayerPrefs.GetString("Mode") == "Sniper")
        {
            currentLevel = PlayerPrefs.GetInt("LevelSniper")-1;
            foreach (var item in moveableObject)
            {
                item.SetActive(false);
            }
            foreach (var item in enableObjects)
            {
                item.SetActive(true);
            }

            for (int i = 0; i <  levelManager[currentLevel].enemiesType.Count; i++)
            {
             GameObject g =Instantiate(enemies[CheckEnemyType(i)],levelManager[currentLevel].enemiesPosition[i].transform.position,levelManager[currentLevel].enemiesPosition[i].transform.rotation);
             levelManager[currentLevel].totalEnemies.Add(g.transform); 
            }

            for (int i = 0; i < levelManager[currentLevel].enemiesWave; i++)
            {
             levelManager[currentLevel].totalEnemies[i].gameObject.SetActive(true);
            }
        }

    }
    
    public void NextEnemy()
    {
        if (levelManager[currentLevel].totalEnemies.Count > 0)
        {
            if (levelManager[currentLevel].totalEnemies.Count == 1)
            {
                levelManager[currentLevel].totalEnemies[0].gameObject.SetActive(true);
            }
            else
            {
                levelManager[currentLevel].totalEnemies[0].gameObject.SetActive(true);
            }
           
        } else
        {
//            GameManager.Instance.GameComplete();
        }
    }
    
    public void EnemiesInList()
    {
        foreach (var item in levelManager[currentLevel].totalEnemies)
        {
            item.GetComponent<HealthScript>().EnemyMeshRender.GetComponent<Outline>().enabled = false; 
        }
    }
    
    public void EnemiesIndicatorDisable()
    {
        foreach (var item in levelManager[currentLevel].totalEnemies)
        {
            item.GetComponent<IndicatorTarget>().enabled = false;
        }
    }
    
    public void EnemiesIndicatorEnable()
    {
        foreach (var item in levelManager[currentLevel].totalEnemies)
        {
            item.GetComponent<IndicatorTarget>().enabled = true;
        }
    }

    private int x;
    public int CheckEnemyType(int i )
    {
        if (levelManager[currentLevel].enemiesType[i] == EnemyTypes.A)
        {
                x= 0;
                return x;
        }
        if (levelManager[currentLevel].enemiesType[i] == EnemyTypes.B)
        {
                x= 1;
                return x;

        }
        return x;
    }
}

[Serializable]
 public class Enemies
 {
     [Space(5)]
     [Space(2)]public Transform playerSpwanPosition;
     [Space(2)]public float playerRotationAngle;
     [Space(2)]public int enemiesWave;
     public int grenades;

     [Space(2)]public List<EnemyTypes> enemiesType;
     
     [Space(2)]public List<Transform > enemiesPosition;
     [Space(2)]public List<Transform> totalEnemies;
     
 }
 
    [Serializable]
    public enum EnemyTypes
    {
       A,B
    }

