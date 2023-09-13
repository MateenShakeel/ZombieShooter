using UnityEngine.Analytics;
using UnityEngine;

public class Analyticsmanager : MonoBehaviour
{
    public static Analyticsmanager instance;
    [HideInInspector]public int iapWeaponIndex= 0;
    public bool showDebugs;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            DestroyImmediate(this.gameObject);
    }


    public void LevelStartEvent(string modename , int levelNumber)
    {
        //AnalyticsEvent.LevelStart(modename+levelNumber);
        
        if(showDebugs)
            Debug.Log("<color=green> Analytic Start </color>"+ modename + levelNumber);
        
    }

    public void LevelCompleteEvent(string modename,int levelNumber)
    {
        //AnalyticsEvent.LevelComplete(modename+levelNumber);
        
        if (showDebugs)
            Debug.Log("<color=green> Analytic Complete </color>" + modename + levelNumber );
        
    }

    public void LevelFailedEvent(string modename,int levelNumber)
    {
        Analytics.CustomEvent("F"+modename+levelNumber);
        
        if (showDebugs)
            Debug.Log("<color=green> Analytic Failed </color>" + modename+levelNumber);

    }
    
    public void CustomEvent(string combination)
    {
        Analytics.CustomEvent(combination);
        
        if(showDebugs)
         Debug.Log ("<color=green> Analytic </color>" + combination);
        
    }

    public void GunsTrackingMenu(int index)
    {
        
        Analytics.CustomEvent("MW" + index);
        
        if(showDebugs)
            Debug.Log ("<color=green> Analytics Menu Weapon </color>" + index);
        
    }
    
    public void GunsTrackingGame(int index)
    {
        Analytics.CustomEvent("GW" + index);
        
        if(showDebugs)
            Debug.Log ("<color=green> Analytics Game Weapon </color> " + index);
        
    }

    public void ExitUtm(int index)
    {
        Analytics.CustomEvent("Exit" + index);
        
        if(showDebugs)
            Debug.Log ("<color=green>Exit UTM Exit </color>" + index);
    } 
    
    public void SettingUtm(int index)
    {
        Analytics.CustomEvent("Setting" + index);
        
        if(showDebugs)
            Debug.Log ("<color=green>Setting UTM Setting </color>" + index);
        
    } 
    
    public void UtmTracking(string location)
    {
        Analytics.CustomEvent(location);
        
        if(showDebugs)
            Debug.Log ("<color=green> InGame UTM </color>" + location);
        
    }   
    
    public void ModeNameEvent(string modeName)
    {
       // AnalyticsEvent.Custom(modeName);
        
        if(showDebugs)
           Debug.Log ("<color=green> Mode Analytic </color>" + modeName);
        
    }
    
}
