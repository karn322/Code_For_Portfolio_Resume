using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeySystem : MonoBehaviour
{
    private bool _HaveKey;
    [SerializeField] private GameObject _KeyPicture;
    private bool _Showed;

    private void Start()
    {
        _KeyPicture.SetActive(false);
        _HaveKey = false;
        _Showed = false;
    }

    private void Update()
    {
        if (_HaveKey && !_Showed)
        {
            _Showed = true;
            _KeyPicture.SetActive(true);
        }

        if (!_HaveKey && _Showed)
        {
            _Showed = false;
            _KeyPicture.SetActive(false);
        }

    }

    public void GetKey()
    {
        _HaveKey = true;
    }

    public void TakeKey()
    {
        _HaveKey = false;
    }

    public bool CheckKey()
    {
        return _HaveKey;
    }
}
