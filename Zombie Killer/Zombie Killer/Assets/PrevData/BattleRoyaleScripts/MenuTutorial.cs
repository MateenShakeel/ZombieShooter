using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTutorial : MonoBehaviour
{
    [Header("Step One Heading")]
    [Space]
    public UnityEngine.Events.UnityEvent FirstStep;
    
//    [Header("Step Two Heading")]
//    [Space]
//    public UnityEngine.Events.UnityEvent SecondStep;
//    [Header("Step Three Heading")]
//    [Space]
//    public UnityEngine.Events.UnityEvent ThirdStep;
    
    void Start()
    {
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            FirstStep.Invoke();
            Debug.Log("FirstStep");
        }
        
//        else if(PlayerPrefs.HasKey("TutorialFirst") && !PlayerPrefs.HasKey("TutorialSecond"))
//        {
//            SecondStep.Invoke();
//            Debug.Log("SecondStep");
//        }
//        else
//        {
//            ThirdStep.Invoke();
//            Debug.Log("ThirdStep");
//        }
        
    }

    public void TutorialOne()
    {
        PlayerPrefs.SetString("Tutorial","Done");
    }


}
