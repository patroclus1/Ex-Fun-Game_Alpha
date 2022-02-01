using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

    private AudioSource backgroundMusic;

    private void Awake()
    {
        backgroundMusic = GetComponent<AudioSource>();
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void BackFromCredits()
    {
        SceneManager.LoadScene("Menu");
    }
}
