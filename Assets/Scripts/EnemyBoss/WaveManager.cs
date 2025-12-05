using UnityEngine;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{
    [Header("Pivotes de respawn")]
    public Transform[] spawnPoints;


    [Header("Prefabs de enemigos")]
    public GameObject basicEnemy;
    public GameObject advancedEnemy;


    [Header("Cantidad por oleada")]
    public int basicWaveAmount = 1;
    public int advancedWaveAmount = 5;


    private int currentWave = 0;


    public void StarWave()
    {
        currentWave++;

        if (currentWave == 1)
            SpawnWave(basicEnemy, basicWaveAmount); // 70%
        else if (currentWave == 2) 
            SpawnWave(advancedEnemy, advancedWaveAmount); //50%
    }

    private void SpawnWave(GameObject prefab, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int r = Random.Range(0, spawnPoints.Length);
            Instantiate(prefab, spawnPoints[r].position, Quaternion.identity);

            
        }
    }
}
