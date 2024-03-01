using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSlot : MonoBehaviour
{
    private bool _IsShow;
    private bool _IsUpdate;
    [SerializeField] private GameObject _LoadSlotBG;
    [SerializeField] private Button[] _Button;
    [SerializeField] private Button[] _LoadSlot;
    [SerializeField] private Image[] _Image;
    [SerializeField] private ShowItemNumber _ShowItemNumber;
    [SerializeField] private GameObject _ConfirmDelete;
    private int _LoadSlotID;
    private string _SceneName = "MainMenu";

    private void Start()
    {
        _IsShow = false;
        _IsUpdate = false;
        _LoadSlotBG.SetActive(false);
        _ConfirmDelete.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) & _IsShow)
        {
            _IsShow = false;
        }

        if (_IsShow & !_IsUpdate)
        {
            _LoadSlotBG.SetActive(true);

            if (_Button.Length > 0)
            {
                for (int i = 0; i < _Button.Length; i++)
                {
                    if (SceneManager.GetActiveScene().name == _SceneName)
                    {
                        _Button[i].interactable = false;
                    }
                    else
                    {
                        _Button[i].enabled = false;
                    }
                }
            }
                
            for (int i = 0; i < _LoadSlot.Length; i++)
            {
                bool HaveSave = GameSaveManager._Instance.IsItHaveSave(i);
                if (!HaveSave)
                {
                    _LoadSlot[i].enabled = false;
                    _Image[i].color = Color.gray;
                }
                else
                {
                    _LoadSlot[i].enabled = true;
                    _Image[i].color = Color.white;

                    // add show item
                }
            }
            _IsUpdate = true;
        }
        if (!_IsShow)
        {
            _LoadSlotBG.SetActive(false);
            if (_Button.Length > 0)
            {
                for (int i = 0; i < _Button.Length; i++)
                {
                    if (SceneManager.GetActiveScene().name == _SceneName)
                    {
                        _Button[i].interactable = true;
                    }
                    else
                    {
                        _Button[i].enabled = true;
                    }
                }
            }
        }
    }

    public void ShowLoadSlot()
    {
        _IsShow = true;
        _IsUpdate = false;
    }

    public void CloseLoadSlot()
    {
        _IsShow = false;
    }

    public void LoadGameData(int i)
    {
        GameSaveManager._Instance.LoadGame(i);
        if (_SceneName == SceneManager.GetActiveScene().name)
        {
            changeScene.LoadNextScene("Map");
        }
        if (_ShowItemNumber != null)
        {
            _ShowItemNumber.LoadHowManyItem();
        }
        CloseLoadSlot();
    }

    public void DeleteConfirm(int i)
    {
        if (!GameSaveManager._Instance.IsItHaveSave(i))
            return;
        _ConfirmDelete.SetActive(true);
        _LoadSlotID = i;
    }

    public void DeleteSave()
    {
        GameSaveManager._Instance.DeleteSaveFile(_LoadSlotID);
        _IsUpdate = false;
        DontDelete();
    }

    public void DontDelete()
    {
        _ConfirmDelete.SetActive(false);
    }

}
