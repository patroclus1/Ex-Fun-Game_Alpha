using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIScript : MonoBehaviour
{
    public void OnRestartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnQuitButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnCreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }
}
