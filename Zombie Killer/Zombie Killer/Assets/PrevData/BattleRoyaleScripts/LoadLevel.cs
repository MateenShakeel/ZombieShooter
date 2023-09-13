using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    
    private void Start()
    {
        Invoke(nameof(Load),4f);
    }
        
    private void Load()
    {
        SceneManager.LoadScene(1);
    }

    
}
