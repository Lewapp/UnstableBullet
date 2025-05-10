using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject creditsMenu;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowCredits()
    {
        creditsMenu.SetActive(true);
    }

    public void HideCredits()
    {
        creditsMenu.SetActive(false);
    }
}
