using UnityEngine;

public class VictoryScript : MonoBehaviour
{
    [SerializeField] private GameObject victoryScreen;

    public void Victory()
    {
        if (EnemySpawner.IsGameBeaten)
        {
            victoryScreen.SetActive(true);
        }
    }
}
