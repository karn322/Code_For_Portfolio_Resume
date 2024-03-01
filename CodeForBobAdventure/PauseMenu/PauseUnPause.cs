using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUnPause : MonoBehaviour
{
    [SerializeField] private GameObject _GameObject;
    [SerializeField] SaveSlot _SaveSlot;
    [SerializeField] LoadSlot _LoadSlot;
    private bool _IsPause;

    void Start()
    {
        _IsPause = false;
        _GameObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _IsPause = !_IsPause;
        }
        if (_IsPause)
        {
            _GameObject.gameObject.SetActive(true);
        }
        if (!_IsPause)
        {
            _GameObject.gameObject.SetActive(false);
            _SaveSlot.CloseSaveSlot();
            _LoadSlot.CloseLoadSlot();
        }
    }

    public bool IsPause()
    {
        return _IsPause;
    }

    public void UnPause()
    {
        _IsPause = false;
    }
}
