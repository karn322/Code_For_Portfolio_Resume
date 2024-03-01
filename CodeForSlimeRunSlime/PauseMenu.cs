using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuUI;

    public GameObject HTPMenuUi;

    private bool pause = false;

    private bool howToPlay = true;

    private void Start()
    {
        PauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !howToPlay)
        {
            pause = !pause;
        }
        if (pause)
        {
            PauseMenuUI.SetActive(true);
        }
        if (!pause)
        {
            PauseMenuUI.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !pause)
        {
            howToPlay = !howToPlay;
        }
        if (howToPlay)
        {
            HTPMenuUi.SetActive(true);
        }
        if (!howToPlay)
        {
            HTPMenuUi.SetActive(false);
        }

        if (pause || howToPlay)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

    }

    public void PauseScene()
    {   
        if (!howToPlay)
        pause = true;
    }

    public void ResumeButtom()
    {
        pause = false;    
    }

    public void RestartButtom()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenuButtom()
    {
        SceneManager.LoadScene("Main");
    }
}
