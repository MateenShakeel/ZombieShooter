using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceCalculator : MonoBehaviour
{
    [SerializeField] bool _canCalculate;
    [SerializeField] Text _distanceText;
    
    Vector3 _originalPosition;
    float _distanceCovered;
    float _distanceToCover;

    // Start is called before the first frame update
    void Start()
    {
        var level = GameplayHandler.Instance.GetCurrentCampaignLevel();
        if(GameManager.Instance.SelectedMode == 1)
        {
        _distanceText.gameObject.SetActive(true);
            _distanceToCover = level.Distance;

        }
        _originalPosition = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.SelectedMode == 1)
        {
            CheckForCompletion();
            CalculateDistance();
        }
    }

    public void SetCanCalculate(bool status)
    {
        _canCalculate = status;
    }
    public float GetDistanceCovered()
    {
        return _distanceCovered;
    }

    void CalculateDistance()
    {
        if (_canCalculate)
        {
            _distanceCovered = Vector3.Distance(_originalPosition, transform.position);
            _distanceText.text = "Distance : "  + Mathf.RoundToInt(_distanceCovered).ToString() + "m" + "/" + _distanceToCover.ToString() + "m";
        }
        else
            return;
    }

    void CheckForCompletion()
    {
        if (_distanceCovered >= _distanceToCover)
        {
            SetCanCalculate(false);
            UIHandler.Instance.OpenLevelCompletePanelTime(1f);
        }
    }
}
