using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _firstLevel;
    [SerializeField] private Transform _secondLevel;
    [SerializeField] private Transform _thirdLevel;

    [SerializeField] private EnemyScript _enemyPrefab;
    [SerializeField] private PlayerScript _followTarget;
    [SerializeField] private int _enemiesPerWave;
    [SerializeField] private float _timeBeforeSpawning;
    [SerializeField] private float _timeBetweenEnemies;
    [SerializeField] private float _timeBetweenWaves;
    [SerializeField] private int _currentNumberOfEnemies;
                     private int _waveNumber;
                     private float _moveSpeed;
                     private static bool _gameDone;

    public static bool IsGameBeaten
    {
        get { return _gameDone; }
    }

    public int Wave
    {
        get { return _waveNumber; }
    }

    public int EnemiesLeft
    {
        get { return _currentNumberOfEnemies; }
    }


    void Awake()
    {
        _moveSpeed = _enemyPrefab.GetMoveSpeed;
        StartCoroutine(SpawnEnemies());   
    }

    private void UpdateLevel()
    {
        if (_waveNumber == 1)
        {
            transform.position = _firstLevel.position;
            _enemyPrefab.transform.localScale = Vector3.one;
            _enemyPrefab.EnemyHealth = 100;
            _enemyPrefab.SetShootInterval = 4f;
        }
        else if (_waveNumber == 2)
        {
            transform.position = _secondLevel.position + Vector3.up * 2;
            _enemyPrefab.transform.localScale = Vector3.one * 2;
            _enemyPrefab.SetMoveSpeed = _moveSpeed * 2;
            _enemyPrefab.EnemyHealth = 200;
            _enemiesPerWave = 20;
        }
        else if (_waveNumber == 3)
        {
            transform.position = _thirdLevel.position;
            _enemyPrefab.transform.localScale = Vector3.one;
            _enemyPrefab.SetMoveSpeed = _moveSpeed * 2;
            _enemyPrefab.EnemyHealth = 400;
            _enemyPrefab.SetShootInterval = 0.25f;
            _enemiesPerWave = 1;
        }
        else if (_waveNumber >= 4)
        {
            _gameDone = true;
            _enemiesPerWave = 0;
        }    
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(_timeBeforeSpawning);

        while (_followTarget.IsPlayerAlive && _waveNumber<4)
        {
            if (_currentNumberOfEnemies <= 0)
            {
                _waveNumber++;
                float randDirection;
                float randDistance;
                UpdateLevel();
                for (int i = 0; i < _enemiesPerWave; i++)
                {
                    randDirection = Random.Range(0, 360);
                    randDistance = Random.Range(10, 25);

                    InstantiateEnemies(randDirection, randDistance);

                    yield return new WaitForSeconds(_timeBetweenEnemies);
                }
            }
            yield return new WaitForSeconds (_timeBetweenWaves);
        }
    }

    private void InstantiateEnemies(float position, float distance)
    {
        float posX = transform.position.x + (Mathf.Cos(position * Mathf.Deg2Rad) * distance);
        float posY = transform.position.z + (Mathf.Sin(position * Mathf.Deg2Rad) * distance);
        var spawnedEnemy = Instantiate(_enemyPrefab, new Vector3(posX, 1f, posY), transform.rotation);
        spawnedEnemy.Initialize(_followTarget, this);
        _currentNumberOfEnemies++;
    }

    public void EnemyKilled()
    {
        _currentNumberOfEnemies--;
    }
}
