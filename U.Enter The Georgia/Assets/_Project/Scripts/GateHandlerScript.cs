using UnityEngine;

public class GateHandlerScript : MonoBehaviour
{
    [SerializeField] private GameObject firstGate;
    [SerializeField] private GameObject secondGate;
    [SerializeField] private EnemySpawner spawner;

    private void Update()
    {
        if (spawner.wave == 2) Destroy(firstGate);
        if (spawner.wave == 3) Destroy(secondGate);
        if (spawner.wave == 4) print("Winner winner chicken dinner!");
    }
}
