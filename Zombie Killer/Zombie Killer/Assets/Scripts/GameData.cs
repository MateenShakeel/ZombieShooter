using UnityEngine;
[CreateAssetMenu(fileName = "GameData", menuName = "Settings/GameData", order = 1)]
public class GameData : ScriptableObject
{
    #region NOT TO CHANGE
    [Header("DEFAULTS")]
    public float moveSpeed ;
    public float touchSens;
    public bool settingsChange;
    public float weapons;
    public bool autoShoot;
    #endregion

    [Header("Persistent Data")]

    public int levelsUnlocked;
    public bool firstTime;

    public Weapons[] Weapons;
   

    
}

[System.Serializable]
public class Weapons
{
    public string name;
    public bool isUnlocked;
    public int price;
   
}

