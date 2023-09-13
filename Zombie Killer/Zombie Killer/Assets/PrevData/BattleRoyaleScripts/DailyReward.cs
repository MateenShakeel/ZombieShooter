using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    
    
    #region Global Refrence
    
    public static DailyReward localInstance;
    public static DailyReward Instance
    {
        get
        {
            if (localInstance == null)
                localInstance = GameObject.FindObjectOfType<DailyReward>();
            return localInstance;
        }
    }
    
    #endregion

    #region Properties

    public Button[] rewardButton;
    public GameObject[] LockButton,SelectedButton,Collected;

    public Week[] Weeks;
    
    public Image[] dailyImage;
    
    public Text[] dailyReward;

    public GameObject Debug,ClaimBtn,RewardedClaimBtn;
    
    public Text Hrs,Min,Sec;
    
    public bool time , advanceDebug;

    [HideInInspector]
    public bool isRewardReady = false;

    [HideInInspector] public bool dailyRewarded;
    
    private int nextRewardIndex;
    
    //wait 23 Hours to activate the next reward (it's better to use 23h instead of 24h)
    private double nextRewardDelay = 23f;

    public Color selectedColor;
    
    //check if reward is available every 5 seconds
    private readonly float checkForRewardDelay = 1f;

    #endregion
    
    private void Awake()
    {
        Init();
        StartCoroutine(CheckForRewards());
    }

    #region Initialization
    
    public void Init()
    {
        nextRewardIndex = PlayerPrefs.GetInt ( "Next_Reward_Index", 0 );
        
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("Reward_Claim_Datetime")))
        {
            if (!time)
            {
                PlayerPrefs.SetString("Reward_Claim_Datetime", DateTime.Now.ToString());
            }
            ActivateReward();
        }
        
        for (int i = 0; i < Weeks[0].Rewarditems.Length; i++)
        {
            if (PlayerPrefs.GetInt("Week") == 0)
            {
             dailyImage[i].sprite = Weeks[0].Rewarditems[i].rewardicons;
             

            dailyReward[i].text = Weeks[0].Rewarditems[i].rewardName;
            }
            else
            {
                dailyImage[i].sprite = Weeks[1].Rewarditems[i].rewardicons;
                dailyReward[i].text = Weeks[1].Rewarditems[i].rewardName;
            }
            
            if (PlayerPrefs.HasKey("ReadyForClaim" + i))
            {
                rewardButton[i].interactable = true;
                SelectedButton[i].GetComponent<Image>().color = selectedColor;
               // rewardButton[i].GetComponent<Image>().sprite = Ready;
               SelectedButton[nextRewardIndex].SetActive(true);
               LockButton[nextRewardIndex].SetActive(false);
               // SelectedButton[nextRewardIndex].GetComponent<Button>().interactable=false;
               // SelectedButton[nextRewardIndex].GetComponent<Image>().enabled=false;


            }
            else
            {
         
                rewardButton[i].interactable = false;
                //rewardButton[i].GetComponent<Image>().sprite = Unclaimed;
                SelectedButton[nextRewardIndex].SetActive(true);
                SelectedButton[i].SetActive(false);
                SelectedButton[i].GetComponent<Button>().interactable = false;
                LockButton[nextRewardIndex].SetActive(false);
                // SelectedButton[nextRewardIndex].GetComponent<Button>().interactable=true;
                // SelectedButton[nextRewardIndex].GetComponent<Image>().enabled=true;

                Collected[i].SetActive(false);
            }
            
            if (PlayerPrefs.HasKey("Claimed" + i))
            {
            
                rewardButton[i].interactable = false;
                
              //  rewardButton[i].GetComponent<Image>().sprite = Claimed;
              
              Collected[i].SetActive(true);
              SelectedButton[i].SetActive(false);
              LockButton[i].SetActive(false);
              // SelectedButton[nextRewardIndex].GetComponent<Button>().interactable=false;
              // SelectedButton[nextRewardIndex].GetComponent<Image>().enabled=true;

            }


            if (nextRewardIndex==0)
            {
                // rewardButton[nextRewardIndex].GetComponent<Image>().sprite = Ready;
                LockButton[nextRewardIndex].SetActive(false);
                // SelectedButton[nextRewardIndex].GetComponent<Button>().interactable=false;
                Collected[i].SetActive(false);
                // SelectedButton[nextRewardIndex].GetComponent<Image>().enabled=false;

            }
          
       
            float x;
            if (nextRewardIndex > 11)
            {
                x = 1f;
            }
            else
            {
                x= nextRewardIndex / 14f;
            }
        }
        
        
        if (time)
        {
            StartCoroutine(CheckForRewards());
        }
        else
        {
            checkTime();
        }

        if (advanceDebug)
        {
            Debug.SetActive(true);
        }
        else
        {
            Debug.SetActive(false);
        }

    }
    #endregion
    
    #region WithTime

    IEnumerator CheckForRewards()
    {
        while (true)
        {
            if (!isRewardReady)
            {
                DateTime currentDatetime = DateTime.Now;
                DateTime rewardClaimDatetime = DateTime.Parse(PlayerPrefs.GetString("Reward_Claim_Datetime", currentDatetime.ToString()));
            
                //get total Hours between this 2 dates
                double elapsedHours = (currentDatetime - rewardClaimDatetime).TotalHours;

                if (elapsedHours >= nextRewardDelay)
                {
                    Hrs.text = "0";
                    Sec.text = "0";
                    Min.text ="0"; 
                    ActivateReward();
                }
                else
                {
                
                    DeactivateReward();
                }
            }

            yield return new WaitForSeconds(checkForRewardDelay);
        }
    }
    public void Update()
    {
       
        if (!isRewardReady&&time)
        {
            DateTime currentDatetime = DateTime.Now;
            DateTime rewardClaimDatetime = DateTime.Parse(getRewardTime(currentDatetime));
            //get total Hours between this 2 dates
            int elapsedHour = getRewardHour(currentDatetime, rewardClaimDatetime);
            int elapsedMins = getRewardMin(currentDatetime, rewardClaimDatetime);
            int elapsedSec= getRewardSec(currentDatetime, rewardClaimDatetime);
            Hrs.text = elapsedHour.ToString();
            Sec.text = elapsedSec.ToString();
            Min.text = elapsedMins.ToString();

        }
        
    } 

        #endregion
    
    #region WithoutTime
        
        public void checkTime()
        {
            DateTime currentDatetime = DateTime.Now;
            DateTime rewardClaimDatetime = DateTime.Parse(getRewardTime(currentDatetime));
            double elapsedHours = (currentDatetime - rewardClaimDatetime).TotalHours;
            Min.gameObject.SetActive(false);
            Sec.gameObject.SetActive(false);
            if (elapsedHours >= nextRewardDelay)
            {
                
                Hrs.text = "24 Hrs";
                ActivateReward();
            
            }
            else
            {
                PlayerPrefs.SetInt("NotifyTime", ((int) (nextRewardDelay - elapsedHours)));
                Hrs.text = ((int)(nextRewardDelay-elapsedHours)).ToString();
                DeactivateReward();
            }
        }
        

        #endregion

    #region Debugging

        public void AdvanceHour()
        {
            DateTime rewardClaimDatetime = DateTime.Parse(getRewardTime(DateTime.Now));
            DateTime AddHour = rewardClaimDatetime.Subtract(new TimeSpan(1, 0, 0));
            PlayerPrefs.SetString("Reward_Claim_Datetime", AddHour.ToString());
            if (!time)
            {
                checkTime();
            }
        }
        public void AdvanceDay()
        {
            //DateTime rewardClaimDatetime = DateTime.Parse( PlayerPrefs.GetString("Reward_Claim_Datetime"));

            DateTime rewardClaimDatetime = DateTime.Parse(getRewardTime(DateTime.Now));
            DateTime AddHour = rewardClaimDatetime.Subtract(new TimeSpan((int)nextRewardDelay, 0, 0));
            PlayerPrefs.SetString("Reward_Claim_Datetime", AddHour.ToString());
            if (!time)
            {
                checkTime();
            }
        }


        public void ResetDailyReward()
        {
            nextRewardIndex = 0;
            for (int i = 0; i < rewardButton.Length; i++)
            {
                PlayerPrefs.DeleteKey("ReadyForClaim" +i);
                 PlayerPrefs.DeleteKey("Claimed" +i);
            }
            PlayerPrefs.SetInt ( "Next_Reward_Index", nextRewardIndex);
            PlayerPrefs.SetString ( "Reward_Claim_Datetime", DateTime.Now.ToString ( ) );
            
            DeactivateReward ( );
            Init();
       
        }

        #endregion
    
    #region RewardState
        
         
       public void ActivateReward ( ) 
        {
            isRewardReady = true;
            print("Ready for claimed");
            rewardButton[nextRewardIndex].interactable = true;
            SelectedButton[nextRewardIndex].GetComponent<Image>().color = selectedColor;
            LockButton[nextRewardIndex].SetActive(false);
            // SelectedButton[nextRewardIndex].GetComponent<Button>().interactable=false;
            // SelectedButton[nextRewardIndex].GetComponent<Image>().enabled=true;

            PlayerPrefs.SetString ( "ReadyForClaim" +nextRewardIndex,"true");

            if (PlayerPrefs.HasKey("ReadyForClaim" + nextRewardIndex))
            {
                ClaimBtn.SetActive(true);
                
                RewardedClaimBtn.SetActive(false);
            }
        }

        void DeactivateReward ( ) 
        {
            isRewardReady = false;
            
            if (!PlayerPrefs.HasKey("ReadyForClaim" + nextRewardIndex))
            {
                ClaimBtn.SetActive(false);
                RewardedClaimBtn.SetActive(true);
            }
        }
        
        public  void OnClaimButtonClick ( ) 
        {
            rewardButton[nextRewardIndex].interactable = false;
         //   rewardButton[nextRewardIndex].GetComponent<Image>().sprite = Claimed;
         
            Collected[nextRewardIndex].SetActive(true);

            LockButton[nextRewardIndex].SetActive(false);
            // SelectedButton[nextRewardIndex].GetComponent<Button>().interactable=false;
            // SelectedButton[nextRewardIndex].GetComponent<Image>().enabled=true;

            PlayerPrefs.SetString ( "Claimed" +nextRewardIndex,"true");
            
            //Save next reward index

      
            if (nextRewardIndex >= Weeks[0].Rewarditems.Length - 1)
            {
                PlayerPrefs.SetInt("Week",0);
                nextRewardIndex = 0;
                print("reset");
               ResetDailyReward();
            }
            else
            {
                CheckRewardType();
                nextRewardIndex++;
                if (nextRewardIndex == 14)
                {
                    PlayerPrefs.SetInt("Week",1);
                    ResetDailyReward();
                }
                //rewardButton[nextRewardIndex].GetComponent<Image>().sprite = Ready;
                foreach (GameObject s in SelectedButton)
                {
                    s.SetActive(false);
                }
                SelectedButton[nextRewardIndex].SetActive(true);
                SelectedButton[nextRewardIndex].GetComponent<Image>().color = selectedColor;
                LockButton[nextRewardIndex].SetActive(false);
                // SelectedButton[nextRewardIndex].GetComponent<Button>().interactable=true;
                // SelectedButton[nextRewardIndex].GetComponent<Image>().enabled=true;

            }

            PlayerPrefs.SetInt ( "Next_Reward_Index", nextRewardIndex);
            float x;
            if (PlayerPrefs.GetInt("Next_Reward_Index") > 11)
            {
                x = 1f;
            }
            else
            {
                x= PlayerPrefs.GetInt ( "Next_Reward_Index") / 14f;
            }

            //Save DateTime of the last Claim Click
            PlayerPrefs.SetString ( "Reward_Claim_Datetime", DateTime.Now.ToString ( ) );
            DeactivateReward ( );
        }


      
        #endregion

    #region MethodVariable
        
        public string getRewardTime(DateTime currentDatetime) => PlayerPrefs.GetString("Reward_Claim_Datetime", currentDatetime.ToString()); 
        public int getRewardHour(DateTime currentDatetime,DateTime rewardClaimDatetime)=>(currentDatetime - rewardClaimDatetime).Hours;
        public int getRewardMin(DateTime currentDatetime,DateTime rewardClaimDatetime)=>(currentDatetime - rewardClaimDatetime).Minutes;
        public int getRewardSec(DateTime currentDatetime,DateTime rewardClaimDatetime)=>(currentDatetime - rewardClaimDatetime).Seconds;
        

        #endregion

    #region GenericMethods
    

    public void Hide()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void Display()
    {
        GetComponent<Canvas>().enabled = true;
        Init();
    }

    public void ClaimByRewarded()
    {
        ActivateReward();
        OnClaimButtonClick();
        dailyRewarded = false;


    }

    public void ShowRewarded()
    {
     
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            dailyRewarded = true;
            // AdsController.Instance.ShowRewardedMenu();
        }
        else
        {
            StartCoroutine(RewardedMenu.instance.NoAccess());
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.DeleteKey("DailyRewardGun" + rewardGunIndex);
    }

    private int rewardGunIndex;
    public void CheckRewardType()
    {
            nextRewardIndex =  PlayerPrefs.GetInt ( "Next_Reward_Index");
           
            int weekindex;
        if (PlayerPrefs.GetInt("Week") == 0)
        {
            weekindex = 0;
        }
        else
        {
            weekindex = 1;

        }
        if (Weeks[weekindex].Rewarditems[nextRewardIndex].weapon)
        {
            rewardGunIndex = Weeks[weekindex].Rewarditems[nextRewardIndex].index;
            PlayerPrefs.SetInt("DailyRewardGun" + Weeks[weekindex].Rewarditems[nextRewardIndex].index, 10);
            Weapon_Manager.Instance.UpdateStatus();
            
           
        }
        if (Weeks[weekindex].Rewarditems[nextRewardIndex].grenades)
        {
           
            PlayerPrefs.SetInt("Grenades", Weeks[weekindex].Rewarditems[nextRewardIndex].index);
        }
        if (Weeks[weekindex].Rewarditems[nextRewardIndex].xp)
        {
        
            PlayerPrefs.SetInt("XP", Weeks[weekindex].Rewarditems[nextRewardIndex].index);
           
        }
        else 
        {
            PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") + Weeks[weekindex].Rewarditems[nextRewardIndex].cashReward);
            MenuManager.Instance.CashShown();
        }
    }

    #endregion
     
}
[Serializable]
 public class Week
 {
     public Rewarditem[] Rewarditems;

 }
[Serializable]
public class Rewarditem
{
    public string rewardName;
    public Sprite rewardicons;
    public bool weapon;
    public bool grenades;
    public bool xp;
    public int index;
    public int cashReward;

}
