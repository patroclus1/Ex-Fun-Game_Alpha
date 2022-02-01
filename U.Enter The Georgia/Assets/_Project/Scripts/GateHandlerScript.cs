using UnityEngine;

public class GateHandlerScript : MonoBehaviour
{
    [SerializeField] private GameObject firstGate;
    [SerializeField] private GameObject secondGate;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private VictoryScript victoryScript;

    private void Update()
    {
        if (spawner.Wave == 2) Destroy(firstGate);
        if (spawner.Wave == 3) Destroy(secondGate);
        if (spawner.Wave == 4) victoryScript.Victory();
    }
}
