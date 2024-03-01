using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    private bool _IsShow;
    [SerializeField] private GameObject _SaveSlotBG;
    [SerializeField] private GameObject _ConfirmOverWrite;
    [SerializeField] private Button[] _Button;
    [SerializeField] private Button[] _Save;
    [SerializeField] private Image[] _Images;
    private bool _IsUpdate;
    private int _SaveSlotID;

    private void Start()
    {
        _IsShow = false;
        _IsUpdate = false;
        _SaveSlotBG.SetActive(false);
        _ConfirmOverWrite.SetActive(false);
    }

    void Update()
    {
        if (_IsShow & !_IsUpdate)
        {
            _SaveSlotBG.SetActive(true);
            for (int i = 0; i < _Save.Length; i++)
            {
                _Button[i].enabled = false;
                bool HaveSave = GameSaveManager._Instance.IsItHaveSave(i);
                if (HaveSave)
                {
                    // show item
                }
            }
            _IsUpdate = true;
        }
        if (!_IsShow)
        {
            _SaveSlotBG.SetActive(false);
            _ConfirmOverWrite.SetActive(false);
            for (int i = 0; i < _Button.Length; i++)
            {
                _Button[i].enabled = true;
            }
        }
    }

    public void ShowSaveSlot()
    {
        _IsShow = true;
        _IsUpdate = false;
    }

    public void CloseSaveSlot()
    {
        _IsShow = false;
    }

    private void SaveGameData(int i)
    {
        GameSaveManager._Instance.SaveGame(i);
        CloseSaveSlot();
    }

    public void SaveOverWrite(int i)
    {
        if (GameSaveManager._Instance.IsItHaveSave(i))
        {
            _SaveSlotID = i;
            _ConfirmOverWrite.SetActive(true);
        }
        else
        {
            SaveGameData(i);
        }
    }

    public void ConfirmOW()
    {
        SaveGameData(_SaveSlotID);
        _SaveSlotID = 0;
        CloseSaveSlot();
    }

    public void DontOW()
    {
        _ConfirmOverWrite.SetActive(false);
    }
}
