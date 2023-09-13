using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    
    public int destroyTime = 4;
    
    private void Start()
    {
        Destroy(this.gameObject,destroyTime);
    }

}
