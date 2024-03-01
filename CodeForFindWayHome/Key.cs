using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private KeySystem _KeySystem;
    private bool _HaveKey;
    [SerializeField] private GameObject _Key;

    private void Awake()
    {
        _KeySystem = GameObject.Find("KeySystem").GetComponent<KeySystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _HaveKey = _KeySystem.CheckKey();
            if (!_HaveKey)
            {
                _KeySystem.GetKey();
                Destroy(_Key);
            }
        }
    }
}
