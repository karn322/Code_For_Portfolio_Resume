using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobblingAnimate : MonoBehaviour
{
    float _Speed = 0.2f;
    Vector3 _Direction = new Vector3(0, 0.1f);
    [HideInInspector] public Vector3 _CurrentPosition;
    Rigidbody2D _Rigidbody;
    PlayerCollector _PlayerCollector;

    float _Time = 0.25f;
    int _range = 10;

    PickUp _PickUp;
    SpriteRenderer _SpriteRenderer;
    bool _SuperMagnet;

    void Start()
    {
        _PickUp = GetComponent<PickUp>();
        _Rigidbody = GetComponent<Rigidbody2D>();
        _PlayerCollector = FindObjectOfType<PlayerCollector>();
        _SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (_PickUp == null)
            return;

        if (GameManager._Instance.StopTime)
        {
            _PickUp._InPlayerMagnetRange = false;
        }

        if (_SuperMagnet)
        {
            transform.SetParent(_PlayerCollector.transform);
            Vector2 forceDirection = (_PlayerCollector.transform.position - transform.position).normalized;

            _Rigidbody.velocity = forceDirection * _PlayerCollector._PullSpeed * 3;
            return;
        }

        if (Vector3.Distance(_PlayerCollector.transform.position, transform.position) >= _range)
        {
            _SpriteRenderer.enabled = false;
            return;
        }
        else
        {
            _SpriteRenderer.enabled = true;
        }

        if(_Time > 0)
        {
            _Time -= Time.deltaTime;
        }

        if (_PickUp._InPlayerMagnetRange && _Time <= 0)
        {
            transform.SetParent(_PlayerCollector.transform);
            Vector2 forceDirection = (_PlayerCollector.transform.position - transform.position).normalized;

            _Rigidbody.velocity = forceDirection * _PlayerCollector._PullSpeed;
        }
        else
        {
            transform.position = _CurrentPosition + (_Direction * Mathf.Sin(Time.time / _Speed));
        }
    }

    public void Respawn(Vector3 pos)
    {
        _CurrentPosition = pos;
        if(_PickUp != null)
        _PickUp._InPlayerMagnetRange = false;
    }
}
