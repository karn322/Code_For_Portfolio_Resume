using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKey : MonoBehaviour
{
    private int _ScoreCount = 0;
    [SerializeField] private GameObject _Key;
    private Transform _PlayerPosition;
    private bool _Spawned;

    private void Awake()
    {
        _PlayerPosition = GameObject.Find("Player").GetComponent<Transform>();
        _Spawned = false;
    }

    private void Update()
    {
        if (_ScoreCount == 3 && !_Spawned)
        {
            Instantiate(_Key, _PlayerPosition.position + new Vector3(-5, 0, 0), Quaternion.identity);
            _Spawned = true;
        }
    }

    public void GetScore()
    {
        _ScoreCount++;
    }
}
