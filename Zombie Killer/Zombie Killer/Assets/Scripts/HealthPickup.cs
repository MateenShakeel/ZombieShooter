using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public AudioClip particleSound;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            SoundManager.instance.PlayEffect(particleSound);
            other.transform.GetComponent<playercontroller>().HealthPickup();
            gameObject.SetActive(false);
        }
    }
}
