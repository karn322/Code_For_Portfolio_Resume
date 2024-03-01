using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenuPage()
    {
        SceneManager.LoadScene("Main");
    }

    public void CreditView()
    {
        SceneManager.LoadScene("Credit");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Map_1");
    }
}
