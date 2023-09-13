using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour
{
    
    #region GlobalInstance

    public static cameraShake localInstance;
    public static cameraShake Instance
    {
        get
        {
            localInstance = GameObject.FindObjectOfType<cameraShake>();
            return localInstance;
        }
    }
    
    #endregion
    
   public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalpos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-0.02f, 0.02f) * magnitude;
            float y = Random.Range(-0.02f, 0.02f) * magnitude;
            transform.localPosition = new Vector3(x,originalpos.y+y,originalpos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalpos;
    }

   public void ShakeStart()
   {
       StartCoroutine(Shake(0.15f, 4));
   }  
   


}
