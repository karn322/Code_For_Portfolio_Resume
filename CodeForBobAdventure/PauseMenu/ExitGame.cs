using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    [SerializeField] GameObject _ExitConfirm;

    private void Start()
    {
        DontExit();
    }

    public void ConfirmExit()
    {
        _ExitConfirm.SetActive(true);
    }

    public void DontExit()
    {
        _ExitConfirm.SetActive(false);
    }

    public void DoExit()
    {
        changeScene.LoadNextScene("MainMenu");
    }
}
