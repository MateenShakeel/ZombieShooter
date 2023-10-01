using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public AudioClip particleSound;

    private void Start()
    {
        Destroy(gameObject, 15f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            SoundManager.instance.PlayEffect(particleSound);
            other.transform.GetComponent<weaponselector>().AmmoPickup();
            gameObject.SetActive(false);
        }
    }
}
