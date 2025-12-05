using UnityEngine;

public class RespawnEnemy : MonoBehaviour
{
    public float respawnTimer = 5f;
    private float timer;


    public Transform[] respawnPoints;
    public GameObject enemyPrefab;


    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= respawnTimer)
        {
            timer= 0;
            int r = Random.Range(0, respawnPoints.Length);
            Instantiate(enemyPrefab, respawnPoints[r].position, Quaternion.identity);

        }
    }
}
