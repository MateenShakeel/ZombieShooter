using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyKillCounter : MonoBehaviour
{
    public static EnemyKillCounter Instance;
    

    [SerializeField] Text _killText;
    int _currentKills;
    int _totalKills;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        _currentKills = 0;

        var level = GameplayHandler.Instance.GetCurrentCampaignLevel();
        _totalKills = level.Kills;
        _killText.gameObject.SetActive(true);
        _killText.text = "Kills : " + _currentKills.ToString() + "/" + _totalKills.ToString();
    }

    public void IncrementKill()
    {
        _currentKills++;
        _killText.text = "Kills : " + _currentKills.ToString() + "/" + _totalKills.ToString();
    }
}
