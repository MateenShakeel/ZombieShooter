using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    
    public float totalTime;
    public Text timer;
    private bool timerIsRunning ;
    
    
    private void Update()
    {
        if (!timerIsRunning)
        {
            if (totalTime >= 1)
            {
              totalTime -= Time.deltaTime;
              UpdateLevelTimer(totalTime);
            }
            else
            {
             timerIsRunning = true;
             TeamDeathManager.Instance.TimerStats();
            }
         
        }
    }

    public void UpdateLevelTimer(float totalSecond)
    {
        int minutes = Mathf.FloorToInt(totalSecond / 60f);
        int second = Mathf.FloorToInt(totalSecond % 60f);

        string formatedSecond = second.ToString();

        if (second == 60)
        {
            second = 0;
            minutes += 1;
        }


        timer.text = minutes.ToString("00") + ":" + second.ToString("00");
    }
}
