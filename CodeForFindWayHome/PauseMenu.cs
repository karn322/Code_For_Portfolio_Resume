using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _PauseMenuUI;
    private bool _Pause;

    void Start()
    {
        _PauseMenuUI.SetActive(false);
        _Pause = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _Pause = !_Pause;
        }
        if (_Pause)
        {
            _PauseMenuUI.SetActive(true);
            Time.timeScale = 0;
        }
        if (!_Pause)
        {
            _PauseMenuUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void ResumeButtom()
    {
        _Pause = false;
    }

    public void MainMenuButtom()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public bool IsPause()
    {
        return _Pause;
    }
}
