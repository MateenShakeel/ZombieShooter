using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointHandler : MonoBehaviour, IPointerUpHandler,IPointerDownHandler
{
    // public GameObject MainPanel;
    // public GameObject bottom;
    // public GameObject player;
    // public Vector3 OriginalPos;
    // public Vector3 OriginalScale;
    public GameObject[] UIButtons;
    private bool isPause;

    private void Awake()
    {    
        foreach (GameObject uiButton in UIButtons)
        {
            uiButton.SetActive(false);
        }
        
    }


    // public void OnMouseDown()
    // {
    //     // MainPanel.SetActive(false);
    //     // OriginalPos = gameObject.transform.position;
    //     // gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + (float) 3,gameObject.transform.localScale.y + (float) 3,gameObject.transform.localScale.z + (float) 3);
    //     
    // }
    //
    // public void OnMouseUp()
    // {
    //     // MainPanel.SetActive(true);
    //     // gameObject.transform.position = OriginalPos;     
    //     // gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x - (float) 3,gameObject.transform.localScale.y - (float) 3,gameObject.transform.localScale.z - (float) 3);
    // }

    public void OnPointerUp(PointerEventData eventData)
    {
        // MainPanel.SetActive(true);
        // bottom.SetActive(true);
        Debug.Log("Upppppppppppppppppppppppppp");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPause = true;

    }
    
    
}
