using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanHit : MonoBehaviour
{
    private SpawnKey _SpawnKey;
    private bool _IsHit;

    private void Awake()
    {
        _SpawnKey = GameObject.Find("SpawnKey").GetComponent<SpawnKey>();
        _IsHit = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") && !_IsHit)
        {
            _IsHit = true;
            _SpawnKey.GetScore();
        }
    }
}
