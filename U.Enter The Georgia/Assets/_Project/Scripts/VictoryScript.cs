using UnityEngine;

public class VictoryScript : MonoBehaviour
{
    [SerializeField] private GameObject _victoryScreen;

    public void Victory()
    {
        if (EnemySpawner.IsGameBeaten)
        {
            _victoryScreen.SetActive(true);
        }
    }
}
