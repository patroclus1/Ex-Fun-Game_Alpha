using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void CreditsButton()
    {
        print("Thanks for playing!");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
