using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    [SerializeField] private EnemyScript enemyPrefab;
    [SerializeField] private PlayerScript followTarget;
    [SerializeField] private int enemiesPerWave;
    [SerializeField] private float timeBeforeSpawning;
    [SerializeField] private float timeBetweenEnemies;
    [SerializeField] private float timeBetweenWaves;

    [SerializeField] private int currentNumberOfEnemies;

    private int waveNumber;

    void Awake()
    {
         StartCoroutine(SpawnEnemies());   
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(timeBeforeSpawning);

        while (followTarget.isAlive)
        {
            if (currentNumberOfEnemies <= 0)
            {
                waveNumber++;
                float randDirection;
                float randDistance;

                for (int i = 0; i < enemiesPerWave; i++)
                {
                    randDirection = Random.Range(0, 360);
                    randDistance = Random.Range(10, 25);

                    float posX = transform.position.x + (Mathf.Cos(randDirection * Mathf.Deg2Rad) * randDistance);
                    float posY = transform.position.z + (Mathf.Sin(randDirection * Mathf.Deg2Rad) * randDistance);
                    var spawnedEnemy = Instantiate(enemyPrefab, new Vector3(posX, 1f, posY), transform.rotation);
                    spawnedEnemy.Initialize(followTarget, this);
                    currentNumberOfEnemies++;

                    yield return new WaitForSeconds(timeBetweenEnemies);
                }
            }
            yield return new WaitForSeconds (timeBetweenWaves);
        }
    }

    public void EnemyKilled()
    {
        currentNumberOfEnemies--;
    }
}
