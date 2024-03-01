using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats _PlayerStats;
    CircleCollider2D _PlayerCollector;
    [HideInInspector] public float _PullSpeed;

    private void Start()
    {
        _PlayerStats = FindObjectOfType<PlayerStats>();
        _PlayerCollector = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        _PlayerCollector.radius = _PlayerStats.CurrentMagnetRange + _PlayerStats._MagnetRangeBoost;
        _PullSpeed = _PlayerStats.CurrentMoveSpeed + _PlayerStats._SpeedBoost + 1;
    }

    private void OnTriggerEnter2D(Collider2D collision) // item enter magnet range
    {
        if (collision.gameObject.TryGetComponent(out ICollectable collectable))
        {     
            collectable.InMagnetRange();
        }
    }
}
