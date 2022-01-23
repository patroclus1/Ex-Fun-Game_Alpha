using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform firstLevel;
    [SerializeField] private Transform secondLevel;
    [SerializeField] private Transform thirdLevel;

    [SerializeField] private EnemyScript enemyPrefab;
    [SerializeField] private PlayerScript followTarget;
    [SerializeField] private int enemiesPerWave;
    [SerializeField] private float timeBeforeSpawning;
    [SerializeField] private float timeBetweenEnemies;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private int currentNumberOfEnemies;
    private int waveNumber;

    public int wave
    {
        get { return waveNumber; }
    }

    public int enemiesLeft
    {
        get { return currentNumberOfEnemies; }
    }


    void Awake()
    {
         StartCoroutine(SpawnEnemies());   
    }

    private void UpdateLevel()
    {
        if (waveNumber == 1)
        {
            transform.position = firstLevel.position;
            enemyPrefab.enemyHealth = 100;
        }
        else if (waveNumber == 2)
        {
            transform.position = secondLevel.position;
            enemyPrefab.enemyHealth = 200;
            enemiesPerWave = 20;
        }
        else if (waveNumber == 3)
        {
            transform.position = thirdLevel.position;
            enemyPrefab.enemyHealth = 400;
            enemiesPerWave = 1;
        }
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

                    UpdateLevel();

                    InstantiateEnemies(randDirection, randDistance);

                    yield return new WaitForSeconds(timeBetweenEnemies);
                }
            }
            yield return new WaitForSeconds (timeBetweenWaves);
        }
    }

    private void InstantiateEnemies(float position, float distance)
    {
        float posX = transform.position.x + (Mathf.Cos(position * Mathf.Deg2Rad) * distance);
        float posY = transform.position.z + (Mathf.Sin(position * Mathf.Deg2Rad) * distance);
        var spawnedEnemy = Instantiate(enemyPrefab, new Vector3(posX, 1f, posY), transform.rotation);
        spawnedEnemy.Initialize(followTarget, this);
        currentNumberOfEnemies++;
    }

    public void EnemyKilled()
    {
        currentNumberOfEnemies--;
    }
}
