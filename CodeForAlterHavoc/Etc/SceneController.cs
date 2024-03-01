using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] bool _IsEscChangeScene;
    [SerializeField] string _Name;

    [SerializeField] GameObject _GameObjectCloseOnEsc;
    [SerializeField] GameObject _DeleteOpject;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_IsEscChangeScene)
            {
                DeleteObjectbeforeLoad();
                SceneChange(_Name);
            }

            if (_GameObjectCloseOnEsc != null)
            {
                _GameObjectCloseOnEsc.SetActive(false);
            }
        }
    }

    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1.0f;
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    public void DeleteObjectbeforeLoad()
    {
        if (_DeleteOpject != null)
        {
            Destroy(_DeleteOpject);
        }
    }
}
