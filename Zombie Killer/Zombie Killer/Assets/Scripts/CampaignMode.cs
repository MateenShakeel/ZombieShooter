using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CampaignMode : MonoBehaviour
{
    public GameObject[] Spawners;
    public GameObject[] Walls;
    public GameObject[] Tracks;
    public float waveTime;
    public Text timeText;
    public GameObject objectivePanel;

    bool canCount = true;


    float timer;

    int index = 0;

    private void Start()
    {
        timer = waveTime;
        timeText.transform.parent.gameObject.SetActive(true);
        Spawners[index].SetActive(true);
        Walls[index].SetActive(true);

    }

    private void Update()
    {
        if(canCount)
        {
            timer -=  Time.deltaTime;
            timeText.text = Mathf.RoundToInt(timer).ToString() + "s";
            
            if(timer <= 0)
            {
                timer = 0;
                StartCoroutine(StartNextPoint());
            }
        }
        
    }

    IEnumerator StartNextPoint()
    {

        canCount = false;
        timeText.text = "";
        Walls[index].SetActive(false);
        Spawners[index].SetActive(false);
        Tracks[index].SetActive(true);
        objectivePanel.SetActive(true);
        index++;

        if (index == Walls.Length)
            index = 0;


        yield return new WaitForSeconds(30f);

        objectivePanel.SetActive(false);
        Spawners[index].SetActive(true);
        Walls[index].SetActive(true);
        Tracks[index].SetActive(false);

        timer = waveTime;
        canCount = true;
    }

}
