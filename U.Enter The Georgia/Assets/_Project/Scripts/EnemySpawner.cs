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
    private float moveSpeed;
    private static bool gameDone;

    public static bool IsGameBeaten
    {
        get { return gameDone; }
    }

    public int Wave
    {
        get { return waveNumber; }
    }

    public int EnemiesLeft
    {
        get { return currentNumberOfEnemies; }
    }


    void Awake()
    {
        moveSpeed = enemyPrefab.GetMoveSpeed;
        StartCoroutine(SpawnEnemies());   
    }

    private void UpdateLevel()
    {
        if (waveNumber == 1)
        {
            transform.position = firstLevel.position;
            enemyPrefab.transform.localScale = Vector3.one;
            enemyPrefab.enemyHealth = 100;
            enemyPrefab.SetShootInterval = 4f;
        }
        else if (waveNumber == 2)
        {
            transform.position = secondLevel.position + Vector3.up * 2;
            enemyPrefab.transform.localScale = Vector3.one * 2;
            enemyPrefab.SetMoveSpeed = moveSpeed * 2;
            enemyPrefab.enemyHealth = 200;
            enemiesPerWave = 20;
        }
        else if (waveNumber == 3)
        {
            transform.position = thirdLevel.position;
            enemyPrefab.transform.localScale = Vector3.one;
            enemyPrefab.SetMoveSpeed = moveSpeed * 2;
            enemyPrefab.enemyHealth = 400;
            enemyPrefab.SetShootInterval = 0.25f;
            enemiesPerWave = 1;
        }
        else if (waveNumber >= 4)
        {
            gameDone = true;
            enemiesPerWave = 0;
        }    
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(timeBeforeSpawning);

        while (followTarget.isAlive && waveNumber<4)
        {
            if (currentNumberOfEnemies <= 0)
            {
                waveNumber++;
                float randDirection;
                float randDistance;
                UpdateLevel();
                for (int i = 0; i < enemiesPerWave; i++)
                {
                    randDirection = Random.Range(0, 360);
                    randDistance = Random.Range(10, 25);

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
