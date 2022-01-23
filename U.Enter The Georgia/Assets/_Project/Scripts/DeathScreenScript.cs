using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenScript : MonoBehaviour
{
    public void OnRestartButton()
    {
        print("Restart!");
        SceneManager.LoadScene("Game");
    }

    public void OnQuitButton()
    {
        print("Quit!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
