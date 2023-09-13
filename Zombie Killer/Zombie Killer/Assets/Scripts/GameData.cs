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
    public int cash;
    public int gold;
    public bool musicEnabled;
    public bool soundEnabled;
    public int levelsUnlocked;
    public bool firstTime;
    public bool IsTutorial;
    public Weapons[] Weapons;
    public Skins[] Skins;

    [Header("InApps Check")]
    public bool isUnlockEverything;
    public bool isUnlockGuns;
    public bool RemoveAds;
}

[System.Serializable]
public class Weapons
{
    public string name;
    public bool isUnlocked;
    public int price;
    public float fireRate;
    public float accuracy;
    public float damage;
    public float reload;
}

[System.Serializable]
public class Skins
{
    public bool isUnlocked;
    public int price;

}
