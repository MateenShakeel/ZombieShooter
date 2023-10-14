using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GunSelectionHandler : MonoBehaviour
{

    public static GunSelectionHandler instance;
    public GameData GData;

    public Text coins;

    public GameObject backBtn;
    public GameObject goBackBtn;
    public GameObject selectBtn;
    public GameObject forwardBtn;
    public GameObject pricePanel;
    public GameObject buyBtn;
    public Text priceText;

    public GameObject[] guns;
    public GameObject[] lockedState;
    public GameObject[] muzzleFlash;
    public Animation[] gunAnimators;
    public GameObject[] prompts;
    public int gunSelected;




    //public Image damageFill;
    //float damageAmt;
    //public Image rangeFill;
    //float rangeAmt;
    //public Image powerFill;
    //float powerAmt;
    //int i;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        gunSelected = 0;
        GameManager.Instance.weaponSelected = 0;
        // GameManager.instance.ResetConditions();
        //UpdateCoins();
        //DisplayDetails();
        ShowGun();
        //UnlockGuns();

    }

    public void UpdateCoins()
    {
        //coins.text = GData.coins.ToString();
    }

    private void Update()
    {
        HandleButtons();
    }

    public void GunSelection(int gunIndex)
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        GameManager.Instance.weaponSelected = gunIndex;
        gunSelected = gunIndex;

        ShowGun();

    }



    public void GoBack()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        GameManager.Instance.LoadScene("MainMenu");
    }

    //public void HidePriceAndBuyButton()
    //{
    //    pricePanel.SetActive(false);
    //    buyBtn.SetActive(false);
    //}

    public void NextBtn()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        if (gunSelected < 5)
        {
            gunSelected++;
            GunSelection(gunSelected);
        }
    }

    public void BackBtn()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        if (gunSelected > 0)
        {
            gunSelected--;
            GunSelection(gunSelected);
        }
    }

    void HandleButtons()
    {
        if (gunSelected == 0)
        {
            backBtn.SetActive(false);
        }
        else
        {
            backBtn.SetActive(true);
        }

        if (gunSelected == 5)
        {
            forwardBtn.SetActive(false);
        }
        else
        {
            forwardBtn.SetActive(true);
        }
    }

    public void ShowGun()
    {
        DisplayGunHandler(gunSelected);
    }

    void DisplayGunHandler(int index)
    {
        if (!GData.Weapons[index].isUnlocked)
            selectBtn.SetActive(false);
        else
            selectBtn.SetActive(true);

        if (index == 0)
        {
            guns[0].SetActive(true);
        }
        for (int i = 0; i < guns.Length; i++)
        {
            if (i == index)
            {
                guns[i].SetActive(true);
                continue;
            }
            guns[i].SetActive(false);
            //muzzleFlash[i].SetActive(false);
        }
    }

    public void PlayGame()
    {
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.GenericButtonClick);
        //  GameManager.instance.isModeSelection = true;
        GameManager.Instance.LoadScene("WeatherSelection");
    }

}
