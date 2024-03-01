using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFarmPoint : MonoBehaviour
{
    [SerializeField] GameObject[] _GameObjects;
    private int _index = 0;

    void Start()
    {
        for (int i = 0; i < _GameObjects.Length; i++)
        {
            _GameObjects[i].SetActive(false);
        }
        _GameObjects[_index].SetActive(true);
    }

    public void ChangeUp()
    {
        _GameObjects[_index].SetActive(false);
        _index++;
        if (_index > _GameObjects.Length - 1)
        {
            _index = 0;
        }
        _GameObjects[_index].SetActive(true);
    }

    public void ChangeDown()
    {
        _GameObjects[_index].SetActive(false);
        _index--;
        if (_index < 0)
        {
            _index = _GameObjects.Length - 1;
        }
        _GameObjects[_index].SetActive(true);
    }
}
