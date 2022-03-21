using UnityEngine;

public class GateHandlerScript : MonoBehaviour
{
    [SerializeField] private GameObject _firstGate;
    [SerializeField] private GameObject _secondGate;
    [SerializeField] private EnemySpawner _spawner;
    [SerializeField] private VictoryScript _victoryScript;

    private void Update()
    {
        if (_spawner.Wave == 2) Destroy(_firstGate);
        if (_spawner.Wave == 3) Destroy(_secondGate);
        if (_spawner.Wave == 4) _victoryScript.Victory();
    }
}
