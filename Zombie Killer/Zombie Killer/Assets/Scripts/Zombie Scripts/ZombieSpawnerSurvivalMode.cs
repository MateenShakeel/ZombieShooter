using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnerSurvivalMode : MonoBehaviour
{
    public GameObject[] Zombies;
    public int ZombieSpawnDelay;
    public int ZombieSpawnAmount;

    private void Start()
    {
        StartCoroutine(SpawnZombie());
    }

    IEnumerator SpawnZombie()
    {
        yield return new WaitForSeconds(ZombieSpawnDelay);
        SoundManager.instance.PlayEffect(AudioClipsSource.Instance.ZombiesSpawningClip);
        UIHandler.Instance.ShowWaveAlert();
        for(int i = 0; i <  ZombieSpawnAmount; i++) 
        {
            int randomIndex = Random.Range(0, Zombies.Length);
            GameObject zombie =  Instantiate(Zombies[randomIndex] , transform);
            zombie.transform.parent = null;
            zombie.GetComponent<EnemyAI>().farDistance = 100f;
        }

        StartCoroutine(SpawnZombie());
    }


}
