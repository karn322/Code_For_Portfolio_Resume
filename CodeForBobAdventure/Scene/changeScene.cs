using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour
{
    public static void LoadNextScene(string _SceneName)
    {
        SceneManager.LoadScene(_SceneName);
    }
    
    public static void ExitApp()
    {
        Application.Quit();
    }
}
